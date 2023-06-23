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
    [SerializeField] [Tooltip("Animator du ! du cerbere")] private Animator exclamationAnimator;
    
    //Id des params d'animator
    private static readonly int ObserveTrigger = Animator.StringToHash("ObserveTrigger");
    private static readonly int UltimoPoderLaser = Animator.StringToHash("UltimoPoderLaser");
    private static readonly int AuDodoTrigger = Animator.StringToHash("AuDodoTrigger");
    private static readonly int Property = Animator.StringToHash("!Pop");
    private static readonly int WakeUpTrigger = Animator.StringToHash("WakeUpTrigger");

    public void WakeUpEnd()
    {
        if (!canWakeUpEnd)
        {
            canWakeUpEnd = true;
            return;
        }
        canWakeUpEnd = false;
        
        animator.SetTrigger(ObserveTrigger);
        isRompiche = false;
    }

    public void ChangeHeadToDodo()
    {
    }

    public void ObserveEnd()
    {
        animator.SetBool(UltimoPoderLaser, false);
        animator.SetTrigger(AuDodoTrigger);
        animator.ResetTrigger(WakeUpTrigger);
        cerbereManager.ResetBulleAnim();
        isRompiche = true;
    }
    public void CheapObserveEnd() //Workaround pour avoir le cerbere éveillé dès le début
    {
        animator.SetBool(UltimoPoderLaser, false);
        animator.SetTrigger("AuDodoTrigger");
    }

    public void Exclamation()
    {
        if (exclamationAnimator != null) exclamationAnimator.SetTrigger(Property);
    }
}
