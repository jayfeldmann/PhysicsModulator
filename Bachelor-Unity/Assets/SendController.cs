using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OscHandler))]
[RequireComponent(typeof(MidiHandler))]
public class SendController : MonoBehaviour
{
    public bool isActive = false;
    public OscHandler oscHandler;
    public MidiHandler midiHanler;

    private void Awake()
    {
        oscHandler = GetComponent<OscHandler>();
        midiHanler = GetComponent<MidiHandler>();
    }
}
