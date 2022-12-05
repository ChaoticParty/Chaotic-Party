using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.ChangeColor(Color.grey);
        StartCoroutine(StopStun());
    }

    private IEnumerator StopStun()
    {
        yield return new WaitForSeconds(_stunTime);

        Debug.Log("player is no more stunned");
        _stunTime = defaultStunTime;
        player.ChangeColor();
        player.isStunned = false;
    }
}
