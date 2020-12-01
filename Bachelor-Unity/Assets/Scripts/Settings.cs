using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SendMode
{
    MIDI,
    OSC
}

public class Settings : MonoBehaviour
{
    public static bool isActive = false;

    public static Settings instance;
    //Default OSC Settings
    public static int oscInPort = 1234;
    public static string oscOutIp = "127.0.0.1";
    public static int oscOutPort = 6161;
    public static SendMode SendMode;

    public OSC osc;



    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(this);
        }
        SetOscValues();
    }
    public void SetOscValues()
    {
        osc.inPort = Settings.oscInPort;
        osc.outPort = Settings.oscOutPort;
        osc.outIP = Settings.oscOutIp;
    }
    
}
