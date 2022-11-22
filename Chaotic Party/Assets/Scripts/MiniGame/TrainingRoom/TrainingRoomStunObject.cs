using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrainingRoomStunObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.name);
        if (other.collider.TryGetComponent(out StunController stunScript))
        {
            Debug.Log("player has stun script");
            stunScript.Stun();
        }
    }
}
