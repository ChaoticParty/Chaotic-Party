using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CerbereSpamController : SpamController
{
    private StunController _stunController;
    private bool hasClicked = false;
    [HideInInspector] public bool isMoving = false;
    private Etat _etat = Etat.NULL;

    protected new void Awake()
    {
        base.Awake();
        _stunController ??= GetComponent<StunController>();
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
        if (hasClicked || player.isStunned) return;
        player.isMoving = true;
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

        player.isMoving = false;
    }

    private void FailAlternate()
    {
        //Anim de chute
        _stunController.Stun();
        //Reactivation a la fin de l'anim de chute, voir comment on gere avec le stun controller
    }

    private void WakuUp()
    {
        isMoving = true;
        isMoving = false;
        //Crer pour reveil cerbere
    }

    public bool IsMoving()
    {
        return isMoving || player.isStunned;
    }
    
    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
    }
    
    private enum Etat
    {
        A,B,NULL
    }
}
