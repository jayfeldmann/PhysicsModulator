using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
