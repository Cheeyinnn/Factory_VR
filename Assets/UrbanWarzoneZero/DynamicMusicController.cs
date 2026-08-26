using UnityEngine;

public class DynamicMusicController : MonoBehaviour
{
    [Header("Ambient Layer Settings")]
    [Range(0f, 1f)] public float ambientMasterVolume = 1f;
    [Range(0f, 100f)] public float ambientTension = 0f;
    [Tooltip("Speed of switching between tracks within the Ambient layer")]
    public float ambientTensionFade = 2f;
    [Tooltip("Speed of fading the entire Ambient layer in/out")]
    public float ambientMasterFade = 1f;
    public AudioSource[] ambientTracks;

    [Header("Combat Layer Settings")]
    [Range(0f, 1f)] public float combatMasterVolume = 0f;
    [Range(0f, 100f)] public float combatTension = 0f;
    [Tooltip("Speed of switching between tracks within the Combat layer")]
    public float combatTensionFade = 5f;
    [Tooltip("Speed of fading the entire Combat layer in/out")]
    public float combatMasterFade = 3f;
    public AudioSource[] combatTracks;

    // Internal variables to track interpolated master volumes
    private float currentAmbientMaster = 0f;
    private float currentCombatMaster = 0f;

    private void Start()
    {
        // Initialize internal volumes to match starting inspector values
        currentAmbientMaster = ambientMasterVolume;
        currentCombatMaster = combatMasterVolume;

        // Start all tracks simultaneously to maintain perfect rhythm sync
        PlayAll(ambientTracks);
        PlayAll(combatTracks);
    }

    private void Update()
    {
        // 1. Smoothly interpolate the Master Volume for each layer independently
        currentAmbientMaster = Mathf.Lerp(currentAmbientMaster, ambientMasterVolume, Time.deltaTime * ambientMasterFade);
        currentCombatMaster = Mathf.Lerp(currentCombatMaster, combatMasterVolume, Time.deltaTime * combatMasterFade);

        // 2. Update individual track volumes using Tension and their respective TensionFade speed
        UpdateLayer(ambientTracks, currentAmbientMaster, ambientTension, ambientTensionFade);
        UpdateLayer(combatTracks, currentCombatMaster, combatTension, combatTensionFade);
    }

    private void UpdateLayer(AudioSource[] tracks, float currentLayerVol, float targetTension, float tnsFade)
    {
        if (tracks == null || tracks.Length == 0) return;

        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i] == null) continue;

            // Determine how much this specific track should contribute based on Tension
            float weight = CalculateWeight(i, tracks.Length, targetTension);

            // Final target volume for the AudioSource
            float finalTargetVol = weight * currentLayerVol;

            // Apply the TensionFade speed to the AudioSource volume
            tracks[i].volume = Mathf.Lerp(tracks[i].volume, finalTargetVol, Time.deltaTime * tnsFade);
        }
    }

    private void PlayAll(AudioSource[] tracks)
    {
        foreach (var track in tracks)
        {
            if (track != null)
            {
                track.volume = 0f;
                track.loop = true;
                track.spatialBlend = 0f; // Force 2D to ensure music is heard regardless of distance
                track.Play();
            }
        }
    }

    private float CalculateWeight(int index, int total, float tension)
    {
        if (total <= 1) return 1f;

        // Calculate the center point for this track on the 0-100 scale
        float step = 100f / (total - 1);
        float center = index * step;

        // Calculate distance from current tension to this track's center
        float dist = Mathf.Abs(tension - center);

        // Linear crossfade: volume is 1 at center, 0 at 'step' distance
        return Mathf.Clamp01(1f - (dist / step));
    }

    // --- Public methods for external control (UI, triggers, etc.) ---
    public void SetAmbientTension(float val) => ambientTension = val;
    public void SetCombatTension(float val) => combatTension = val;
    public void SetAmbientMasterVolume(float val) => ambientMasterVolume = val;
    public void SetCombatMasterVolume(float val) => combatMasterVolume = val;
}