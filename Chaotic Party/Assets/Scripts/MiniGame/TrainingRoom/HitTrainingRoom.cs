using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrainingRoom : HitController
{
    public Vector2 hitForce;

    public override void Hited(GameObject hitter)
    {
        if (player.isHit) return;
        
        Debug.Log("Photo");
        player.isHit = true;
        //Lancement de l'anim de hit
        myRigidbody.AddForce(hitForce * hitter.transform.localScale); //Propulsion
        StartCoroutine(ReactivateInput());
        //Reactivation des valeurs du gamepad
    }

    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(0.5f);
        player.isHit = false;
    }
}
