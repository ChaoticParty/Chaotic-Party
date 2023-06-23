using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPicturePoints : MonoBehaviour
{
    public Animator animator;
    private static readonly int Points1 = Animator.StringToHash("Points");

    public void StartAnimation(Transform ObjectTransform)
    {
        transform.position = ObjectTransform.position;
        animator.gameObject.SetActive(true);
        animator.SetTrigger(Points1);
    }

    public void Deactivate()
    {
        animator.gameObject.SetActive(false);
    }
}
