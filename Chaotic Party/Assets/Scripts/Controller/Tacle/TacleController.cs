using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TacleController : MiniGameController
{
    public GameObject tacleChild;
    
    public Vector2 forceTacle;
    private Rigidbody2D rigidbody2D;

    private bool isTacling = false;
    private new void Awake()
    {
        base.Awake();
        Instantiate(tacleChild, transform);
        rigidbody2D = player.GetComponent<Rigidbody2D>();
        player.xJustPressed.AddListener(Tacled);
    }

    private void Tacled()
    {
        if (!player.miniGameManager.isMinigamelaunched || isTacling)
        {
            return;
        }
        //Lancement de l'anim
        isTacling = true;
        player.isTackling = true;
        player.gamepad.leftStick.Disable();
        rigidbody2D.velocity = forceTacle * player.transform.localScale.x;
        player.gamepad.A.Disable();
        player.gamepad.X.Disable();
        StartCoroutine(ReactivateInput());
        //Remettre isTacling a false et reactiver les touches A et stick gauche a la fin de l'anim
    }
    
    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(0.5f);
        player.gamepad.A.Enable();
        player.gamepad.X.Enable();
        player.gamepad.leftStick.Enable();
        isTacling = false;
        player.isTackling = false;
        rigidbody2D.velocity = Vector2.zero;
    }
}
