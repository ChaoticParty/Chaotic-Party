using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    protected PlayerController player;
    
    protected void Awake()
    {
        player ??= GetComponent<PlayerController>();
    }
}
