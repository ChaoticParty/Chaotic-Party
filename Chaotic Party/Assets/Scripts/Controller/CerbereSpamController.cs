using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CerbereSpamController : SpamController
{
    private StunController _stunController;
    private bool hasClicked = false;
    private bool lastClickedInput = false;

    protected new void Awake()
    {
        base.Awake();
        _stunController ??= GetComponent<StunController>();
        player.aJustPressed.AddListener(() => AlternateClick(false));
        player.bJustPressed.AddListener(() => AlternateClick(true));
    }

    protected override void Click()
    {
        spamManager.Click(player.index, spamManager.spamValue);
        //Gestion du mouvement ici
        //A chaque input fait avancer (incremente une unité dans le minigame controller correspondant a la position du player)
    }

    private void AlternateClick(bool value) //false = a, true = b
    {
        if (hasClicked) return;
        StartCoroutine(Cooldown());
        
        if (lastClickedInput != value)
        {
            Click();
        }
        else
        {
            FailAlternate();
        }
        lastClickedInput = value;
    }

    private void FailAlternate()
    {
        //Anim de chute
        _stunController.Stun();
        //Reactivation a la fin de l'anim de chute, voir comment on gere avec le stun controller
    }
    
    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
    }
}
