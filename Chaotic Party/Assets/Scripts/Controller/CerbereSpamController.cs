using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CerbereSpamController : SpamController
{
    private StunController _stunController;
    private bool hasClicked = false;
    [HideInInspector] public bool isShout = false;
    [HideInInspector] public Etat etat = Etat.NULL;
    private CerbereManager cerbereManager;
    [Header("Temps placeholder, a changer une fois les anims dispo")]
    [SerializeField] private float standUpAnimTime = 1f;
    [SerializeField] private float wakeUpAnimTime = 1f;

    protected new void Awake()
    {
        base.Awake();
        _stunController ??= GetComponent<StunController>();
        cerbereManager = spamManager as CerbereManager;
        player.aJustPressed.AddListener(() => AlternateClick(Etat.A));
        player.bJustPressed.AddListener(() => AlternateClick(Etat.B));
        player.yJustPressed.AddListener(WakuUp);
    }

    protected override void Click()
    {
        spamManager.Click(player.index, spamManager.spamValue);
    }

    private void AlternateClick(Etat value)
    {
        if (hasClicked || !player.CanMove()) return;

        if (etat.Equals(Etat.FALL))
        {
            hasClicked = true;
            StartCoroutine(StandUpPlayer(standUpAnimTime));
            return;
        } 
        
        StartCoroutine(Cooldown());
        
        if (etat != value)
        {
            Click();
            etat = value;
        }
        else
        {
            FailAlternate();
            etat = Etat.FALL;
        }
    }

    private void FailAlternate()
    {
        //Anim de chute
        _stunController.Stun();
        //Reactivation a la fin de l'anim de chute, voir comment on gere avec le stun controller
    }

    private void WakuUp()
    {
        if (isShout || !player.CanMove()) return;
        isShout = true;
        cerbereManager.playerYell.Invoke(transform.position, "Argument");
        player.ChangeColor(Color.magenta);
        cerbereManager.PlayerWakeUp();
        StartCoroutine(WakeUpFeedBack(wakeUpAnimTime));
    }

    
    #region Feedback Methods

    private IEnumerator StandUpPlayer(float animationTime)
    {
        player.ChangeColor(Color.yellow);
        yield return new WaitForSeconds(animationTime);
        player.ChangeColor();
        hasClicked = false;
        etat = Etat.NULL;
    } 
    
    private IEnumerator WakeUpFeedBack(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        isShout = false;
        player.ChangeColor();
    }

    #endregion
    
    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
    }

    public enum Etat
    {
        A,B,FALL,NULL
    }
}
