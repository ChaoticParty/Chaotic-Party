using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacleDetector : MonoBehaviour
{
    public LayerMask layer;
    public List<GameObject> floorObjects = new();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CheckLayer(other))
        {
            floorObjects.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exit");
        if (CheckLayer(other))
        {
            Debug.Log("remove");
            floorObjects.Remove(other.gameObject);
        }
    }

    private bool CheckLayer(Collider2D other)
    {
        return CheckLayer(other.gameObject.layer);
    }

    private bool CheckLayer(int otherLayer)
    {
        return (int)Mathf.Pow(2, otherLayer) == layer;
    }

    public bool IsFloored()
    {
        return floorObjects.Count >= 1;
    }
}
