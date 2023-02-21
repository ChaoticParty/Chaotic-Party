using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class ResetOnMainMenu : ScriptableObject
{
    private void Awake()
    {
        Debug.Log("awake");
        SceneManager.sceneLoaded += CheckMainMenu;
    }

    private void CheckMainMenu(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("checkmainmenu");
        if (scene.name == "MenuPrincipal")
        {
            Debug.Log("menuprincipal");
            OnSceneLoaded(scene, loadSceneMode);
        }
    }

    protected abstract void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode);
}
