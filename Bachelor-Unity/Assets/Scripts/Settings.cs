using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//SendMode enum um SendHandler zu aktivieren bzw deaktivieren.
public enum SendMode
{
    MIDI,
    OSC,
    DISABLED
}
/// <summary>
/// Globale Einstgellungen wie Midi-Device oder allgemeine Programmeinstellungen.
/// </summary>
public class Settings : MonoBehaviour
{
    //Einstellungsvariablen. Werden als JSON gespeicehrt.
    public static bool isActive = false;

    public static Settings instance;
    //Default OSC Settings
    public static int oscInPort = 1234;
    public static string oscOutIp = "127.0.0.1";
    public static int oscOutPort = 6161;
    public static SendMode SendMode = SendMode.MIDI;

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
