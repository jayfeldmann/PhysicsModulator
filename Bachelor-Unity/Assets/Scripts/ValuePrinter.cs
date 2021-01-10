using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ValuePrinter ist eine Hilfsklasse, die dazu dient, über einen Zeitraum die aktuellen Midi Values eines Objektes auszulesen und diese danach im JSON fromat als int array zu speichern.
/// Diese Daten werden dann mit dem beigefügten Python Script und Matplotlib als Graph ausgegeben.
/// Diese Klasse wird vom Benutzer nicht verwendet werden können und ist nur im Unity Editor verfügbar.
/// </summary>
public class ValuePrinter : MonoBehaviour
{
    // Liefert die auszulesenden Midi Daten.
    public MidiHandler midiHandler;
    //Savekey ist der Dateiname der json Datei.
    public string saveKey;
    //Die Dauer über die gelesen werden soll 
    public int readDuration = 10;

    //Hilfsvariabeln zum Auslesen der Daten und bestimmung der vorangeschrittenen Zeit
    private bool printValues = false;
    private int valueCount = 0;

    private int durationValueCount => (int)durationValueCount * readDuration;
    private int readFrequenzy => (int) (1 / Time.fixedDeltaTime); // 50Hz durch projectsettings

    //die zu speichernde Datenliste
    private List<int> midiValueList;
    private void Start()
    {
        midiValueList = new List<int>();
    }

    //Fixed update loop um Midi Daten in gleichmäßigem Abstand auszulesen. Abtastfrequenz = 50Hz
    private void FixedUpdate()
    {
        if (printValues)
        {
            midiValueList.Add(GetCurrentMidiValue());
            valueCount++;
            if (valueCount > durationValueCount)
            {
                SaveMidiValues();
                ResetReading();
            }
        }
    }

    //Initialisiert ein Leseverfahren. Setzt alle hilfsvariabeln zurück.
    public void StartValueRead()
    {
        Debug.Log("Start value reading...");
        midiValueList.Clear();
        valueCount = 0;
        printValues = true;
    }
    
    //Speichert die gesammelten Daten als json Datei im Ordner "valuereadings"
    public void SaveMidiValues()
    {
        var valueData = new MidiValueData(midiValueList.ToArray(), readDuration, readFrequenzy);
        SaveManager.Save(valueData,saveKey,"valuereadings");
    }

    //Setzt Hilfsvariabeln am Ende der Auslesezeit wieder zurück.
    public void ResetReading()
    {
        Debug.Log($"Stop/Reset value reading with {midiValueList.Count} values.");
        printValues = false;
        midiValueList.Clear();
    }
    
    private int GetCurrentMidiValue()
    {
        return midiHandler.midiValue;
    }

    /// <summary>
    /// Diese Klasse dient dazu, die gesammelten Midi Daten serialisieren zu können und als JSON abzuspeichern.
    /// Um auch später noch Graphen daraus generieren zu können werden neben den Midi Daten auch die Abtastdauer und Frequenz
    /// mit abgespeichert.
    /// </summary>
    public class MidiValueData
    {    
        public int readDuration;
        public int readFrequenzy;
        public int[] values;
    
        public MidiValueData(int[] _values, int _duration, int _freq)
        {
            values = _values;
            readDuration = _duration;
            readFrequenzy = _freq;
        }
    }
}



