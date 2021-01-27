using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Diese Klasse verwaltet die Darstellung und Verarbeitung des Einstellungsmenüs und die Daten für die Simulation der Boids.
/// Auch wird durch das UI ermöglicht, diese zu ändern.
/// Zusätzlich zu den Simulationseinstellungen können auch Midi/OSC Einstellungen vorgenommen werden. 
/// </summary>
public class BoidOptions : MonoBehaviour
{
    //Hilfsvariablen
    public static BoidOptions instance;

    public static bool isActive = false;

    //Referenz zum aktuell ausgewählten Boid.
    public static SendController sendController;
    private SendMode prevMode;

    //Referenzen zum UI
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

    //Sendet einzelne Midi Nachricht um Midi-Learn funktionen von zB Ableton Live verwenden zu können.
    public void SendMidiLearnMessage()
    {
        var midi = sendController.midiHandler;
        if (ValidateMidiValues(ref midi.midiChannel, ref midi.midiCC))
        {
            midi.midiValue = 100;
            midi.SendMidi();
        }
    }

    //Laden  UI
    private void OnEnable()
    {
        if (sendController.gameObject)
        {
            isActive = true;
            prevMode = Settings.SendMode;
            Settings.SendMode = SendMode.DISABLED;
            ShowSettings();
        }
        BoidSettings.instance.LoadSliders();
    }
    //Entladen UI
    private void OnDisable()
    {
        Settings.SendMode = prevMode;
        SaveSettings();
        isActive = false;
        sendController.gameObject.GetComponent<Boid>().DeselectBoid();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    //Laden UI
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
        _sendModDropdown.value = sendController.sendModulatorIndex;
    }

    //überprüft, ob die eingegebenen Werte valide sind und für Midi Nachrichten verwendet werden können.
    private bool ValidateMidiValues(ref int channelVal, ref int ccVal)
    {
        int channel;
        int cc;
        if (!Int32.TryParse(_sendChannel.text,out channel))
        {
            return false;
        }

        if (!Int32.TryParse(_sendCC.text,out cc))
        {
            return false;
        }

        channelVal = channel;
        ccVal = cc;
        return true;
    }

    //überprüft Einstellungen und speichert diese
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

    //Event zum speichern
    public void OnSendMidiToggle()
    {
        SaveSettings();
    }

}
