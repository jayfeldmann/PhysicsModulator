 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscHandler : DataHandler
{
    // Update is called once per frame
    void Update()
    {
        if(!_sendController.isActive) return;
        if (Settings.SendMode == SendMode.OSC)
        {
            
        }   
    }
}
