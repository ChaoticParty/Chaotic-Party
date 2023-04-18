using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotoMinionOverlord : MonoBehaviour
{
    public Transform head;
    public List<Transform> positions = new();
    private Transform _currentPosition;
    public Animator beforePictureAnimator;
    public CinemachineVirtualCamera vCam;
    public float baseOrthoSize;
    private static readonly int Launch = Animator.StringToHash("Launch");
    private static readonly int Stop = Animator.StringToHash("Stop");

    private void Awake()
    {
        _currentPosition = transform.parent;
        head ??= transform.Find("Head");
        baseOrthoSize = vCam.m_Lens.OrthographicSize / transform.localScale.x;
    }

    public void ChangePosition(bool isPositionDifferent = true)
    {
        if(positions.Count <= 1) return;
        
        Transform nextPosition = positions[Random.Range(0, positions.Count)];
        if(isPositionDifferent)
        {
            while (nextPosition == _currentPosition)
            {
                nextPosition = positions[Random.Range(0, positions.Count)];
            }
        }

        _currentPosition = nextPosition;
        // Set valeurs du transform
        Transform transform1;
        (transform1 = transform).SetParent(_currentPosition);
        transform1.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform1.localScale = Vector3.one;
    }

    public float GetPlayerDistance(Transform playerTransform)
    {
        return Vector2.Distance(head.position, playerTransform.position);
    }

    public void StartAnimationBeforePicture()
    {
        beforePictureAnimator.SetTrigger(Launch);
    }

    public void StopAnimationBeforePicture()
    {
        beforePictureAnimator.SetTrigger(Stop);
    }

    public void Focus()
    {
        vCam.Priority = 10000;
    }

    public void Unfocus()
    {
        vCam.Priority = 0;
    }

    public void RemoveCameraFromOverlord()
    {
        Vector3 camPos = vCam.transform.position;
        Quaternion camRot = vCam.transform.rotation;
        vCam.transform.SetParent(transform.parent, false);
        vCam.transform.position = camPos;
        vCam.transform.rotation = camRot;
    }

    public void PlaceCameraOnOverlord()
    {
        vCam.transform.SetParent(head, true);
        vCam.transform.localPosition = Vector3.zero + new Vector3(0, 0, -1);
        vCam.transform.localRotation = Quaternion.identity;
        vCam.m_Lens.OrthographicSize = baseOrthoSize * transform.parent.parent.localScale.x;
    }
}
