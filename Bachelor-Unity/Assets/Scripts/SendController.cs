using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Der Sendcontroller hält nur die Referenzen zum OSC und zum MidiHandler. Das dient dazu, dass jedes Physikobjekt nur
/// eine Referenz benötigt, den SendController, und nicht jedes mal OSC und MidiHandler einzeln.
/// Das vereinfacht den Code.
/// </summary>
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
