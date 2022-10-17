using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public SOOptions optionsSO;

    public AudioMixer audioManager;
    
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectVolume;

    public TextMeshProUGUI masterTMP;
    public TextMeshProUGUI musicTMP;
    public TextMeshProUGUI effectTMP;

    private void Awake()
    {
        masterVolume.value = optionsSO.optionsData.masterVolume;
        musicVolume.value = optionsSO.optionsData.musicVolume;
        effectVolume.value = optionsSO.optionsData.effectVolume;
        
        masterVolume.onValueChanged.AddListener(MasterChange);
        masterVolume.onValueChanged.AddListener(MusicChange);
        masterVolume.onValueChanged.AddListener(EffectChange);
    }

    private void MasterChange(float sliderValue)
    {
        audioManager.SetFloat("masterVolume", Mathf.Log10(sliderValue) * 20);
        masterTMP.text = Mathf.RoundToInt(sliderValue).ToString();
    }

    private void MusicChange(float sliderValue)
    {
        audioManager.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
        musicTMP.text = Mathf.RoundToInt(sliderValue).ToString();
    }

    private void EffectChange(float sliderValue)
    {
        audioManager.SetFloat("effectVolume", Mathf.Log10(sliderValue) * 20);
        effectTMP.text = Mathf.RoundToInt(sliderValue).ToString();
    }
}
