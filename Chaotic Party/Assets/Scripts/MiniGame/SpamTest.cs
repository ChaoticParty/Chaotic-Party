using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamTest : MonoBehaviour
{
    private void Awake()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.aJustPressed.AddListener(Spam);
    }

    private void Spam()
    {
        Debug.Log("nique");
    }
}
