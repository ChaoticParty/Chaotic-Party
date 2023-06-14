using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    //TODO a like dans l'animator
    public void PlayIdleSound()
    {
        player.PlayIdleSound();
    }
    
    public void PlayWalkSound()
    {
        player.PlayMarcheSound();
    }
}
