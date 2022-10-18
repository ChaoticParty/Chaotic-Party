using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionSO", menuName = "ScriptableObjects/OptionScriptableObject", order = 1)]
public class SOOptions : ScriptableObject
{
    public StructOptions optionsData;

    public bool firstLaunch = true;
}
