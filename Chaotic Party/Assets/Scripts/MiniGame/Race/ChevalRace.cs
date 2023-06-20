using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChevalRace : MonoBehaviour
{
    public Animator positionAnimator;
    public Animator chevalAnimator;
    public AudioSource source;

    public void StartAnimation()
    {
        positionAnimator.SetTrigger("Cheval");
        //positionAnimator.transform.GetChild(0).gameObject.SetActive(true);
        chevalAnimator.SetTrigger("Cheval");
    }

    public void PlaySound()
    {
        source.Play();
    }

    public void MoveHorse()
    {
        //transform.SetParent(voiture);
    }

    public void Destroy()
    {
        positionAnimator.SetTrigger("Fin");
        chevalAnimator.SetTrigger("Fin");
        Pooling.instance.ReturnToPool(gameObject);
    }
}
