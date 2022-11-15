using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class JumpController : MiniGameController
{
    private Rigidbody2D _rigidbody2D;
    public SpamButton jumpButton = SpamButton.A;
    public float jumpForce = 5;
    public bool isJumping;
    public Transform footObject;

    #region CodeVariables

    private readonly Vector2 _downVector = Vector2.down;
    private readonly Vector2 _raycastSize = new Vector2(1, 0.01f);

    #endregion
    
    private new void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        UnityEvent buttonEvent = new();
        switch (jumpButton)
        {
            case SpamButton.A:
                buttonEvent = player.aJustPressed;
                break;
            case SpamButton.B:
                buttonEvent = player.bJustPressed;
                break;
            case SpamButton.X:
                buttonEvent = player.xJustPressed;
                break;
            case SpamButton.Y:
                buttonEvent = player.yJustPressed;
                break;
        }
        buttonEvent.AddListener(Jump);
    }

    private void FixedUpdate()
    {
        isJumping = !Physics2D.BoxCast(footObject.position, _raycastSize, 0, _downVector,
            0.1f, LayerMask.GetMask("Floor")).collider;
    }

    private void Jump()
    {
        if (isJumping) return;
        _rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
