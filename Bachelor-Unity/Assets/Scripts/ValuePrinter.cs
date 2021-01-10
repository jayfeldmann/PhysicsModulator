using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuePrinter : MonoBehaviour
{
    public MidiHandler midiHandler;

    public string saveKey;
    public int readDuration = 10;

    private bool printValues = false;
    private int valueCount = 0;

    private int durationValueCount
    {
        get
        {
            return (int)(1 / 0.02) * readDuration;
        }
    }

    private List<int> midiValueList;
    private void Start()
    {
        midiValueList = new List<int>();
    }

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

    public void StartValueRead()
    {
        Debug.Log("Start value reading...");
        midiValueList.Clear();
        valueCount = 0;
        printValues = true;
    }
    
    public void SaveMidiValues()
    {
        var valueData = new MidiValueData(midiValueList.ToArray());
        SaveManager.Save(valueData,saveKey,"valuereadings");
    }

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


}

public class MidiValueData
{
    public int[] values;

    public MidiValueData(int[] _values)
    {
        values = _values;
    }
}
