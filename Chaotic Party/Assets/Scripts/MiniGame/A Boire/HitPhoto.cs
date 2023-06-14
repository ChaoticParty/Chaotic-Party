using System.Collections;
using UnityEngine;

public class HitPhoto : HitController
{
    public Vector2 hitForce;

    public override void Hited(GameObject hitter)
    {
        if (player.isHit) return;
        
        Debug.Log("Photo");
        player.isHit = true;
        player.PlayHitSound();
        if (bamPrefab != null) Instantiate(bamPrefab, transform.position, Quaternion.identity);
        //Lancement de l'anim de hit
        player.gamepad.A.Disable();
        player.gamepad.Y.Disable();
        player.gamepad.leftStick.Disable();
        player.gamepad.X.Disable();
        Debug.Log("add force");
        myRigidbody.AddForce(new Vector3(hitForce.x * hitter.transform.localScale.x, hitForce.y)); //Propulsion
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
