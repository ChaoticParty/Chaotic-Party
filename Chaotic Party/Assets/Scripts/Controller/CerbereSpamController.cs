using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CerbereSpamController : SpamController
{
    private StunController _stunController;
    private bool hasClicked = false;
    [HideInInspector] public bool isShout = false;
    public Etat _etat = Etat.NULL;
    private CerbereManager cerbereManager;

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
        //A chaque input fait avancer (incremente une unité dans le minigame controller correspondant a la position du player)
    }

    private void AlternateClick(Etat value) //false = a, true = b
    {
        if (hasClicked || !player.CanMove()) return;
        // player.isMoving = true;
        StartCoroutine(Cooldown());
        
        if (_etat != value)
        {
            Click();
            _etat = value;
        }
        else
        {
            FailAlternate();
            _etat = Etat.NULL;
        }

        // player.isMoving = false;
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
        StartCoroutine(WakeUpFeedBack(2));
        //Crer pour reveil cerbere
    }

    private IEnumerator WakeUpFeedBack(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        isShout = false;
        player.ChangeColor();
    }
    
    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
    }

    public enum Etat
    {
        A,B,NULL
    }
}
