using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CerbereSpamController : SpamController
{
    private StunController _stunController;
    private bool hasClicked = false;
    [HideInInspector] public bool isShout = false;
    [HideInInspector] public bool isUping = false;
    [HideInInspector] public Etat etat = Etat.NULL;
    private CerbereManager cerbereManager;
    [Header("Temps placeholder, a changer une fois les anims dispo")]
    [SerializeField] private TextMeshProUGUI bullPlayerFeedback;
    [SerializeField] private float standUpAnimTime = 0.75f;
    [SerializeField] private float wakeUpAnimTime = 1f;
    //Event
    public UnityEvent playerFall;

    protected new void Awake()
    {
        base.Awake();
        _stunController ??= GetComponent<StunController>();
        cerbereManager = spamManager as CerbereManager;
        player.ChangeBulleText("A ou B");
        AddListeners();
    }

    public override void AddListeners()
    {
        player.aJustPressed.AddListener(() => AlternateClick(Etat.A));
        player.bJustPressed.AddListener(() => AlternateClick(Etat.B));
        player.yJustPressed.AddListener(WakuUp);
    }

    protected override void Click()
    {
        spamManager.Click(player.index, spamManager.spamValue);
    }

    private void AlternateClick(Etat value)
    {
        if (hasClicked || !player.CanMove() || !cerbereManager.isMinigamelaunched) return;

        if (etat.Equals(Etat.FALL))
        {
            hasClicked = true;
            isUping = true;
            StartCoroutine(StandUpPlayer(standUpAnimTime));
            player.Releve();
            return;
        }
        
        StartCoroutine(Cooldown());

        if (etat != value)
        {
            Click();
            player.ChangeBulleText(value.Equals(Etat.A) ? "B" : "A");
            player.MarcheDiscretement(value.Equals(Etat.A) ? 1 : 0);
            etat = value;
        }
        else
        {
            FailAlternate();
            etat = Etat.FALL;
        }
    }

    private void FailAlternate()
    {
        //Anim de chute
        player.Chute();
        playerFall.Invoke();
        player.ChangeBulleText("Debout!");
        _stunController.Stun();
        //Reactivation a la fin de l'anim de chute, voir comment on gere avec le stun controller
    }

    private void WakuUp()
    {
        if (isShout || !player.CanMove() || !cerbereManager.isMinigamelaunched || cerbereManager.hasAlreadyShout[player.index]) return;
        player.ChangeBulleText("Hey!!!");
        isShout = true;
        cerbereManager.hasAlreadyShout[player.index] = true;
        cerbereManager.playerYell.Invoke(transform.position, "Argument");
        cerbereManager.hudMegaphone[player.index].GetComponent<Animator>().SetTrigger("Bigger");
        cerbereManager.PlayerWakeUp();
        StartCoroutine(WakeUpFeedBack(wakeUpAnimTime));
    }

    
    #region Feedback Methods

    private IEnumerator StandUpPlayer(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        isUping = false;
        if (!player.isStunned)
        {
            player.ChangeBulleText("A ou B");
        }
        hasClicked = false;
        etat = Etat.NULL;
    } 
    
    private IEnumerator WakeUpFeedBack(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        if (etat.Equals(Etat.NULL))
        {
            player.ChangeBulleText("A ou B");
        }
        else
        {
            player.ChangeBulleText(etat.Equals(Etat.A) ? "B" : "A");
        }
        isShout = false;
    }

    #endregion
    
    private IEnumerator Cooldown()
    {
        hasClicked = true;
        yield return new WaitForNextFrameUnit();
        hasClicked = false;
    }

    public enum Etat
    {
        A,B,FALL,NULL
    }
}
