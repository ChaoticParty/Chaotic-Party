using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class OrientationProjecteur : MonoBehaviour
{
    public Transform OverlordPosition;

    [Button ("Orienter le projecteur")]
    public void LookAtOverlord()
    {
        transform.LookAt(OverlordPosition);
    }
}
