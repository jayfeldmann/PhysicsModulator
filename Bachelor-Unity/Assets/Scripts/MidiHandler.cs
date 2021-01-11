using NAudio.Midi;
using UnityEngine;

/// <summary>
/// MidiHandler ist ein Component, das zu Unity GameObjects hinzugefügt wird, um Midi Nachrichten an Geräte zu schicken.
/// </summary>
public class MidiHandler : DataHandler
{
    //Einstellungen Midi Nachricht
    public int midiChannel = 1;
    public int midiCC = 1;
    public int midiValue = 0;
    //Property generiert Valide, sendebereite Midi-Nachricht 
    public int sendMessage => MidiMessage.ChangeControl(midiCC, midiValue, midiChannel).RawData;
    
    // Fixed Update sendet, wenn Sendemodus auf Midi gestellt ist, 50 Midi Value updates pro sekunde
    void FixedUpdate()
    {
        if(!_sendController.isActive) return;
        if (Settings.SendMode == SendMode.MIDI)
        {
            SendMidi();
        }
    }
    
    //Sendet Midi Nachricht
    public void SendMidi()
    {
        var activeDevice = MidiDeviceManager.activeMidiDevice;
        if (activeDevice >=0)
        {
            using (MidiOut midiOut = new MidiOut(activeDevice))
            {
                midiOut.Send(sendMessage);
            }
        }
    }
}
