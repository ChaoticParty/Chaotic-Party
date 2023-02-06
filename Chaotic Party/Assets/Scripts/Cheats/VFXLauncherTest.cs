using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VFXLauncherTest : MonoBehaviour
{
    public KeyCode launchKey;
    public UnityEvent launchVFXEvent;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(launchKey))
        {
            launchVFXEvent.Invoke();
        }
    }
}
