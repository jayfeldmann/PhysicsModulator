using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableDisableSettings();
        }
    }


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
