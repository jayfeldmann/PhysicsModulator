using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasse zum Speichern und Laden von JSON basierten Datenstrukturen
/// Basierend auf Generischen Methoden, damit die meisten Arten von C# Objekten als JSON gespeichert werden können.
/// </summary>
public class SaveManager: MonoBehaviour
{
    // Events um GameSettings zu speichern
    public static Action OnSaveGameSettings;
    public static Action OnLoadGameSettings;
    
    // Generische Methode, Korrekt Formatiertes C# Objekt wird als JSON datei gespeichert
    // Savekey ist der Dateiname und FolderKey 
    public static void Save<T>(T saveObject, string saveKey, string folderKey)
    {
        string path = Application.persistentDataPath + "/" +folderKey + "/";
        Directory.CreateDirectory(path);
        string jsonString = JsonUtility.ToJson(saveObject,true);
        File.WriteAllText(path + saveKey + ".json",jsonString);
        print("Saved File.");
    }

    // Generische Methode, JSON dateien zu lesen und als C# Objekte zu laden.
    public static T Load<T>(string saveKey, string folderKey)
    {
        string path = Application.persistentDataPath + "/" +folderKey + "/";
        string jsonFile = File.ReadAllText(path + saveKey + ".json");
        T returnValue = default;
        returnValue = JsonUtility.FromJson<T>(jsonFile);
        print("Loaded File.");
        return returnValue;
    }
    
    // Generische Methode, JSON dateien zu lesen und als C# Objekte zu laden.
    // lädt dateien anhand direkter Dateipfade, anstelle von Keys
    public static T Load<T>(string filePath)
    {
        string jsonFile = File.ReadAllText(filePath);
        T returnValue = default;
        returnValue = JsonUtility.FromJson<T>(jsonFile);
        print("Loaded File.");
        return returnValue;
    }
    
    // Methode zum Testen, ob Keys existieren bevor sie geladen werden sollen
    public static bool SaveExists(string saveKey, string folderKey)
    {
        var path = Application.persistentDataPath + "/"+folderKey+"/"+saveKey+".json";
        return File.Exists(path);
    }

    // Methode zum Testen, ob Dateipfade existieren bevor sie geladen werden sollen
    public static bool SaveExists(string filePath)
    {
        return File.Exists(filePath);
    }
    
    // Liest vorhandene Presetdateien aus Ordner und gibt sie als Liste entsprechender C# Objekte wieder aus.
    public static List<string> GetPresetKeys<T>(string folderKey)
    {
        List<string> presetKeys = new List<string>();
        var path = Application.persistentDataPath + $"/{folderKey}";
        Directory.CreateDirectory(path);
        var filePaths = Directory.GetFiles(path,"*.json");
        foreach (var filePath in filePaths)
        {
            if (IsValidSave<T>(filePath))
            {
                presetKeys.Add(Path.GetFileNameWithoutExtension(filePath));
            }
        }
        return presetKeys;
    }

    // Kontrolliert die Validität der JSON Dateien
    public static bool IsValidSave<T>(string presetPath)
    {
        try
        {
            T testPreset = Load<T>(presetPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }


    // Zuweisen der Events
    private void Awake()
    {
        OnSaveGameSettings += SaveGameSettings;
        OnLoadGameSettings += LoadGameSettings;
    }

    // Dereferenzieren der Events, um Fehler zu vermeiden.
    private void OnDestroy()
    {
        OnSaveGameSettings -= SaveGameSettings;
        OnLoadGameSettings -= LoadGameSettings;
    }

    
    // Speichert GameSettings
    public void SaveGameSettings()
    {
        GameSettingsSaveData settingsSaveData = new GameSettingsSaveData()
        {
            midiDevice = MidiDeviceManager.instance.GetActiveMidiDeviceName(),
            oscInPort = Settings.oscInPort,
            oscOutIp = Settings.oscOutIp,
            oscOutPort = Settings.oscOutPort
        };
        
        Save(settingsSaveData,"gamesettings","settings");
    }

    // Lädt GameSettings
    public void LoadGameSettings()
    {
        if (!SaveExists("gamesettings","settings"))
        {
            return;
        }
        
        GameSettingsSaveData settingsSaveData = Load<GameSettingsSaveData>("gamesettings", "settings");
        MidiDeviceManager.instance.SetActiveMidiDevice(settingsSaveData.midiDevice);
        Settings.oscInPort = settingsSaveData.oscInPort;
        Settings.oscOutIp = settingsSaveData.oscOutIp;
        Settings.oscOutPort = settingsSaveData.oscOutPort;
        Settings.instance.SetOscValues();
    }
}
