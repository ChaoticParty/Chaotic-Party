using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BAM_fx : MonoBehaviour
{
    public Animator bamAnimator;
    public CP_OnomatopeChoice script;
    
    [Button("BAM")]
    public void BAM()
    {
        script.BAM();
        bamAnimator.SetTrigger("BAM");
    }
}
