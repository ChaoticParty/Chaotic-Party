using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpamController : MonoBehaviour
{
    protected PlayerController player;
    public SpamManager spamManager;
    public float spamValue;

    protected void Awake()
    {
        player = GetComponent<PlayerController>();
        spamManager = player.miniGameManager as SpamManager;
    }

    protected abstract void Click();
}
