using System;
using System.Collections;
using System.Collections.Generic;
using HinputClasses;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Gamepad gamepad;
    public UnityEvent aJustPressed = new ();

    private void Update()
    {
        if(gamepad.A.justPressed)
        {
            aJustPressed.Invoke();
            Debug.Log(gameObject.name + " pressed A");
        }
    }

    private void OnDisable()
    {
        aJustPressed.RemoveAllListeners();
    }
}
