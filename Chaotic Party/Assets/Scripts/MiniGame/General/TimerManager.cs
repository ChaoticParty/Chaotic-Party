using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public MiniGameManager gameManager;
    public float[] timers;
    [SerializeField] private Image timerImage;
    [SerializeField] private Image.FillMethod fillMethod;
    private int timerIndex;
    private float currentTimer;

    private void Awake()
    {
        gameManager ??= FindObjectOfType<MiniGameManager>();
        ChangeTimer(0);
    }

    private void ChangeTimer(int followingTimer = 1)
    {
        timerIndex += followingTimer;
        currentTimer = timers[timerIndex];
    }

    private void Update()
    {
        if(gameManager.isGameDone) return;
        
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            gameManager.FinishTimer(timerIndex + 1 == timers.Length);
        }
        else
        {
            timerImage.fillAmount = currentTimer / timers[timerIndex];
        }
    }
}
