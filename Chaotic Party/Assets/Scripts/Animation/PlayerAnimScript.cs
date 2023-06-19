using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    public void PlayIdleSound()
    {
        if (player != null) player.PlayIdleSound();
    }
    public void PlayTricheSound()
    {
        if (player != null) player.PlayTricheSound();
    }
    public void PlayHappySound()
    {
        if (player != null) player.PlayHappySound();
    }
    public void PlaySadSound()
    {
        if (player != null) player.PlaySadSound();
    }
    public void PlayHitSound()
    {
        if (player != null) player.PlayHitSound();
    }
    public void PlayJumpSound()
    {
        if (player != null) player.PlayJumpSound();
    }
    
    public void PlayWalkSound()
    {
        if (player != null) player.PlayMarcheSound();
    }
}
