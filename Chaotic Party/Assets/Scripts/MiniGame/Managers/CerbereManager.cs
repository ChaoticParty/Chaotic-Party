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
    [SerializeField] private ParticleSystem rompicheEffect;
    private bool isMinigamelaunch = false;
    private bool[] wasHittedByCerbere; //Tableau de bool, true si a été touché. Repasse a false quand Cerbere se rendort. De 0 à 3, correspondant aux players;
    private float[] walkDestination = new float[]{};
    [SerializeField] private float timeBeforeWake = 0;
    [SerializeField] private float timePassedBeforeWake = 0;
    [SerializeField] private float xStartValuePos = 0;
    [SerializeField] private float xEndValuePos = 0;
    [SerializeField] private float inGameValuePerClick = 0;
    [SerializeField] private bool isRompiche = true;
    [Header("Distance avec Cerbere")]
    [SerializeField] [Tooltip("Valeur indiquant la distance entre les joueurs et cerbere")] private int endValue = 100;
    [Header("Rompiche")]
    [SerializeField] [Range(0,120)] [Tooltip("Valeur basse du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberRompicheRangeMin;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur haute du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberRompicheRangeMax;
    [Header("Réveil")]
    [SerializeField] [Range(0,120)] [Tooltip("Valeur basse du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberWakeUpTimeRangeMin;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur haute du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberWakeUpTimeRangeMax;
    [Space] 
    [Header("Score UI")] 
    [SerializeField] private List<TextMeshProUGUI> scoreDisplay = new List<TextMeshProUGUI>(); //Les 4 ui de scores
    [Space] 
    [Header("Cerbere UI")] 
    [SerializeField] private List<SpriteRenderer> listTeteCerbere = new List<SpriteRenderer>(); //Les 4 tetes
    
    #region Events

    [Space, Header("Events")] //A placer dans le code
    public UnityEvent<Vector2, string> playerWalk;
    public UnityEvent<Vector2, string> playerGetBackToStart;
    public UnityEvent<Vector2, string> playerYell;
    public UnityEvent cerbereLaser;

    #endregion

    private new void Start()
    {
        base.Start();
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
        float valueForOneScore = endValue / spamValue;
        inGameValuePerClick = distance / valueForOneScore;
        Debug.Log("Babass "+inGameValuePerClick + "Distance : "+distance);
        
        walkDestination = new float[players.Count];
        for (int i = 0; i < walkDestination.Length; i++)
        {
            if(i >= players.Count) continue;
            walkDestination[i] = players[i].transform.position.x;
        }

        foreach (TextMeshProUGUI text in scoreDisplay)
        {
            text.text = "0";
        }
        
        StartCoroutine(ZNumberFeedBack(timeBeforeWake));
        isMinigamelaunch = true;
    }

    private void Update()
    {
        if (!isMinigamelaunch) return;
        for (int i = 0; i < walkDestination.Length; i++)
        {
            if(i >= players.Count) continue;
            if (walkDestination[i] > xEndValuePos) walkDestination[i] = xEndValuePos;
            // Debug.Log("Index : "+i+" / Actual position : "+players[i].transform.position.x+" / Destination : "+walkDestination[i]);
            if (players[i].transform.position.x >= walkDestination[i]) continue;
            
            players[i].transform.position = new Vector3(Mathf.Lerp(players[i].transform.position.x, walkDestination[i], Time.deltaTime * 10),
                players[i].transform.position.y, players[i].transform.position.z);
            
            if (walkDestination[i] - players[i].transform.position.x <= 0.01f)
                players[i].transform.position = new Vector3(walkDestination[i], players[i].transform.position.y, players[i].transform.position.z);
            // Debug.Log("Update position : "+players[i].transform.position);
        }
        if (!isRompiche)
        {
            Debug.Log("Rompiche plus");
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsStandingStill() && wasHittedByCerbere[i].Equals(false))
                {
                    wasHittedByCerbere[i] = true;
                    players[i].transform.position = new Vector3(xStartValuePos, players[i].transform.position.y,
                        players[i].transform.position.z);
                    //Se fait toucher, donc animation + changements de variables
                }    
            }
            return;
        }
        if (timePassedBeforeWake >= 0) //Décrémentation temps avant reveille Cerbere
        {
            timePassedBeforeWake -= Time.deltaTime;
            //Animation (les z du dodo qui decrementent ?)
        }
        else //Lancement d'une nouvelle boucle de dodo
        {
            isRompiche = false;
            StartCoroutine(WakeUp());
        }
    }
    
    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        //playerIndex de 0 à 3, player.index en gros
        Debug.Log("CLICk"); //BUG Si on descactive les input avec le stun et qu'on r'appui, tt les inputs sont compté et nous "tp" a la fin, Envoyer une action dans le stun ?
        walkDestination[playerIndex] += inGameValuePerClick;
        clicksArray[playerIndex] += Mathf.RoundToInt(inGameValuePerClick);
        scoreDisplay[playerIndex].text = clicksArray[playerIndex].ToString();
        // Debug.Log(clicksArray[playerIndex]);
        // Debug.Log(players[playerIndex].transform.position.x);
        // Debug.Log(inGameValuePerClick);
        // Debug.Log(walkDestination[playerIndex]);
        DisplayCrown();
        
        if (IsSomeoneArrived()) OnMinigameEnd();
    }

    private bool IsSomeoneArrived()
    {
        foreach (float score in clicksArray)
        {
            if (score >= endValue)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator WakeUp()
    {
        foreach (var spriteRenderer in listTeteCerbere)
        {
            spriteRenderer.color = Color.red;
        }
        yield return new WaitForSeconds(Random.Range(cerberWakeUpTimeRangeMin, cerberWakeUpTimeRangeMax + 1));
        wasHittedByCerbere = new[] {false, false, false, false};
        isRompiche = true;
        timeBeforeWake = Random.Range(cerberRompicheRangeMin, cerberRompicheRangeMax + 1);
        timePassedBeforeWake = timeBeforeWake;
        StartCoroutine(ZNumberFeedBack(timeBeforeWake));
    }

    private IEnumerator ZNumberFeedBack(float timeBefWake)
    {
        var rompicheEffectMain = rompicheEffect.main;

        //TODO:En commentaire le temps d'avoir ce particle system
        
        if (timePassedBeforeWake > timeBefWake / 3 * 2)
        {
            foreach (var spriteRenderer in listTeteCerbere)
            {
                spriteRenderer.color = Color.blue;
            }
            // rompicheEffectMain.maxParticles = 3;
            //Rajouter de potentiels fx de real
        }
        else if (timePassedBeforeWake > timeBefWake / 3)
        {
            foreach (var spriteRenderer in listTeteCerbere)
            {
                spriteRenderer.color = Color.green;
            }
            // rompicheEffectMain.maxParticles = 2;
        }
        else
        {
            foreach (var spriteRenderer in listTeteCerbere)
            {
                spriteRenderer.color = Color.yellow;
            }
            // rompicheEffectMain.maxParticles = 1;
        }

        yield return new WaitForSeconds(timeBefWake / 3);

        if(isRompiche) StartCoroutine(ZNumberFeedBack(timeBefWake));
    }

    protected override void OnMinigameEnd()
    {
        foreach (PlayerController player in players)
        {
            if(!player.gameObject.activeSelf) continue;
            player.RemoveAllListeners();
        }

        isMinigamelaunch = true;
        //Gerer la fin du mini jeu
    }
}
