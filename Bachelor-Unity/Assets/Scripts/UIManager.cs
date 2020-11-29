using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject settingsPanel;
    public GameObject boidOptionsPanel;
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
            boidOptionsPanel.SetActive(false);
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
