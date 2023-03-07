using System;
using UnityEngine;

public class SetAnimProperties : MonoBehaviour
{
    public string animationName = "Rotate";
    public int speed = 1;
    [Tooltip("Type de l'offset ci-dessous. Time est un temps en secondes, NormalizedTime est un temps en \"pourcentage\" (Exemple: 0.5 sera la moitié de l'anim)")]
    public AnimOffsetType offsetType = AnimOffsetType.Time;
    public float animTimeOffset;
    private Animator _animator;

    private void Awake()
    {
        _animator ??= GetComponent<Animator>();
        if(!_animator) return;

        switch (offsetType)
        {
            case AnimOffsetType.Time:
                _animator.PlayInFixedTime(animationName, -1, animTimeOffset);
                break;
            case AnimOffsetType.NormalizedTime:
                _animator.Play(animationName, -1, animTimeOffset);
                break;
            default:
                _animator.PlayInFixedTime(animationName, -1, animTimeOffset);
                break;
        }
        _animator.speed = speed;
    }
}

[Serializable]
public enum AnimOffsetType
{
    Time,
    NormalizedTime
}