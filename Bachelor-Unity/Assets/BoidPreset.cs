using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class BoidPreset : MonoBehaviour
{
    public TMP_Dropdown presetDropdown;
    public TMP_InputField presetNameInputField;
    public TMP_Text errorText;

    private string fileNameRegx = @"^[\w\-. ]+$";
    private string folderKey = "boidpresets";

    private void OnEnable()
    {
        FillPresetDropdown();
    }

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

    public void LoadPreset()
    {
        var loadKey = presetDropdown.captionText.text;
        if (!SaveManager.SaveExists(loadKey,folderKey))
        {
            Debug.Log("Preset file does not exist.");
            return;
        }
        BoidPresetSaveData loadedSave = SaveManager.Load<BoidPresetSaveData>(loadKey, folderKey);
        BoidSettings.UpdateSettings(loadedSave);
        BoidSettings.instance.LoadSliders();
    }

    public void FillPresetDropdown()
    {
        presetDropdown.ClearOptions();
        var presetKeys = SaveManager.GetPresetKeys<BoidPresetSaveData>(folderKey);
        presetDropdown.AddOptions(presetKeys);
    }
}
