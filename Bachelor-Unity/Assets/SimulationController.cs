using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{

    public static bool simulateMovement = true;
    public static bool sendMidi = true;

    public Toggle movementToggle;
    public Toggle midiToggle;

    private void Start()
    {
        ToggleMidiSend();
        ToggleMovementControls();
    }

    public void ToggleMovementControls()
    {
        simulateMovement = movementToggle.isOn;
    }

    public void ToggleMidiSend()
    {
        sendMidi = midiToggle.isOn;
    }
}
