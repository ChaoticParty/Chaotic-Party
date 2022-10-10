using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float maxClamp;
    public float minClamp;
    
    private void Awake()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.leftStickMoved.AddListener(MoveHorizontally);
    }

    private void MoveHorizontally(float x, float y)
    {
        Vector3 position = transform.position;
        float newXPos = position.x + x / 30;
        newXPos = Mathf.Clamp(newXPos, minClamp, maxClamp);
        position = new Vector3(newXPos, position.y);
        transform.position = position;
    }
}
