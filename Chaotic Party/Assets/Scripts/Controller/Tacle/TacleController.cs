using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class TacleController : MiniGameController
{
    [SerializeField] private AudioClip tacleSoundClip;
    public GameObject tacleChild;
    
    public Vector2 forceTacle;
    private Rigidbody2D rigidbody2D;
    public TacleDetectorManager tacleDetectorManager;

    private bool isTacling = false;
    private new void Awake()
    {
        base.Awake();
        Instantiate(tacleChild, transform);
        rigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(!isTacling) return;
        
        if(tacleDetectorManager != null) tacleDetectorManager.IsFloored();
    }

    public override void AddListeners()
    {
        player.xJustPressed.AddListener(Tacled);
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void Tacled()
    {
        if (player.miniGameManager != null)
        {
            if (!player.miniGameManager.isMinigamelaunched) return;
        }
        if (!player.CanAct()) return;
        //Lancement de l'anim
        if (tacleSoundClip != null)
        {
            gameObject.GetComponent<AudioSource>().clip = tacleSoundClip;
            player.soundManager.PlaySelfSound(gameObject.GetComponent<AudioSource>());
        }
        
        isTacling = true;
        player.isTackling = true;
        player.gamepad.leftStick.Disable();
        rigidbody2D.velocity = new Vector3(forceTacle.x * player.transform.localScale.x, forceTacle.y);
        player.gamepad.A.Disable();
        player.gamepad.X.Disable();
        player.StartTacle();
        StartCoroutine(ReactivateInput());
        //Remettre isTacling a false et reactiver les touches A et stick gauche a la fin de l'anim
    }
    
    IEnumerator ReactivateInput()
    {
        yield return new WaitForSeconds(0.5f);
        player.gamepad.A.Enable();
        player.gamepad.X.Enable();
        player.gamepad.leftStick.Enable();
        isTacling = false;
        player.isTackling = false;
        rigidbody2D.velocity = Vector2.zero;
        player.StopTacle();
    }
}
