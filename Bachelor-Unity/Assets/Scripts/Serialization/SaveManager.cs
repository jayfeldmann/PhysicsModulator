using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager: MonoBehaviour
{
    public static Action OnSaveGameSettings;
    public static Action OnLoadGameSettings;
    public static void Save<T>(T saveObject, string saveKey, string folderKey)
    {
        string path = Application.persistentDataPath + "/" +folderKey + "/";
        Directory.CreateDirectory(path);
        string jsonString = JsonUtility.ToJson(saveObject,true);
        File.WriteAllText(path + saveKey + ".json",jsonString);
        print("Saved File.");
    }

    public static T Load<T>(string saveKey, string folderKey)
    {
        string path = Application.persistentDataPath + "/" +folderKey + "/";
        string jsonFile = File.ReadAllText(path + saveKey + ".json");
        T returnValue = default;
        returnValue = JsonUtility.FromJson<T>(jsonFile);
        print("Loaded File.");
        return returnValue;
    }
    
    public static bool SaveExists(string saveKey, string folderKey)
    {
        var path = Application.persistentDataPath + "/"+folderKey+"/"+saveKey+".json";
        bool file = File.Exists(path);
        return file;
    }


    private void Awake()
    {
        OnSaveGameSettings += SaveGameSettings;
        OnLoadGameSettings += LoadGameSettings;
    }

    private void OnDestroy()
    {
        OnSaveGameSettings -= SaveGameSettings;
        OnLoadGameSettings -= LoadGameSettings;
    }

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
