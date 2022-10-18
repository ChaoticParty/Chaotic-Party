using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class Initializer : MonoBehaviour
{
    public static StructOptions optionsStruct;
    private string path;
    public SOOptions optionsSO;
    public AudioMixer audioManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        
        if (optionsSO.firstLaunch)
        {
            if (File.Exists(path + "optionsData"))
            {
                using StreamReader reader = new StreamReader(path + "optionsData");
                string json = reader.ReadToEnd();

                optionsStruct = JsonUtility.FromJson<StructOptions>(json);
                optionsSO.optionsData = optionsStruct;
            }
            else
            {
                StructOptions options = new StructOptions {masterVolume = 100, musicVolume = 100, effectVolume = 100};
                string json = JsonUtility.ToJson(options);
                using StreamWriter writer = new StreamWriter(path + "optionsData");
                writer.Write(json);
                
                optionsSO.optionsData = options;
            }

            audioManager.SetFloat("masterVolume", Mathf.Log10(optionsSO.optionsData.masterVolume/100) * 20); //Valeurs entre 0.001 et 100 dans le json
            audioManager.SetFloat("musicVolume", Mathf.Log10(optionsSO.optionsData.musicVolume/100) * 20); //Mettre direct les valeurs pour le slider
            audioManager.SetFloat("effectVolume", Mathf.Log10(optionsSO.optionsData.effectVolume/100) * 20);
        }
    }

    private void OnApplicationQuit()
    {
        optionsStruct = optionsSO.optionsData;

        string json = JsonUtility.ToJson(optionsStruct);
        
        using StreamWriter writer = new StreamWriter(path + "optionsData");
        writer.Write(json);
    }
}