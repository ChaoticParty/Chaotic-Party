using System.Collections.Generic;
using UnityEngine;

public class CrownManager : MonoBehaviour
{
    public List<GameObject> ObjectsToActivate;
    public List<GameObject> ObjectsToDeactivate;

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
    }
}
