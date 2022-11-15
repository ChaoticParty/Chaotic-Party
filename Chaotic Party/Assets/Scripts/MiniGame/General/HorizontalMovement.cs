using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class HorizontalMovement : MiniGameController
{
    public float speed;
    public float maxClamp;
    public float minClamp;
    private Rigidbody2D _rigidbody2D;
    
    private new void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player.leftStickMoved.AddListener(MoveHorizontally);
    }

    private void MoveHorizontally(float x, float y)
    {
        _rigidbody2D.velocity = new Vector2(x * speed, _rigidbody2D.velocity.y);
        Vector3 position = transform.position;
        position = new Vector3(Mathf.Clamp(position.x, minClamp, maxClamp), position.y);
        transform.position = position;
    }
}
