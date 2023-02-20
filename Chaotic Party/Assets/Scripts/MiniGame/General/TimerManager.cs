using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private MiniGameManager gameManager;
    [SerializeField] private Image timerImage;
    [SerializeField] private Image timerFleche;
    [SerializeField] private float originTime;
    public float currentTime;

    private void Awake()
    {
        gameManager ??= FindObjectOfType<MiniGameManager>();
        Debug.Log(gameManager.gameObject.name);
        gameManager.timerManager = this;
    }

    public void SetTimer(float timerInSecond)
    {
        Debug.Log("settimer");
        Debug.Log(timerInSecond);
        originTime = timerInSecond;
        currentTime = originTime;
        Debug.Log(currentTime);
    }

    private void Update()
    {
        Debug.Log(currentTime);
        if(!gameManager.isMinigamelaunched) return;

        Debug.Log("update");
        Debug.Log(currentTime);
        currentTime = currentTime - Time.deltaTime;
        Debug.Log(currentTime);

        if (currentTime <= 0)
        {
            timerImage.fillAmount = 1;
            timerFleche.transform.localRotation = Quaternion.Euler(0,0,-360);
            gameManager.FinishTimer();
        }
        else
        {
            timerImage.fillAmount = currentTime / originTime; //0 l'image est transparante, 1 elle est pleine.
            timerFleche.transform.localRotation = Quaternion.Euler(0,0,timerImage.fillAmount * 360); //1 fill = 0° et 0.5 fill = -90°
        }
    }
}
