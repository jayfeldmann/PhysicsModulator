using System.Collections;
using System.Collections.Generic;
using NAudio.Midi;
using UnityEngine;
using UnityEngine.UI;

public class SliderMidiTest : MonoBehaviour
{
    public void SendTestMidi(float value)
    {
        int device = MidiDeviceManager.activeMidiDevice;
        MidiMessage message = MidiMessage.ChangeControl(1,(int)value,1);
        if (device<0) return;
        
        using (MidiOut midiOut = new MidiOut(device))
        {
            var raw = message.RawData;
            midiOut.Send(raw);
        }
    }
}
