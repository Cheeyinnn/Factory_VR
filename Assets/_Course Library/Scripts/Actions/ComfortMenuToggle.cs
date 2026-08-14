using UnityEngine;

public class ComfortMenuToggle : MonoBehaviour
{
    public GameObject comfortSettingsPanel;

    public void ToggleComfortMenu()
    {
        if (comfortSettingsPanel != null)
        {
            comfortSettingsPanel.SetActive(
                !comfortSettingsPanel.activeSelf
            );
        }
    }

    public void CloseComfortMenu()
    {
        if (comfortSettingsPanel != null)
        {
            comfortSettingsPanel.SetActive(false);
        }
    }
}