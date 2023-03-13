using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class SetAnimProperties : MonoBehaviour
{
    [ValueDropdown(nameof(GetAnimationNames))]
    public string animationName = "Rotate";
    public int speed = 1;
    [Tooltip("Type de l'offset ci-dessous. Time est un temps en secondes, NormalizedTime est un temps en \"pourcentage\" (Exemple: 0.5 sera la moitié de l'anim)")]
    public AnimOffsetType offsetType = AnimOffsetType.Time;
    [ShowIf(nameof(ShouldTimeShow))]
    public float animTimeOffset;
    private Animator _animator;

#if UNITY_EDITOR
    public string[] GetAnimationNames()
    {
        _animator = GetComponent<Animator>();
        AnimatorController ac = _animator.runtimeAnimatorController as AnimatorController;
        AnimatorControllerLayer[] acLayers = ac.layers;
        List<string> allStates = new();
        foreach (AnimatorControllerLayer i in acLayers)
        {
            ChildAnimatorState[] animStates = i.stateMachine.states;
            foreach (ChildAnimatorState j in animStates) 
            {
                allStates.Add(j.state.name);
            }
        }

        return allStates.ToArray();
    }

    public bool ShouldTimeShow()
    {
        return offsetType != AnimOffsetType.Random;
    }
#endif

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
            case AnimOffsetType.Random:
                _animator.PlayInFixedTime(animationName, -1, Random.Range(0f, 1f));
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
    NormalizedTime, 
    Random
}