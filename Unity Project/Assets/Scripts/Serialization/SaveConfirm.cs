using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obsolete Klasse, soll ein Bestätigungspanel aufrufen um dsa Speichern der GameSettings zu bestätigen.
/// </summary>
public class SaveConfirm : MonoBehaviour
{
    public LoadSaveSettings loadSaveSettings;

    public void ClosePanel(bool saveSettings)
    {
        if (saveSettings)
        {
            loadSaveSettings.SaveSettings();        
        }
        gameObject.SetActive(false);
    }
}
