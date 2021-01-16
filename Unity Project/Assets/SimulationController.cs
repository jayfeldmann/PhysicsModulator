using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{

    public static bool simulateMovement = true;

    public Toggle movementToggle;
    public Toggle midiToggle;

    private void Start()
    {
        ToggleMovementControls();
    }

    public void ToggleMovementControls()
    {
        simulateMovement = movementToggle.isOn;
    }
    
}
