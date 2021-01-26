using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasse zur Steuerung des Aktiven Midi-Geräts
/// </summary>
public class MidiDeviceManager : MonoBehaviour
{
    // Instance um auf MidiDeviceManager im gesamten Projekt verweisen zu können
    public static MidiDeviceManager instance;
    // Index des Ausgewählten Midi Device - Wird zum Senden der MIDI Daten benötigt
    public static int activeMidiDevice = -1;
    
    // Gerätedropdown für Settings Menü
    [SerializeField] private TMP_Dropdown _midiDeviceDropdown = default;
    // Dictionary zum Verbinden von Geräte Index und Gerätenamen
    private Dictionary<int, string> _midiDevices;


    // Initialisierung intance
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

    // Gibt den Namen des aktuell ausgewählten Midi-Gerätes als String zurück
    public string GetActiveMidiDeviceName()
    {
        if (activeMidiDevice>=0)
        {
            return _midiDevices[activeMidiDevice];
        }

        return null;
    }
    
    // Neues Midi-Gerät zuweisen
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

    // Erneuert die Liste der an den Pc angeschlossenen MIDI Geräte
    // Löscht auch Dropdown und füllt es mit aktualisierter Geräteliste auf
    public void RefreshMidiDevices()
    {
        _midiDeviceDropdown.ClearOptions();
        _midiDevices = new Dictionary<int, string>();
        List<string> dropdownDevices = new List<string>();
        int deviceCount = 0;
        for (int deviceIndex = 0; deviceIndex < MidiOut.NumberOfDevices; deviceIndex++)
        {
            var deviceName = MidiOut.DeviceInfo(deviceIndex).ProductName;
            _midiDevices.Add(deviceIndex,deviceName);
            dropdownDevices.Add(deviceName);
        }
        _midiDeviceDropdown.AddOptions(dropdownDevices);
        SetActiveMidiDevice(0);
        SaveManager.OnLoadGameSettings();
    }

    // Legt aktives Midigerät fest
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
