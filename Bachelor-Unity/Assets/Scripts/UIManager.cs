using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// UI Manager dient als Zentrum um das UI steuern zu können.
/// </summary>
public class UIManager : MonoBehaviour
{
    //Referenzen zu UI Elementen.
    public static UIManager instance;

    public GameObject settingsPanel;
    [FormerlySerializedAs("boidOptionsPanel")] public GameObject midiOptionsPanel;
    public LoadSaveSettings loadSaveSettings;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(this);
        }
    }

    //Öffnet Einstellungen bei ESC Taste.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableDisableSettings();
        }
    }


    //Öffnet oder Schließt Einstellungsfenster.
    public void EnableDisableSettings()
    {
        
        if (BoidOptions.isActive)
        {
            midiOptionsPanel.SetActive(false);
            return;
        }
        
        if (!Settings.isActive)
        {
            settingsPanel.SetActive(true);
            Settings.isActive = true;
        }
        else
        {
            if (loadSaveSettings.SaveSettings())
            {
                Settings.isActive = false;
                settingsPanel.SetActive(false);
            }
        }

        
    }
}
