using UnityEngine;

public class DangerZoneController : MonoBehaviour
{
    [Header("Testing")]
    public bool testDangerActive = false;

    [Header("Colors")]
    public Color safeColor = Color.yellow;
    public Color dangerColor = Color.red;

    [Header("Flashing")]
    public float flashSpeed = 4f;

    private Renderer[] borderRenderers;

    void Awake()
    {
        borderRenderers = GetComponentsInChildren<Renderer>(true);

        Debug.Log("Danger Zone Renderers Found: " + borderRenderers.Length);
    }

    void Update()
    {
        if (testDangerActive)
        {
            // Flash between bright red and dark red
            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);

            Color darkRed = dangerColor * 0.25f;
            darkRed.a = 1f;

            Color flashingColor =
                Color.Lerp(dangerColor, darkRed, t);

            SetBorderColor(flashingColor);
        }
        else
        {
            SetBorderColor(safeColor);
        }
    }

    public void SetDangerState(bool active)
    {
        testDangerActive = active;
    }

    private void SetBorderColor(Color color)
    {
        foreach (Renderer borderRenderer in borderRenderers)
        {
            if (borderRenderer != null)
            {
                borderRenderer.material.color = color;
            }
        }
    }
}