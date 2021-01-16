using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendmodeSwitch : MonoBehaviour
{
    public TMP_Dropdown sendModeDropdown;
    public void OnSendmodeDropdown()
    {
        if (sendModeDropdown.value == 0)
        {
            //Midi mode:
            Settings.SendMode = SendMode.MIDI;
        }
        else if (sendModeDropdown.value == 1)
        {
            Settings.SendMode = SendMode.OSC;
        }
        else
        {
            Settings.SendMode = SendMode.DISABLED;
        }
        
    }
}
