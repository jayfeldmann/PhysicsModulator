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

    public static Boid activeBoid;

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
        if (activeBoid)
        {
            isActive = true;
            activeBoid.SelectBoid();
            ShowSettings();
        }
    }

    private void OnDisable()
    {
        SaveSettings();
        activeBoid.SelectBoid();
        isActive = false;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    private void ShowSettings()
    {
        _sendChannel.text = activeBoid.sendChannel.ToString();
        _sendCC.text = activeBoid.sendCC.ToString();
        _sendMidiToggle.isOn = activeBoid.isActive;
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

        activeBoid.sendChannel = channel;
        activeBoid.sendCC = cc;
        activeBoid.SetSendMod(_sendModDropdown.value);
        activeBoid.isActive = _sendMidiToggle.isOn;
    }
    
}
