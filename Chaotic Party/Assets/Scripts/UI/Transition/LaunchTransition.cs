using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LaunchTransition : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Launch = Animator.StringToHash("Launch");

    public Animator animator
    {
        get
        {
            if (!_animator) _animator = GetComponent<Animator>();
            return _animator;
        }
        set => _animator = value;
    }

    [Button]
    public void LaunchAnim()
    {
        animator.SetTrigger(Launch);
    }

    [Button]
    public void LaunchAnim(Vector3 position)
    {
        transform.position = position;
        LaunchAnim();
    }

    [Button]
    public void LaunchAnim(Transform obj)
    {
        LaunchAnim(obj.position);
    }
}
