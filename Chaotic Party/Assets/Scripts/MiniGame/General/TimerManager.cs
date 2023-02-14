using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private MiniGameManager gameManager;
    [SerializeField] private Image timerImage;
    private float originTime;
    private float currentTime;

    private void Awake()
    {
        gameManager ??= FindObjectOfType<MiniGameManager>();
    }

    public void SetTimer(float timerInSecond)
    {
        originTime = timerInSecond;
        currentTime = originTime;
    }

    private void Update()
    {
        if(!gameManager.isMinigamelaunched) return;
        
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            timerImage.fillAmount = 1;
            gameManager.FinishTimer();
        }
        else
        {
            timerImage.fillAmount = MathF.Abs(currentTime / originTime - 1); //0 l'image est transparante, 1 elle est pleine.
        }
    }
}
