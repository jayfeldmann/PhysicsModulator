using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;
using TMPro;
using UnityEngine;

public class MidiDeviceManager : MonoBehaviour
{
    public static MidiDeviceManager instance;
    public static int activeMidiDevice = -1;
    
    [SerializeField] private TMP_Dropdown _midiDeviceDropdown = default;
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

    public string GetActiveMidiDeviceName()
    {
        if (activeMidiDevice>=0)
        {
            return _midiDevices[activeMidiDevice];
        }

        return null;
    }

    public void SetActiveMidiDevice(string name)
    {
        if (_midiDevices.ContainsValue(name))
        {
            var activeIndex = _midiDevices.FirstOrDefault(x => x.Value == name).Key;
            SetActiveMidiDevice(activeIndex);
            _midiDeviceDropdown.value = activeIndex;
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
        int deviceCount = 0;
        for (int deviceIndex = 0; deviceIndex < MidiOut.NumberOfDevices; deviceIndex++)
        {
            var deviceName = MidiOut.DeviceInfo(deviceIndex).ProductName;
            if (deviceName.Contains("Microsoft"))
            {
                //Skips microsoft internal midi bus because it causes major performance problems
                continue;
            }
            _midiDevices.Add(deviceCount,deviceName);
            dropdownDevices.Add(deviceName);
            deviceCount++;
        }
        _midiDeviceDropdown.AddOptions(dropdownDevices);
        SetActiveMidiDevice(0);
        SaveManager.OnLoadGameSettings();
    }

    public void SetActiveMidiDevice(int deviceIndex)
    {
        if (_midiDevices.ContainsKey(deviceIndex))
        {
            activeMidiDevice = deviceIndex;
            Debug.Log($"New midi device: {_midiDevices[deviceIndex]}");
        }
        else
        {
            activeMidiDevice = -1;
            Debug.Log("No MIDI Devices connected.");
        }
    }
}
