using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraController : MonoBehaviour
{
    //public List<CinemachineVirtualCamera> cameras;
    //public CinemachineVirtualCamera activeCamera;
    private static CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        //cameras ??= FindObjectsOfType<CinemachineVirtualCamera>().ToList();
        _impulseSource ??= GetComponent<CinemachineImpulseSource>();
    }

    /*public void SwitchActiveCamera()
    {
        int prio = cameras[0].Priority;
        activeCamera = cameras[0];
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            if (camera.Priority <= prio)
            {
                prio = camera.Priority;
                activeCamera = camera;
            }
        }
    }*/

    public static void Shake()
    {
        Debug.Log("shake");
        _impulseSource.GenerateImpulse();
    }

    public static void Shake(Vector3 position, Vector3 velocity)
    {
        _impulseSource.GenerateImpulseAtPositionWithVelocity(position, velocity);
    }

    public static void Shake(float force)
    {
        Debug.Log(force);
        _impulseSource.GenerateImpulseWithForce(force);
    }
}
