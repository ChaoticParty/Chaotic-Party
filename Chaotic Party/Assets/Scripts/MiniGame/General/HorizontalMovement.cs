using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class HorizontalMovement : MiniGameController
{
    public float speed = 5;
    public float minClamp = -8;
    public float maxClamp = 8;
    private Rigidbody2D _rigidbody2D;
    private bool right = false;
    private Transform playerTransform;
    private Vector3 watchingRight;
    private Vector3 watchingLeft;
    
    private new void Awake()
    {
        base.Awake();
        playerTransform = player.transform;
        var localScale = playerTransform.localScale;
        watchingRight = new Vector3(1, localScale.y, localScale.z);
        watchingLeft = new Vector3(-1, localScale.y, localScale.z);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player.leftStickMoved.AddListener(MoveHorizontally);
    }

    private void FixedUpdate()
    {
        if (minClamp == maxClamp) return;

        Vector3 position = transform.position;
        position = new Vector3(Mathf.Clamp(position.x, minClamp, maxClamp), position.y);
        transform.position = position;
    }

    private void MoveHorizontally(float x, float y)
    {
        if (!player.CanMove())
        {
            return;
        }

        switch (x)
        {
            case > 0:
            {
                if (!right)
                {
                    right = true;
                    playerTransform.localScale = watchingRight;
                }
                break;
            }
            case < 0:
            {
                if (right)
                {
                    right = false;
                    playerTransform.localScale = watchingLeft;
                }
                break;
            }
        }
        _rigidbody2D.velocity = new Vector2(x * speed, _rigidbody2D.velocity.y);
        Vector3 position = transform.position;
        position = new Vector3(Mathf.Clamp(position.x, minClamp, maxClamp), position.y);
        transform.position = position;
    }
}
