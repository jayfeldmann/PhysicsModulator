using NAudio.Midi;
using UnityEngine;

public class MidiHandler : DataHandler
{
    public int midiChannel = 1;
    public int midiCC = 1;
    public int midiValue = 0;
    public int sendMessage => MidiMessage.ChangeControl(midiCC, midiValue, midiChannel).RawData;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_sendController.isActive) return;
        if (Settings.SendMode == SendMode.MIDI)
        {
            SendMidi();
        }
    }
    
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
