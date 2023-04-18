using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerbereAnimEvent : MonoBehaviour
{
    [Header("Gestion du cerbere")]
    [SerializeField] [Tooltip("Manager du cerbere")] private CerbereManager cerbereManager;
    [SerializeField] [Tooltip("Animator du cerbere")] private Animator animator;
    [HideInInspector] public bool isRompiche = true;
    [HideInInspector] public bool canWakeUpEnd = true;
    [Space]
    [Header("Gameobject du cerbere")]
    [SerializeField] [Tooltip("Tête du cerbere")] private GameObject tete;
    [SerializeField] [Tooltip("Animator du ! du cerbere")] private Animator exclamationAnimator;
    [Space]
    [Header("Sprites du cerbere")]
    [SerializeField] [Tooltip("Sprite du dodo du cerbere")] private Sprite cerbereDodo;
    [SerializeField] [Tooltip("Sprite du cerbere qui observe")] private Sprite cerbereObserve;
    
    //Id des params d'animator
    private static readonly int ObserveTrigger = Animator.StringToHash("ObserveTrigger");
    private static readonly int UltimoPoderLaser = Animator.StringToHash("UltimoPoderLaser");
    private static readonly int AuDodoTrigger = Animator.StringToHash("AuDodoTrigger");
    private static readonly int Property = Animator.StringToHash("!Pop");

    public void WakeUpEnd()
    {
        if (!canWakeUpEnd)
        {
            canWakeUpEnd = true;
            return;
        }
        
        tete.GetComponent<SpriteRenderer>().sprite = cerbereObserve;
        animator.SetTrigger(ObserveTrigger);
        isRompiche = false;
        canWakeUpEnd = false;
    }

    public void ObserveEnd()
    {
        tete.GetComponent<SpriteRenderer>().sprite = cerbereDodo;
        animator.SetBool(UltimoPoderLaser, false);
        animator.SetTrigger(AuDodoTrigger);
        cerbereManager.ResetBulleAnim();
        isRompiche = true;
    }

    public void Exclamation()
    {
        exclamationAnimator.SetTrigger(Property);
    }
}
