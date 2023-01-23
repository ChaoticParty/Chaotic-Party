using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public SOOptions optionsSO;
    private ReferenceHolder _referenceHolder;
    [SerializeField] private EventSystem _eventSystem;
    [Space]
    public AudioMixer audioManager;
    [Space]
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectVolume;
    [Space]
    public Toggle masterMuteButton;
    public Toggle musicMuteButton;
    public Toggle effectMuteButton;
    public Toggle fullScreenMode;
    [Space]
    public TextMeshProUGUI masterTMP;
    public TextMeshProUGUI musicTMP;
    public TextMeshProUGUI effectTMP;
    [Space]
    public Button backBTN;

    #region MonoBehaviour Méthodes

    private void OnEnable()
    {
        _referenceHolder ??= GameObject.Find("ReferenceHolder").GetComponent<ReferenceHolder>();
        EventSystem.current = _eventSystem;
        
        masterVolume.value = optionsSO.optionsData.masterVolume;
        musicVolume.value = optionsSO.optionsData.musicVolume;
        effectVolume.value = optionsSO.optionsData.effectVolume;
        
        MasterChange(masterVolume.value);
        MusicChange(musicVolume.value);
        EffectChange(effectVolume.value);
        
        fullScreenMode.isOn = optionsSO.optionsData.fullscreen;
        
        masterVolume.onValueChanged.AddListener(MasterChange);
        musicVolume.onValueChanged.AddListener(MusicChange);
        effectVolume.onValueChanged.AddListener(EffectChange);
        
        masterMuteButton.onValueChanged.AddListener(MasterMute);
        musicMuteButton.onValueChanged.AddListener(MusicMute);
        effectMuteButton.onValueChanged.AddListener(EffectMute);
        
        fullScreenMode.onValueChanged.AddListener(FullScreenChange);
        
        backBTN.onClick.AddListener(Back);
    }

    private void OnDisable()
    {
        masterVolume.onValueChanged.RemoveAllListeners();
        musicVolume.onValueChanged.RemoveAllListeners();
        effectVolume.onValueChanged.RemoveAllListeners();
        
        masterMuteButton.onValueChanged.RemoveAllListeners();
        musicMuteButton.onValueChanged.RemoveAllListeners();
        effectMuteButton.onValueChanged.RemoveAllListeners();
        
        fullScreenMode.onValueChanged.RemoveAllListeners();
        
        backBTN.onClick.RemoveAllListeners();
    }

    #endregion

    #region Class Méthodes

    #region Gestion Sonore

    private void MasterChange(float sliderValue)
    {
        audioManager.SetFloat("masterVolume", Mathf.Log10(sliderValue/100) * 20);
        masterTMP.text = Mathf.RoundToInt(sliderValue).ToString();
        optionsSO.optionsData.masterVolume = Mathf.RoundToInt(sliderValue);
    }
    
    private void MasterMute(bool toggleValue)
    {
        if (!toggleValue)
        {
            audioManager.SetFloat("masterVolume", Mathf.Log10(optionsSO.optionsData.masterVolume/100) * 20);
            masterVolume.interactable = true;
        }
        else
        {
            audioManager.SetFloat("masterVolume", -80);
            masterVolume.interactable = false;
        }
    }

    private void MusicChange(float sliderValue)
    {
        audioManager.SetFloat("musicVolume", Mathf.Log10(sliderValue/100) * 20);
        musicTMP.text = Mathf.RoundToInt(sliderValue).ToString();
        optionsSO.optionsData.musicVolume = Mathf.RoundToInt(sliderValue);
    }
    
    private void MusicMute(bool toggleValue)
    {
        if (!toggleValue)
        {
            audioManager.SetFloat("musicVolume", Mathf.Log10(optionsSO.optionsData.musicVolume/100) * 20);
            musicVolume.interactable = true;
        }
        else
        {
            audioManager.SetFloat("musicVolume", -80);
            musicVolume.interactable = false;
        }
    }

    private void EffectChange(float sliderValue)
    {
        audioManager.SetFloat("effectVolume", Mathf.Log10(sliderValue/100) * 20);
        effectTMP.text = Mathf.RoundToInt(sliderValue).ToString();
        optionsSO.optionsData.effectVolume = Mathf.RoundToInt(sliderValue);
    }
    
    private void EffectMute(bool toggleValue)
    {
        if (!toggleValue)
        {
            audioManager.SetFloat("effectVolume", Mathf.Log10(optionsSO.optionsData.effectVolume/100) * 20);
            effectVolume.interactable = true;
        }
        else
        {
            audioManager.SetFloat("effectVolume", -80);
            effectVolume.interactable = false;
        }
    }

    #endregion
    
    private void FullScreenChange(bool toggleValue)
    {
        Screen.fullScreen = toggleValue;
        optionsSO.optionsData.fullscreen = toggleValue;
    }

    private void Back()
    {
        _referenceHolder.oldEventSystem.SetActive(true);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    #endregion

}
