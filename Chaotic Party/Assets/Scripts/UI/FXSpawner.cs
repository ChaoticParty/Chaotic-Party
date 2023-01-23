using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FXSpawner : MonoBehaviour
{
    public static FXObject Spawn(FXBuilder fxSo, Vector3 position)
    {
        GameObject fxObject = new() { transform = { position = position } };
        FXObject fxScript = fxObject.AddComponent<FXObject>();
        fxScript.SetFXObject(fxSo);
        return fxScript;
    }
}
