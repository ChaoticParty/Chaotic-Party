using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CerbereManager : SpamManager
{
    [SerializeField] private ParticleSystem rompicheEffect;
    private bool isMinigamelaunch = false;
    private bool[] wasHittedByCerbere; //Tableau de bool, true si a été touché. Repasse a false quand Cerbere se rendort. De 0 à 3, correspondant aux players;
    [SerializeField] private int endValue = 0;
    [SerializeField] private float timeBeforeWake = 0;
    [SerializeField] private float timePassedBeforeWake = 0;
    [SerializeField] private bool isRompiche = false;
    [Header("Rompiche")]
    [SerializeField] [Range(0,120)] [Tooltip("Valeur basse du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberRompicheRangeMin;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur haute du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberRompicheRangeMax;
    [Header("Réveil")]
    [SerializeField] [Range(0,120)] [Tooltip("Valeur basse du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberWakeUpTimeRangeMin;
    [SerializeField] [Range(0,120)] [Tooltip("Valeur haute du random définissant l'intervalle de temps entrenles sommeils du cerbere")] private int cerberWakeUpTimeRangeMax;

    private new void Start()
    {
        base.Start();
        if (cerberRompicheRangeMin > cerberRompicheRangeMax)
        {
            Debug.Log("Valeur minimal supérieur à la maximale, inversion des champs pour pouvoir lancer le minijeu");
            (cerberRompicheRangeMax, cerberRompicheRangeMin) = (cerberRompicheRangeMin, cerberRompicheRangeMax);
        }
        wasHittedByCerbere = new[] {false, false, false, false};
        //Mettre a true isRompiche
    }
    
    private void Update()
    {
        if (!isMinigamelaunch) return;
        if (!isRompiche)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsStandingStill() && wasHittedByCerbere[i].Equals(false))
                {
                    wasHittedByCerbere[i] = true;
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
        clicksArray[playerIndex] += value;
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
        yield return new WaitForSeconds(Random.Range(cerberWakeUpTimeRangeMin, cerberWakeUpTimeRangeMax + 1));
        wasHittedByCerbere = new[] {false, false, false, false};
        isRompiche = true;
        timeBeforeWake = Random.Range(cerberRompicheRangeMin, cerberRompicheRangeMax + 1);
        StartCoroutine(zNumberFeedBack(timeBeforeWake));
    }

    private IEnumerator zNumberFeedBack(float timeBefWake)
    {
        var rompicheEffectMain = rompicheEffect.main;

        if (timePassedBeforeWake > timeBefWake / 3 * 2)
        {
            rompicheEffectMain.maxParticles = 3;
            //Rajouter de potentiels fx de real
        }
        else if (timePassedBeforeWake > timeBefWake / 3)
        {
            rompicheEffectMain.maxParticles = 2;
        }
        else
        {
            rompicheEffectMain.maxParticles = 1;
        }

        yield return new WaitForSeconds(timeBefWake / 3);

        StartCoroutine(zNumberFeedBack(timeBefWake));
    }

    protected override void OnMinigameEnd()
    {
        throw new System.NotImplementedException();
    }
}
