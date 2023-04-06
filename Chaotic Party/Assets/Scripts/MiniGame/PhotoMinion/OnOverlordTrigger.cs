using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnOverlordTrigger : MonoBehaviour
{
    public List<PlayerController> playersTouchingOverlord = new ();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player) && !playersTouchingOverlord.Contains(player))
        {
            playersTouchingOverlord.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player) && playersTouchingOverlord.Contains(player))
        {
            playersTouchingOverlord.Remove(player);
        }
    }
}
