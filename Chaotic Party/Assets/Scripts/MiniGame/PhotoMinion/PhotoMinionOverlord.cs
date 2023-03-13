using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotoMinionOverlord : MonoBehaviour
{
    public Transform head;
    public List<Transform> positions = new();
    private Transform _currentPosition;

    private void Awake()
    {
        _currentPosition = transform.parent;
        head ??= transform.Find("Head");
    }

    public void ChangePosition()
    {
        if(positions.Count <= 1) return;
        
        Transform nextPosition = _currentPosition;
        while (nextPosition == _currentPosition)
        {
            nextPosition = positions[Random.Range(0, positions.Count)];
        }

        _currentPosition = nextPosition;
        // Set valeurs du transform
        Transform transform1;
        (transform1 = transform).SetParent(_currentPosition);
        transform1.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public float GetPlayerDistance(Transform playerTransform)
    {
        return Vector2.Distance(head.position, playerTransform.position);
    }
}
