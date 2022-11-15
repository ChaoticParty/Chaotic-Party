using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPhoto : HitController
{
    public Vector2 hitForce;
    private Rigidbody2D myRigidbody;
    
    public override void Hited()
    {
        Debug.Log("Photo");
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
        yield return new WaitForSeconds(2);
        player.gamepad.A.Enable();
        player.gamepad.Y.Enable();
        player.gamepad.leftStick.Enable();
        player.gamepad.X.Enable();
    }
}
