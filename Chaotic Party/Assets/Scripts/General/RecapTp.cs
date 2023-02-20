using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapTp : MonoBehaviour
{
    [SerializeField] private float xPositionTp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = new Vector3(xPositionTp, other.transform.position.y, other.transform.position.z);
    }
}
