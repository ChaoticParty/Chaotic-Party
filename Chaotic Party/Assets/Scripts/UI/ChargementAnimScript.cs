using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargementAnimScript : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    private bool isPlusAnim = false;
    public UnityEvent animEndAction;

    public void ChangeBoolean()
    {
        isPlusAnim = true;
    }

    public void Action()
    {
        animEndAction.Invoke();
        isPlusAnim = false;
    }
}
