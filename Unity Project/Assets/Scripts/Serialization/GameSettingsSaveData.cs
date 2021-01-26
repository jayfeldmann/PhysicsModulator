using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Datenstruktur zum Speichern der Generellen Einstellungen wie Midigeräte und Netzwerkeinstellungen
/// </summary>
public class GameSettingsSaveData
{
    public string midiDevice;

    public int oscInPort;

    public int oscOutPort;

    public string oscOutIp;
}
