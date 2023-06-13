using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class JumpController : MiniGameController
{
    [SerializeField] private AudioClip jumpSoundClip;
    private Rigidbody2D _rigidbody2D;
    public SpamButton jumpButton = SpamButton.A;
    public float jumpForce = 5;
    public bool canFootStool;
    public bool isJumping;
    [NotNull] public Transform footObject;
    private Collider2D _collider2D;

    #region CodeVariables

    private readonly Vector2 _downVector = Vector2.down;
    private readonly Vector2 _upVector = Vector2.up;
    private readonly Vector2 _raycastSize = new Vector2(0.49f, 0.01f);

    #endregion
    
    private new void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    public override void AddListeners()
    {
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

    private void OnEnable()
    {
        AddListeners();
    }

    private void FixedUpdate()
    {
        isJumping = !Physics2D.BoxCast(footObject.position, _raycastSize, 0, _downVector,
            0.1f, LayerMask.GetMask("Floor")).collider;
        player.isInTheAir = isJumping;

        if (isJumping)
        {
            player.Jump();
            if (_rigidbody2D.velocity.y <= 0)
            {
                player.Fall();
            }
        }
        else
        {
            player.StopFalling();
            if(Mathf.Abs(_rigidbody2D.velocity.y) < 2)
            {
                player.StopJumping();
            }
        }
    }

    private void Jump()
    {
        if (player.miniGameManager != null)
        {
            if(!player.miniGameManager.isMinigamelaunched) return;
        }
        if (player.CanAct())
        {
            Jumping();
        }
        else
        {
            Collider2D playerCollider = Physics2D.BoxCast((Vector2)footObject.position + _downVector * 0.2f, _raycastSize, 0, _upVector,
                0.1f, LayerMask.GetMask("Player")).collider;
            if (isJumping && canFootStool && playerCollider && playerCollider != _collider2D)
            {
                _rigidbody2D.velocity = Vector2.zero;
                Jumping();
                if (playerCollider.TryGetComponent(out StunController stunScript))
                {
                    stunScript.Stun();
                }
            }
        }
    }

    private void Jumping()
    {
        //_rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        if (jumpSoundClip != null)
        {
            gameObject.GetComponent<AudioSource>().clip = jumpSoundClip;
            player.soundManager.PlaySelfSound(gameObject.GetComponent<AudioSource>());
        }
        _rigidbody2D.velocity = new Vector2(0, jumpForce);
        player.Jump();
    }
}
