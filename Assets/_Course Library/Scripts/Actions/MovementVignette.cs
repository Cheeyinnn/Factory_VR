using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MovementVignette : MonoBehaviour
{
    [Header("References")]
    public Volume globalVolume;
    public Transform movementTarget;

    [Header("Vignette Settings")]
    public bool vignetteEnabled = true;

    [Range(0f, 1f)]
    public float movingIntensity = 0.45f;

    [Range(0f, 1f)]
    public float idleIntensity = 0f;

    public float transitionSpeed = 4f;

    [Header("Movement Detection")]
    public float movementThreshold = 0.1f;

    private Vignette vignette;
    private Vector3 lastPosition;

    void Start()
    {
        if (globalVolume != null &&
            globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = idleIntensity;
        }

        if (movementTarget != null)
        {
            lastPosition = movementTarget.position;
        }
    }

    void Update()
    {
        if (vignette == null || movementTarget == null)
            return;

        Vector3 currentPosition = movementTarget.position;

        Vector3 movement = currentPosition - lastPosition;
        movement.y = 0f;

        float speed =
            movement.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);

        bool isMoving = speed > movementThreshold;

        float targetIntensity =
            vignetteEnabled && isMoving
                ? movingIntensity
                : idleIntensity;

        vignette.intensity.value = Mathf.Lerp(
            vignette.intensity.value,
            targetIntensity,
            transitionSpeed * Time.deltaTime
        );

        lastPosition = currentPosition;
    }

    public void SetVignetteEnabled(bool enabled)
    {
        vignetteEnabled = enabled;

        if (!enabled && vignette != null)
        {
            vignette.intensity.value = idleIntensity;
        }
    }
}