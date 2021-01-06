using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoidOptions : MonoBehaviour
{
    public static BoidOptions instance;

    public static bool isActive = false;

    public static SendController sendController;

    [SerializeField] private TMP_InputField _sendChannel;
    [SerializeField] private TMP_InputField _sendCC;
    [SerializeField] private TMP_Dropdown _sendModDropdown;
    [SerializeField] private Toggle _sendMidiToggle;

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

    private void OnEnable()
    {
        if (sendController.gameObject)
        {
            isActive = true;
            ShowSettings();
        }
        BoidSettings.instance.LoadSliders();
    }

    private void OnDisable()
    {
        SaveSettings();
        isActive = false;
        sendController.gameObject.GetComponent<Boid>().DeselectBoid();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    private void ShowSettings()
    {
        var midi = sendController.midiHandler;
        _sendChannel.text = midi.midiChannel.ToString();
        _sendCC.text = midi.midiCC.ToString();
        _sendMidiToggle.isOn = sendController.isActive;
        LoadDropdown();
    }

    private void LoadDropdown()
    {
        _sendModDropdown.ClearOptions();
        _sendModDropdown.AddOptions(sendController.sendModulators);
        _sendModDropdown.value = 0;
    }

    private void SaveSettings()
    {
        int channel;
        int cc;
        if (!Int32.TryParse(_sendChannel.text,out channel))
        {
            return;
        }

        if (!Int32.TryParse(_sendCC.text,out cc))
        {
            return;
        }

        var midi = sendController.midiHandler;
        midi.midiChannel = channel;
        midi.midiCC = cc;
        
        sendController.sendModulatorIndex = _sendModDropdown.value;
        sendController.isActive = _sendMidiToggle.isOn;
    }

    public void OnSendMidiToggle()
    {
        SaveSettings();
    }

}
