using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BAM_fx : MonoBehaviour
{
    public Animator bamAnimator;
    
    [Button("BAM")]
    public void BAM()
    {
        bamAnimator.SetTrigger("BAM");
    }
}
