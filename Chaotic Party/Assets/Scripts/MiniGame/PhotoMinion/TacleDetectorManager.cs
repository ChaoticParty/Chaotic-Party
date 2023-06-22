using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacleDetectorManager : MonoBehaviour
{
    public List<TacleDetector> TacleDetectors = new List<TacleDetector>();
    public int threshold = 4;
    public PlayerController player;

    public void IsFloored()
    {
        int nbFloored = 0;

        foreach (TacleDetector tacleDetector in TacleDetectors)
        {
            if (tacleDetector.IsFloored()) nbFloored++;
        }

        if (nbFloored == threshold)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        
        Debug.Log(nbFloored);
    }
}
