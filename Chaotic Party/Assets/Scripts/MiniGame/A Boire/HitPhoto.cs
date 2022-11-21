using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPhoto : HitController
{
    public Vector2 hitForce;

    public override void Hited()
    {
        if (player.isHit) return;
        
        Debug.Log("Photo");
        player.isHit = true;
        //Lancement de l'anim de hit
        player.gamepad.A.Disable();
        player.gamepad.Y.Disable();
        player.gamepad.leftStick.Disable();
        player.gamepad.X.Disable();
        myRigidbody.AddForce(hitForce * transform.localScale); //Propulsion
        StartCoroutine(ReactivateInput());
        //Reactivation des valeurs du gamepad
    }

    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(0.5f);
        player.gamepad.A.Enable();
        player.gamepad.Y.Enable();
        player.gamepad.leftStick.Enable();
        player.gamepad.X.Enable();
        player.isHit = false;
    }
}
