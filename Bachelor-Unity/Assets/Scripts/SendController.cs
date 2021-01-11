using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(OscHandler))]
[RequireComponent(typeof(MidiHandler))]
public class SendController : MonoBehaviour
{
    public bool isActive = false;
    public OscHandler oscHandler;
    [FormerlySerializedAs("midiHanler")] public MidiHandler midiHandler;

    public List<string> sendModulators;
    public int sendModulatorIndex = 0;
}
