using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Animator> gearAnimators = new();
    public Animator animator;
    public SoundManager soundManager;
    public AudioSource source;

    public void ToggleGears()
    {
        if(source.isPlaying) soundManager.StopSelfSound(source);
        else soundManager.PlaySelfSound(source, true);
        foreach (Animator animator in gearAnimators)
        {
            animator.SetTrigger("Toggle");
        }
    }
}
