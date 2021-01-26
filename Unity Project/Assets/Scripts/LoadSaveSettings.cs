using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using TMPro;
using UnityEngine;

/// <summary>
/// Hilfsklasse um GameSettings zu speichern.
/// </summary>
public class LoadSaveSettings : MonoBehaviour
{
    public TMP_InputField inPortInputField;
    public TMP_InputField outPortInputField;
    public TMP_InputField outIpInputField;

    private void OnEnable()
    {
        LoadInputFieldTexts();
    }
    
    public void LoadInputFieldTexts()
    {
        inPortInputField.text = Settings.oscInPort.ToString();
        outPortInputField.text = Settings.oscOutPort.ToString();
        outIpInputField.text = Settings.oscOutIp;
    }

    public bool SaveSettings()
    {
        if (!Int32.TryParse(inPortInputField.text,out Settings.oscInPort))
        {
            Debug.Log("Invalid OSC In Port!");
            return false;
        }
        
        if (!Int32.TryParse(outPortInputField.text,out Settings.oscOutPort))
        {
            Debug.Log("Invalid OSC out Port!");
            return false;
        }

        Settings.oscOutIp = outIpInputField.text;

        Settings.instance.SetOscValues();
        return true;
    }

    
}
