using UnityEngine;

public class SetAnimProperties : MonoBehaviour
{
    public string animationName = "Rotate";
    public int speed = 1;
    public float animTimeOffset;
    private Animator _animator;

    private void Awake()
    {
        _animator ??= GetComponent<Animator>();
        if(!_animator) return;
        
        _animator.PlayInFixedTime(animationName, -1, animTimeOffset);
        _animator.speed = speed;
    }
}
