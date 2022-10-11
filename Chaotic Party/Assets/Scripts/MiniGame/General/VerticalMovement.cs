using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public float maxClamp;
    public float minClamp;
    
    private void Awake()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.leftStickMoved.AddListener(MoveVertically);
    }

    private void MoveVertically(float x, float y)
    {
        Vector3 position = transform.position;
        float newYPos = position.y + y / 30;
        newYPos = Mathf.Clamp(newYPos, minClamp, maxClamp);
        position = new Vector3(position.x, newYPos);
        transform.position = position;
    }
}
