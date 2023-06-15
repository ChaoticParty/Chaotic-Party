using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CerbereManager : SpamManager
{
    [Header("CheapSetup")] //Workaround pour avoir le cerbere éveillé dès le début
    [SerializeField] [Tooltip("Tableau des cerberes, de 0 à 3, correspondant aux players")] private CerbereAnimEvent[] cheapCerbereAnimEvents;
    [SerializeField] [Tooltip("Gameobject cheap du cerbere")] private GameObject cheapCerbere;
    [SerializeField] [Tooltip("Gameobject du cerbere")] private GameObject trueCerbere;
    [Header("Setup")]
    [SerializeField] private CanvasGroup Hud;
    public GameObject[] hudMegaphone;
    [SerializeField] private GameObject nuitObject;
    [SerializeField] [Tooltip("Tableau d'animator, de 0 à 3, correspondant aux players")] private Animator[] cerbereAnimator;
    [SerializeField] [Tooltip("Tableau des cerberes, de 0 à 3, correspondant aux players")] private CerbereAnimEvent[] cerbereAnimEvents;
    [SerializeField] [Tooltip("Animator de la bulle de cerbere")] private Animator bulleAnimator;
    [SerializeField] [Tooltip("Animator des nuages de la bulle de cerbere")] private Animator[] nuagesAnimator;
    [SerializeField] [Tooltip("Animator des z cassé de la bulle de cerbere")] private Animator[] zBreakAnimator;
    private int winnerIndex;
    private bool[] wasHittedByCerbere; //Tableau de bool, true si a été touché. Repasse a false quand Cerbere se rendort. De 0 à 3, correspondant aux players;
    public bool[] hasAlreadyShout; //Tableau de bool, true si a déjà crié. De 0 à 3, correspondant aux players;
    private float[] walkDestination = new float[]{};
    private Coroutine myCoroutine;
    [HideInInspector] public RompicheState rompicheState;
    // private bool isRompiche = true;
    private float timeBeforeWake = 0;
    [SerializeField] private float timePassedBeforeWake = 0;
    private float[] xStartValuePos = new float[4];
    private float[] xEndValuePos = new float[4];
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

    private IEnumerator CheapCerbereAnimLaunch() //Workaround pour avoir le cerbere éveillé dès le début
    {
        yield return new WaitForSeconds(2);
        foreach (var animEvent in cheapCerbereAnimEvents)
        {
            animEvent.CheapObserveEnd();
        }
        yield return new WaitForSeconds(0.33f);
        trueCerbere.SetActive(true);
        cheapCerbere.SetActive(false);
    }

    [ContextMenu("LoadMiniGame")]
    public override void LoadMiniGame()
    {
        StartCoroutine(CheapCerbereAnimLaunch());
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
        hasAlreadyShout = new[] {false, false, false, false};

        for (int i = 0; i < xStartValuePos.Length; i++)
        {
            if (i >= players.Count) continue;
            xStartValuePos[i] = players[i].transform.position.x;
            xEndValuePos[i] = listTeteCerbere[i].transform.position.x - 2; //Le -2 est un padding pour que l'arrivée soit devant cerbere et pas dessus, a changer en fonction
        } 
        
        float distance = xEndValuePos[0] - xStartValuePos[0];
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
                scoreDisplay[i].transform.parent.gameObject.SetActive(true);
                scoreDisplay[i].text = "0";
            }
        }
        
        bulleAnimator.gameObject.SetActive(true);
        
        foreach(GameObject obj in hudMegaphone)
        {
            obj.SetActive(true);
        }

        Hud.alpha = 1;
    }

    private void FixedUpdate()
    {
        if (!isMinigamelaunched) return;
        for (int i = 0; i < walkDestination.Length; i++)
        {
            if(i >= players.Count) continue;
            if (walkDestination[i] > xEndValuePos[i]) walkDestination[i] = xEndValuePos[i];
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
                if ((players[i].GetComponent<CerbereSpamController>().isShout 
                    || Math.Abs(players[i].transform.position.x - walkDestination[i]) > 0.01f) && wasHittedByCerbere[i].Equals(false) 
                    || (players[i].GetComponent<CerbereSpamController>().isUping && wasHittedByCerbere[i].Equals(false)))
                {
                    cerbereSee.Invoke();
                    clicksArray[i] = 0;
                    cerbereAnimator[i].SetBool(UltimoPoderLaser, true);
                    laserPlaceHolder[i].SetActive(true);
                    wasHittedByCerbere[i] = true;
                    StartCoroutine(DelayedPlayerGoBack(i));
                    //Feedback
                    cerbereAnimEvents[i].Exclamation();
                    cerbereLaser.Invoke();
                    players[i].ChangeBulleText("!");
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
        }
        else //Lancement d'une nouvelle boucle de dodo
        {
            WakeUp();
            
            rompicheState = RompicheState.NULL;
        }
    }

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        LaunchBulleAnim(rompicheState);
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
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
                winnerIndex = i;
                return true;
            }
        }

        return false;
    }

    private void WakeUp()
    {
        if(rompicheState.Equals(RompicheState.NULL)) return;
        
        nuitObject.SetActive(true);
        nuitObject.GetComponent<Animator>().SetTrigger("Bigger");
        
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
        foreach (var animator in nuagesAnimator)
        {
            animator.ResetTrigger(Depop);
        }
        foreach (var players in players)
        {
            players.isStunned = false;
        }
        for (int i = 0; i < wasHittedByCerbere.Length; i++)
        {
            if (wasHittedByCerbere[i])
            {
                players[i].Releve();
                players[i].ChangeBulleText("A ou B");
                players[i].EndDegatLaser();
                players[i].ResetReleve();
            }
        }
        wasHittedByCerbere = new[] {false, false, false, false};
        myCoroutine = null;
        
        yield return new WaitForSeconds(0.33f);
        
        timeBeforeWake = Random.Range(cerberRompicheRangeMin, cerberRompicheRangeMax + 1);
        timePassedBeforeWake = timeBeforeWake;

        LaunchBulleAnim(rompicheState);
    }

    private IEnumerator DelayedPlayerGoBack(int index)
    {
        yield return new WaitForSeconds(1f);
        scoreDisplay[index].text = "0";
        players[index].DegatGaucheLaser();
        players[index].PlayHitSound();
        walkDestination[index] = xStartValuePos[index];
        playerGetBackToStart.Invoke(players[index].transform.position, "Argument");
        players[index].transform.position = new Vector3(xStartValuePos[index], players[index].transform.position.y,
            players[index].transform.position.z);
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
        switch (rompicheState)
        {
            case RompicheState.UN:
                zBreakAnimator[2].gameObject.SetActive(true);
                WakeUp();
                timePassedBeforeWake = 0;
                rompicheState = RompicheState.NULL;
                break;
            case RompicheState.DEUX:
                zBreakAnimator[1].gameObject.SetActive(true);
                timePassedBeforeWake = timeBeforeWake / 3;
                LaunchBulleAnim(rompicheState);
                break;
            case RompicheState.TROIS:
                zBreakAnimator[0].gameObject.SetActive(true);
                timePassedBeforeWake = timeBeforeWake / 3 * 2;
                LaunchBulleAnim(rompicheState);
                break;
            case RompicheState.NULL:
                Debug.Log("Cerbere already wake up");
                break;
        }
    }

    public override void FinishTimer()
    {
        float value = -1;
        for (int i = 0; i < clicksArray.Length; i++)
        {
            if (value < clicksArray[i])
            {
                value = clicksArray[i];
                winnerIndex = i;
            }
        }
        OnMinigameEnd();
    }

    protected override void OnMinigameEnd()
    {
        timerManager.isPaused = true;
        isMinigamelaunched = false;
        if (stopMiniGameObject != null) stopMiniGameObject.SetActive(true);
        foreach (PlayerController player in players)
        {
            if(!player.gameObject.activeSelf) continue;
            player.RemoveAllListeners();
        }
        StopAllCoroutines();
        
        ranking = GetRanking();
        AddPoints();
        SetCurrentRanking();
        StartCoroutine(EndMiniGameAnim());
    }

    #region Feedback Methods

    private IEnumerator EndMiniGameAnim()
    {
        int[] tabRank = new int[4];
        foreach (var player in ranking)
        {
            tabRank[player.Value] = player.Key.index;
        }

        foreach (var animator in cerbereAnimator)
        {
            animator.SetTrigger("MiniGameEnd");
        } 
        foreach (var animator in nuagesAnimator)
        {
            animator.ResetTrigger("Depop");
            animator.SetTrigger("MiniGameEnd");
        } 
        bulleAnimator.SetTrigger("MiniGameEnd");

        for (int i = tabRank.Length - 1; i >= 0; i--)
        {
            if (i >= players.Count || i.Equals(winnerIndex)) continue;
            laserPlaceHolder[i].SetActive(true);
            yield return new WaitForSeconds(1);
            players[i].DegatGaucheLaser();
            players[i].PlayHitSound();
            players[i].ChangeBulleText("!");
            players[i].transform.position = new Vector3(xStartValuePos[i], players[i].transform.position.y,
                players[i].transform.position.z);
        }

        players[winnerIndex].Releve();
        players[winnerIndex].VictoryAnimation();
        yield return new WaitForSeconds(4);
        LoadRecap();
    }

    #endregion
    
    public enum RompicheState
    {
        ZERO,UN,DEUX,TROIS,NULL
    }
}
