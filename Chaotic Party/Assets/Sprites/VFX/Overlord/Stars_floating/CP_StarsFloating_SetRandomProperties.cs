using System.Collections.Generic;
using UnityEngine;

public class CP_StarsFloating_SetRandomProperties : MonoBehaviour
{
    public List<GameObject> stars;
    void Start()
    {
        foreach (GameObject star in stars)
        {   
            star.transform.localScale = new Vector3(Random.Range(0.005f, 0.01f), Random.Range(0.005f, 0.01f), 1);
            star.transform.rotation = new Quaternion(1, 1, Random.Range(-360, 360), 1);
        }
    }
}
