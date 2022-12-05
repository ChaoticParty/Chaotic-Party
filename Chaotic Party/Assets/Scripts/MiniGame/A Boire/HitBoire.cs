using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoire : HitController
{
    public override void Hited(GameObject hitter)
    {
        if (player.isHit) return;
        
        Debug.Log("Boire");
        player.isHit = true;
        //Lancement de l'anim de hit
        player.gamepad.A.Disable();
        player.gamepad.B.Disable();
        player.gamepad.leftStick.Disable();
        player.gamepad.X.Disable();
        StartCoroutine(ReactivateInput());
        //Gestion de la chute du verre
        //Reactivation des valeurs du gamepad
    }
    
    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(2);
        player.gamepad.A.Enable();
        player.gamepad.B.Enable();
        player.gamepad.leftStick.Enable();
        player.gamepad.X.Enable();
        player.isHit = false;
    }
}
