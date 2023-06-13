using System;
using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

public class TargetGroupForCinemachineCamera : MonoBehaviour
{
    public CinemachineTargetGroup.Target[] _targets = Array.Empty<CinemachineTargetGroup.Target>();

    [Serializable]
    public struct CameraClamp
    {
        public bool isClamp;
        public float clampValue;
    }

    public CameraClamp XMin;
    public CameraClamp XMax;
    public CameraClamp YMin;
    public CameraClamp YMax;
    
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 averagePos = CalculateAveragePosition();
        
        float xMin = XMin.isClamp ? XMin.clampValue : averagePos.x;
        float xMax = XMax.isClamp ? XMax.clampValue : averagePos.x;
        float yMin = YMin.isClamp ? YMin.clampValue : averagePos.y;
        float yMax = YMax.isClamp ? YMax.clampValue : averagePos.y;
    }
    
    Vector3 CalculateAveragePosition()
    {
        Vector3 pos = Vector3.zero;
        float weight = 0;
        for (int i = 0; i < _targets.Length; ++i)
        {
            if (_targets[i].target != null)
            {
                weight += _targets[i].weight;
                pos += _targets[i].target.position
                       * _targets[i].weight;
            }
        }
        if (weight > UnityVectorExtensions.Epsilon)
            pos /= weight;
        else
            pos = transform.position;
        return pos;
    }
}
