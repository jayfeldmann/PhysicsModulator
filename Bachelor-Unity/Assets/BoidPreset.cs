using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/// <summary>
/// Definiert wie BoidSetting Presets gespeichert und Geladen werden.
/// </summary>
public class BoidPreset : MonoBehaviour
{
    //UI Referenzen
    public TMP_Dropdown presetDropdown;
    public TMP_InputField presetNameInputField;
    public TMP_Text errorText;

    //Regular Expression von hier: https://stackoverflow.com/questions/11794144/regular-expression-for-valid-filename
    //Definiert RegEx für validen Dateinamen um auf Betriebssystemen verbotene Zeichen auszuschließen.
    private string fileNameRegx = @"^[\w\-. ]+$";
    //Ordnername, in dem die Presets gespeichert werden.
    private string folderKey = "boidpresets";

    private void OnEnable()
    {
        FillPresetDropdown();
    }

    //Speichert aktuelle BoidSettings als JSON Datei im folderKey Ordner.
    public void SavePreset()
    {
        var saveKey = presetNameInputField.text;
        var match = Regex.Match(saveKey, fileNameRegx, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            errorText.text = "Invalid Presetname!";
            return;
        }
        errorText.text = "";
        
        BoidPresetSaveData newBoidPreset = new BoidPresetSaveData();
        SaveManager.Save(newBoidPreset,saveKey,folderKey);
        presetNameInputField.text = "";
        FillPresetDropdown();
    }

    //Lädt ein bestimmtes Preset anhand einer Auswahl im UI Dropdown
    public void LoadPreset()
    {
        var loadKey = presetDropdown.captionText.text;
        if (!SaveManager.SaveExists(loadKey,folderKey))
        {
            Debug.Log("Preset file does not exist.");
            return;
        }
        BoidPresetSaveData loadedSave = SaveManager.Load<BoidPresetSaveData>(loadKey, folderKey);
        BoidSettings.instance.UpdateSettings(loadedSave);
        BoidSettings.instance.LoadSliders();
    }

    //Lädt aktuell gespeicherte Presets in ein Dropdownmenü
    public void FillPresetDropdown()
    {
        presetDropdown.ClearOptions();
        var presetKeys = SaveManager.GetPresetKeys<BoidPresetSaveData>(folderKey);
        presetDropdown.AddOptions(presetKeys);
    }
}
