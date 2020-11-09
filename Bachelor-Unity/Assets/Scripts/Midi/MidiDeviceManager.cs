using System;
using System.Collections;
using System.Collections.Generic;
using NAudio.Midi;
using TMPro;
using UnityEngine;

public class MidiDeviceManager : MonoBehaviour
{
    public static MidiDeviceManager instance;

    [SerializeField] private TMP_Dropdown _midiDeviceDropdown;
    private Dictionary<int, string> _midiDevices;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        RefreshMidiDevices();
    }

    public void RefreshMidiDevices()
    {
        _midiDeviceDropdown.ClearOptions();
        _midiDevices = new Dictionary<int, string>();
        List<string> dropdownDevices = new List<string>();
        for (int deviceIndex = 0; deviceIndex < MidiOut.NumberOfDevices; deviceIndex++)
        {
            var deviceName = MidiOut.DeviceInfo(deviceIndex).ProductName;
            _midiDevices.Add(deviceIndex,deviceName);
            dropdownDevices.Add(deviceName);
        }
        _midiDeviceDropdown.AddOptions(dropdownDevices);
    }
}
