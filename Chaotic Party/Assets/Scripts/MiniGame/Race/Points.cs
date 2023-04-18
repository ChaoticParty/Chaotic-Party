using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class Points
{
    [ReadOnly]
    public int points;
    public Vector2 scale = Vector2.one;
    public Color color = Color.white;
    [PropertyRange(-180, 180)]
    public float rotation;
    [PreviewField]
    public GameObject effect;

    public Quaternion GetRotation()
    {
        Debug.Log(rotation);
        Debug.Log(new Vector3(0, 0, rotation));
        return Quaternion.Euler(new Vector3(0, 0, rotation));
    }
}
