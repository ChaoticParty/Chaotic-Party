using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShakeEffect", menuName = "ScriptableObjects/FX/ShakeEffect")]
public class ShakeEffect : ScriptableObject
{
    public Vector3 position;
    public Vector3 velocity;
    public float force;

    public void Spawn()
    {
        if (velocity != default)
        {
            CameraController.Shake(position, velocity);
        }
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        else if (force != default)
        {
            CameraController.Shake(force);
        }
        else
        {
            CameraController.Shake();
        }
    }
}
