using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunController : MiniGameController
{
    [SerializeField] private float defaultStunTime = 0.5f;
    private float _stunTime;

    private new void Awake()
    {
        base.Awake();
        
        _stunTime = defaultStunTime;
    }

    /// <summary>
    /// Fonction qui va stun le joueur portant le script
    /// </summary>
    /// <param name="newStunTime">Temps de stun en secondes. Laisse vide si le temps voulu est le défaut du mini-jeu</param>
    public void Stun(float newStunTime = -1)
    {
        if(!player.CanBeStunned()) return;
        
        if (newStunTime > 0)
        {
            _stunTime = newStunTime;
        }

        Debug.Log("player got stunned");
        player.isStunned = true;
        DesactivateInput();
        if(player.GetComponent<Rigidbody2D>() != null) player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.ChangeColor(Color.grey);
        StartCoroutine(StopStun());
    }

    private void DesactivateInput()
    {
        player.gamepad.A.Disable();
        player.gamepad.B.Disable();
        player.gamepad.X.Disable();
        player.gamepad.Y.Disable();
        player.gamepad.leftStick.Disable();
    }
    private void ReactivateInput()
    {
        player.gamepad.A.Enable();
        player.gamepad.B.Enable();
        player.gamepad.X.Enable();
        player.gamepad.Y.Enable();
        player.gamepad.leftStick.Enable();
    }

    private IEnumerator StopStun()
    {
        yield return new WaitForSeconds(_stunTime);

        Debug.Log("player is no more stunned");
        _stunTime = defaultStunTime;
        player.ChangeColor();
        ReactivateInput();
        player.isStunned = false;
        //Voir comment on gere avec les anims
    }
}
