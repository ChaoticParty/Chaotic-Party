using System;
using System.Collections.Generic;
using UnityEngine;

public class CrownManager : MonoBehaviour
{
    public List<GameObject> ObjectsToActivate;
    public List<GameObject> ObjectsToDeactivate;
    public List<AnimationData> AnimationDatas;

    public void SetCrown(bool isCrownOn)
    {
        foreach (GameObject go in ObjectsToActivate)
        {
            go.SetActive(isCrownOn);
        }
        foreach (GameObject go in ObjectsToDeactivate)
        {
            go.SetActive(!isCrownOn);
        }
        foreach (AnimationData data in AnimationDatas)
        {
            if (data.isCrown == isCrownOn)
            {
                data.animator.SetTrigger(data.trigger);
            }
        }
    }

    [Serializable]
    public struct AnimationData
    {
        public Animator animator;
        public string trigger;
        public bool isCrown;
    }
}
