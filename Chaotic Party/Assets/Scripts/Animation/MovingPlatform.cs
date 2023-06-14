using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Animator> gearAnimators = new();

    public void ToggleGears()
    {
        foreach (Animator animator in gearAnimators)
        {
            animator.SetTrigger("Toggle");
        }
    }

    
}
