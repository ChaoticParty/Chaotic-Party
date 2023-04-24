using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform transformToCopy;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = transformToCopy.localScale;
    }
}
