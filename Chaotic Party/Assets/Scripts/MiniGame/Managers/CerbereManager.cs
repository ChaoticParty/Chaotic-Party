using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CerbereManager : SpamManager
{
    [Header("Setup")]
    // [SerializeField] private ParticleSystem rompicheEffect;
    [SerializeField] private CanvasGroup Hud;
    [SerializeField] [Tooltip("Tableau d'animator, de 0 à 3, correspondant aux players")] private Animator[] cerbereAnimator;
    [SerializeField] [Tooltip("Tableau des cerberes, de 0 à 3, correspondant aux players")] private CerbereAnimEvent[] cerbereAnimEvents;
    [SerializeField] [Tooltip("Animator de la bulle de cerbere")] private Animator bulleAnimator;
    [SerializeField] [Tooltip("Animator des nuages de la bulle de cerbere")] private Animator[] nuagesAnimator;
    private int winnerIndex;
    private bool[] wasHittedByCerbere; //Tableau de bool, true si a été touché. Repasse a false quand Cerbere se rendort. De 0 à 3, correspondant aux players;
    private float[] walkDestination = new float[]{};
    private Coroutine myCoroutine;
    [HideInInspector] public RompicheState rompicheState;
    // private bool isRompiche = true;
    private float timeBeforeWake = 0;
    private float timePassedBeforeWake = 0;
    private float xStartValuePos = 0;
    private float xEndValuePos = 0;
    private float inGameValuePerClick = 0;
    [Space]
    [Header("Distance avec Cerbere")]
    [SerializeField] [Tooltip("Valeur indiquant la distance entre les joueurs et cerbere")] private int endValue = 100;
    [Space]
    [Header("Rompiche")]
    [SerializeField] [Range(0,120)] [Tooltip("Valeur basse du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberRompicheRangeMin;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur haute du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberRompicheRangeMax;
    [Space]
    [Header("Réveil")]
    [SerializeField] [Tooltip("le temps que met le cerbere pour relever la tête")] private float cerberBeginAnimTime = 1.5f;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur basse du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberWakeUpTimeRangeMin;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur haute du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberWakeUpTimeRangeMax;
    [Space] 
    [Header("Score UI")] 
    [SerializeField] private List<TextMeshProUGUI> scoreDisplay = new List<TextMeshProUGUI>(); //Les 4 ui de scores
    //Potentiellement a tej vu que c'est placeholder
    [SerializeField] private List<GameObject> laserPlaceHolder; //Les 4 laser placeholders
    [SerializeField] private TextMeshProUGUI winTMP; //Le msg de win
    //
    [Space] 
    [Header("Cerbere UI")] 
    [SerializeField] private List<SpriteRenderer> listTeteCerbere = new List<SpriteRenderer>(); //Les 4 tetes
    
    #region Events

    [Space, Header("Events")] //A placer dans le code
    public UnityEvent<Vector2, string> playerWalk;
    public UnityEvent<Vector2, string> playerGetBackToStart;
    public UnityEvent<Vector2, string> playerYell;
    public UnityEvent cerbereLaser;
    public UnityEvent cerbereSee;
    
    //Id des param d'animator
    private static readonly int WakeUpTrigger = Animator.StringToHash("WakeUpTrigger");
    private static readonly int ObserveTrigger = Animator.StringToHash("ObserveTrigger");
    private static readonly int UltimoPoderLaser = Animator.StringToHash("UltimoPoderLaser");
    private static readonly int AuDodoTrigger = Animator.StringToHash("AuDodoTrigger");
    
    private static readonly int LaunchBulleDisappear = Animator.StringToHash("LaunchBulleDisappear");
    private static readonly int LaunchFirstDisappear = Animator.StringToHash("LaunchFirstDisappear");
    private static readonly int LaunchSecondDisappear = Animator.StringToHash("LaunchSecondDisappear");
    private static readonly int LaunchThirdDisappear = Animator.StringToHash("LaunchThirdDisappear");
    private static readonly int Reset = Animator.StringToHash("Reset");
    private static readonly int Depop = Animator.StringToHash("Depop");
    private static readonly int Pop = Animator.StringToHash("Pop");

    #endregion

    private new void Start()
    {
        base.Start();
    }

    [ContextMenu("LoadMiniGame")]
    public override void LoadMiniGame()
    {
        base.LoadMiniGame();
        rompicheState = RompicheState.NULL;
        if (cerberRompicheRangeMin > cerberRompicheRangeMax)
        {
            Debug.Log("Valeur minimal supérieur à la maximale, inversion des champs pour pouvoir lancer le minijeu");
            (cerberRompicheRangeMax, cerberRompicheRangeMin) = (cerberRompicheRangeMin, cerberRompicheRangeMax);
        }
        timeBeforeWake = Random.Range(cerberRompicheRangeMin, cerberRompicheRangeMax + 1);
        timePassedBeforeWake = timeBeforeWake;
        
        wasHittedByCerbere = new[] {false, false, false, false};
        
        xStartValuePos = players[0].transform.position.x;
        xEndValuePos = listTeteCerbere[0].transform.position.x - 1; //Le -1 est un padding pour que l'arrivée soit devant cerbere et pas dessus, a changer en fonction
        
        float distance = xEndValuePos - xStartValuePos;
        float valueForOneClick = endValue / spamValue;
        inGameValuePerClick = distance / valueForOneClick;
        
        walkDestination = new float[players.Count];
        for (int i = 0; i < walkDestination.Length; i++)
        {
            if(i >= players.Count) continue;
            walkDestination[i] = players[i].transform.position.x;
        }

        for (var i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.activeSelf)
            {
                players[i].ActivateBulle(true);
                scoreDisplay[i].transform.parent.gameObject.SetActive(true); //TODO avoir la foi de changer ca, c'est moche et pas opti
                scoreDisplay[i].text = "0";
            }
        }
        
        bulleAnimator.gameObject.SetActive(true);

        Hud.alpha = 1;
    }

    private void FixedUpdate()
    {
        if (!isMinigamelaunched) return;
        for (int i = 0; i < walkDestination.Length; i++)
        {
            if(i >= players.Count) continue;
            if (walkDestination[i] > xEndValuePos) walkDestination[i] = xEndValuePos;
            if (players[i].transform.position.x >= walkDestination[i]) continue;
            
            players[i].transform.position = new Vector3(Mathf.Lerp(players[i].transform.position.x, walkDestination[i], Time.deltaTime * 10),
                players[i].transform.position.y, players[i].transform.position.z);
            
            if (walkDestination[i] - players[i].transform.position.x <= 0.01f)
                players[i].transform.position = new Vector3(walkDestination[i], players[i].transform.position.y, players[i].transform.position.z);
        }

        foreach (var animEvent in cerbereAnimEvents)
        {
            if (animEvent.isRompiche) continue;

            if (myCoroutine == null) myCoroutine = StartCoroutine(Observe());
            
            for (int i = 0; i < players.Count; i++)
            {
                if ((players[i].GetComponent<CerbereSpamController>().isShout || Math.Abs(players[i].transform.position.x - walkDestination[i]) > 0.01f) && wasHittedByCerbere[i].Equals(false))
                {
                    cerbereSee.Invoke();
                    clicksArray[i] = 0;
                    cerbereAnimator[i].SetBool(UltimoPoderLaser, true);
                    laserPlaceHolder[i].SetActive(true);
                    wasHittedByCerbere[i] = true;
                    walkDestination[i] = xStartValuePos;
                    //Feedback
                    cerbereAnimEvents[0].Exclamation();
                    cerbereLaser.Invoke();
                    playerGetBackToStart.Invoke(players[i].transform.position, "Argument");
                    //
                    players[i].transform.position = new Vector3(xStartValuePos, players[i].transform.position.y,
                        players[i].transform.position.z);
                    players[i].ChangeBulleText("Lasered");
                    scoreDisplay[i].text = "0";
                    players[i].isStunned = true;
                    players[i].GetComponent<CerbereSpamController>().etat = CerbereSpamController.Etat.NULL;
                }    
            }
        }
        if (timePassedBeforeWake >= 0) //Décrémentation temps avant reveille Cerbere
        {
            timePassedBeforeWake -= Time.deltaTime;
            if (timePassedBeforeWake > timeBeforeWake / 3 * 2)
            {
                rompicheState = RompicheState.TROIS;
            }
            else if (timePassedBeforeWake > timeBeforeWake / 3)
            {
                rompicheState = RompicheState.DEUX;
            }
            else
            {
                rompicheState = RompicheState.UN;
            }
            //Animation (les z du dodo qui decrementent ?)
        }
        else //Lancement d'une nouvelle boucle de dodo
        {
            // if (!rompicheState.Equals(RompicheState.ZERO)) return;
            WakeUp();
            rompicheState = RompicheState.NULL;
        }
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        LaunchBulleAnim(rompicheState);
        // StartCoroutine(ZNumberFeedBack(timeBeforeWake));
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        //BUG Si on descactive les input avec le stun et qu'on r'appui, tt les inputs sont compté et nous "tp" a la fin, Envoyer une action dans le stun ? (peut etre plus d'actu)
        if (!isMinigamelaunched) return;
        walkDestination[playerIndex] += inGameValuePerClick;
        clicksArray[playerIndex] += Mathf.RoundToInt(spamValue);
        scoreDisplay[playerIndex].text = clicksArray[playerIndex].ToString();
        DisplayCrown();
        playerWalk.Invoke(players[playerIndex].transform.position,"Argument");
        
        if (IsSomeoneArrived()) OnMinigameEnd();
    }

    private bool IsSomeoneArrived()
    {
        for (int i = 0; i < clicksArray.Length; i++)
        {
            if (clicksArray[i] >= endValue)
            {
                //Version temporaire du msg de fin
                winnerIndex = i;
                winTMP.text = "j"+(i+1)+" win";
                winTMP.gameObject.SetActive(true);
                //
                return true;
            }
        }

        return false;
    }

    private void WakeUp()
    {
        if(rompicheState.Equals(RompicheState.NULL)) return;
        
        foreach (var animator in cerbereAnimator)
        {
            animator.SetTrigger(WakeUpTrigger);
        }
        foreach (var animator in nuagesAnimator)
        {
            animator.SetTrigger(Depop);
        }
        bulleAnimator.SetTrigger(LaunchBulleDisappear);
    }

    public IEnumerator Observe()
    {
        yield return new WaitForSeconds(Random.Range(cerberWakeUpTimeRangeMin, cerberWakeUpTimeRangeMax + 1));
        foreach (var lasers in laserPlaceHolder)
        {
            lasers.SetActive(false);
        }
        foreach (var animEvent in cerbereAnimEvents)
        {
            animEvent.ObserveEnd();
        }
        foreach (var players in players)
        {
            players.isStunned = false;
        }
        wasHittedByCerbere = new[] {false, false, false, false};
        myCoroutine = null;
        yield return new WaitForSeconds(1);
        
        timeBeforeWake = Random.Range(cerberRompicheRangeMin, cerberRompicheRangeMax + 1);
        timePassedBeforeWake = timeBeforeWake;
        
        LaunchBulleAnim(rompicheState);
    }

    private void LaunchBulleAnim(RompicheState rompicheState)
    {
        bulleAnimator.speed = 3 / timeBeforeWake;
        switch (rompicheState)
        {
            case RompicheState.DEUX:
                this.rompicheState = RompicheState.UN;
                bulleAnimator.SetTrigger(LaunchThirdDisappear);
                break;
            case RompicheState.TROIS:
                this.rompicheState = RompicheState.DEUX;
                bulleAnimator.SetTrigger(LaunchSecondDisappear);
                break;
            case RompicheState.NULL:
                this.rompicheState = RompicheState.TROIS;
                bulleAnimator.SetTrigger(LaunchFirstDisappear);
                break;
        }
    }

    public void ResetBulleAnim()
    {
        bulleAnimator.speed = 1;
        foreach (var animator in nuagesAnimator)
        {
            animator.SetTrigger(Pop);
        }
        bulleAnimator.SetTrigger(Reset);
    }

    public void PlayerWakeUp()
    {
        // if (myCoroutine != null) StopCoroutine(myCoroutine);
        switch (rompicheState)
        {
            case RompicheState.UN:
                timePassedBeforeWake = 0;
                WakeUp();
                rompicheState = RompicheState.NULL;
                break;
            case RompicheState.DEUX:
                timePassedBeforeWake = timeBeforeWake / 3;
                LaunchBulleAnim(rompicheState);
                // myCoroutine = StartCoroutine(ZNumberFeedBack(timeBeforeWake));
                break;
            case RompicheState.TROIS:
                timePassedBeforeWake = timeBeforeWake / 3 * 2;
                LaunchBulleAnim(rompicheState);
                // myCoroutine = StartCoroutine(ZNumberFeedBack(timeBeforeWake));
                break;
            case RompicheState.NULL:
                Debug.Log("Cerbere already wake up");
                break;
        }
    }

    public override void FinishTimer()
    {
        OnMinigameEnd();
    }

    protected override void OnMinigameEnd()
    {
        foreach (PlayerController player in players)
        {
            if(!player.gameObject.activeSelf) continue;
            player.RemoveAllListeners();
        }
        StopAllCoroutines();
        // StartCoroutine(EndMiniGameAnim()); //TODO enlever une fois les anims preparer
        
        ranking = GetRanking();
        AddPoints();
        SetCurrentRanking();
        
        LoadRecap();
        //Gerer la fin du mini jeu
    }

    #region Feedback Methods

    private IEnumerator LaserFeedBack(float laserTime, GameObject laser)
    {
        yield return new WaitForSeconds(laserTime);
        laser.SetActive(false);
    }

    private IEnumerator EndMiniGameAnim()
    {
        //Anim du player gagnant qui touche le cerbere
        yield return new WaitForSeconds(cerberBeginAnimTime);
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].gameObject.activeSelf || i.Equals(winnerIndex)) yield break;
            StartCoroutine(LaserFeedBack(2, laserPlaceHolder[i]));
            //Disparition du player ou retour au debut ?
        }
        yield return new WaitForSeconds(2);
        //Anime du winner qui se fait manger
    }

    #endregion
    
    public enum RompicheState
    {
        ZERO,UN,DEUX,TROIS,NULL
    }
}
