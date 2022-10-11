using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionSO", menuName = "ScriptableObjects/OptionScriptableObject", order = 1)]
public class SOOptions : ScriptableObject
{
    public float masterVolume;
    public float musicVolume;
    public float effectVolume;

    public bool firstLaunch = true;
}
