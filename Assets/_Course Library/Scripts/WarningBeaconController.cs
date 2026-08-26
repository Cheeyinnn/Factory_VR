using UnityEngine;

public class WarningBeaconController : MonoBehaviour
{
    public Light warningLight;
    public Renderer beaconRenderer;
    public AudioSource warningAudio;

    public Color offColor = new Color(0.2f, 0f, 0f);
    public Color onColor = Color.red;

    public float flashSpeed = 4f;

    private bool isActive = false;

    void Update()
    {
        if (!isActive)
        {
            if (warningLight != null)
                warningLight.enabled = false;

            if (beaconRenderer != null)
                beaconRenderer.material.color = offColor;

            return;
        }

        float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
        bool flashOn = t > 0.5f;

        if (warningLight != null)
            warningLight.enabled = flashOn;

        if (beaconRenderer != null)
            beaconRenderer.material.color = flashOn ? onColor : offColor;
    }

    public void SetWarningState(bool active)
    {
        isActive = active;

        if (warningAudio != null)
        {
            if (active)
            {
                if (!warningAudio.isPlaying)
                    warningAudio.Play();
            }
            else
            {
                warningAudio.Stop();
            }
        }
    }
}