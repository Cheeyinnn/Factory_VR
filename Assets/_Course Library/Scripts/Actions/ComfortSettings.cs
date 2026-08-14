using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ComfortSettings : MonoBehaviour
{
    [Header("Snap Turning")]
    public ActionBasedSnapTurnProvider snapTurnProvider;

    public void SetSnapTurnEnabled(bool enabled)
    {
        if (snapTurnProvider != null)
        {
            snapTurnProvider.enabled = enabled;
        }
    }
}