 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /// <summary>
 /// Klasse zum Senden von OSC Werten
 /// </summary>
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
            // TODO: Methode zum Versenden der Values ins Netzwerk
            // OSC Ist noch nicht implementiert und außer anzeichen im UI Nicht funktionsfähig.
        }   
    }
}
