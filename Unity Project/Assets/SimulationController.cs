using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hilfsklasse zur Steuerung der Simulation dur Toggle im Header-Menu
/// </summary>

public class SimulationController : MonoBehaviour
{

    public static bool simulateMovement = true;

    public Toggle movementToggle;

    private void Start()
    {
        ToggleMovementControls();
    }

    public void ToggleMovementControls()
    {
        simulateMovement = movementToggle.isOn;
    }
    
}
