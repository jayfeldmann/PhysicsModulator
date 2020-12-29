 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscHandler : DataHandler
{
    public int oscValue = 0;
    public int oscLowerValue = 0;
    public int oscUpperValue = 100;
    public string oscPath = "/";

    // Update is called once per frame
    void Update()
    {
        if(!_sendController.isActive) return;
        if (Settings.SendMode == SendMode.OSC)
        {
            
        }   
    }
}
