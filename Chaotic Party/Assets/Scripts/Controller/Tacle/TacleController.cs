using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TacleController : MiniGameController
{
    public float impulsion;
    public GameObject tacleChild;
    
    private Vector2 forceTacle;
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
        //Lancement de l'anim
        isTacling = true;
        player.isTackling = true;
        player.gamepad.leftStick.Disable();
        rigidbody2D.velocity = Vector2.zero;
        player.gamepad.A.Disable();
        player.gamepad.X.Disable();
        forceTacle = new Vector2(impulsion * transform.localScale.x, 0);
        rigidbody2D.AddForce(forceTacle, ForceMode2D.Force);
        StartCoroutine(ReactivateInput());
        //Remettre isTacling a false et reactiver les touches A et stick gauche a la fin de l'anim
    }
    
    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(1);
        player.gamepad.A.Enable();
        player.gamepad.X.Enable();
        player.gamepad.leftStick.Enable();
        isTacling = false;
        player.isTackling = false;
    }
}
