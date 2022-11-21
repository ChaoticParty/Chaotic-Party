using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EcranPersonnage : MonoBehaviour
{
    public MenuManager menuManager;
    public sbyte playSceneIndex = 1;

    private bool isReady = false;

    [Header("Listes")]
    public List<Races> listRaces = new List<Races>();
    public List<Tetes> listTetes = new List<Tetes>();
    public List<Corps> listCorps = new List<Corps>();
    public List<Color> listColor = new List<Color>();
    [SerializeField] private List<Sprite> listCurrentTete = new List<Sprite>();
    [SerializeField] private List<Sprite> listCurrentCorps = new List<Sprite>();

    [Header("Reférences Custo")]
    public Image raceIMG;
    public GameObject indicNavRaceGO;
    public Image teteIMG;
    public GameObject indicNavTeteGO;
    public Image corpsIMG;
    public GameObject indicNavCorpsGO;
    public Image colorIMG;
    public GameObject indicNavColorGO;
    
    [Header("Affichage")]
    private Races currentRace;
    public Image currentTeteIMG;
    public Image currentCorpsIMG;
    private sbyte currentRaceIndex = 0; //De -128 à 128
    private sbyte currentTeteIndex = 0;
    private sbyte currentCorpsIndex = 0;
    private sbyte currentColorIndex = 0;
    private Custo enumCusto = Custo.RACE;
    private Race enumRace = Race.GOBLIN;
    
    public PlayerController myPlayerController;


    private void Awake()
    {
        myPlayerController ??= GetComponent<PlayerController>();
        InitCusto();
    }

    private void OnEnable()
    {
        myPlayerController.leftStickJustMovedDown.AddListener(MenuNavigateDown);
        myPlayerController.leftStickJustMovedLeft.AddListener(MenuNavigateLeft);
        myPlayerController.leftStickJustMovedRight.AddListener(MenuNavigateRight);
        myPlayerController.leftStickJustMovedUp.AddListener(MenuNavigateUp);
        myPlayerController.startPressed.AddListener(Ready);
    }

    private void MenuNavigateUp(float x, float y)
    {
        switch (enumCusto)
        {
            case Custo.TETE:
                enumCusto = Custo.RACE;
                indicNavTeteGO.SetActive(false);
                indicNavRaceGO.SetActive(true);
                break;
            case Custo.CORPS:
                enumCusto = Custo.TETE;
                indicNavCorpsGO.SetActive(false);
                indicNavTeteGO.SetActive(true);
                break;
            case Custo.COULEUR:
                enumCusto = Custo.CORPS;
                indicNavColorGO.SetActive(false);
                indicNavCorpsGO.SetActive(true);
                break;
        }
        Debug.Log(enumCusto);
        Refresh();
    }
    private void MenuNavigateDown(float x, float y)
    {
        switch (enumCusto)
        {
            case Custo.RACE:
                enumCusto = Custo.TETE;
                indicNavRaceGO.SetActive(false);
                indicNavTeteGO.SetActive(true);
                break;
            case Custo.TETE:
                enumCusto = Custo.CORPS;
                indicNavTeteGO.SetActive(false);
                indicNavCorpsGO.SetActive(true);
                break;
            case Custo.CORPS:
                enumCusto = Custo.COULEUR;
                indicNavCorpsGO.SetActive(false);
                indicNavColorGO.SetActive(true);
                break;
        }
        Refresh();
    }
    private void MenuNavigateRight(float x, float y)
    {
        switch (enumCusto)
        {
            case Custo.RACE:
                if (currentRaceIndex == listRaces.Count - 1)
                {
                    currentRaceIndex = 0;
                    
                }
                else
                {
                    currentRaceIndex ++;
                }
                SwitchRace();
                break;
            case Custo.TETE:
                if (currentTeteIndex == listCurrentTete.Count - 1)
                {
                    currentTeteIndex = 0;
                    
                }
                else
                {
                    currentTeteIndex ++;
                }
                break;
            case Custo.CORPS:
                if (currentCorpsIndex == listCurrentCorps.Count - 1)
                {
                    currentCorpsIndex = 0;
                    
                }
                else
                {
                    currentCorpsIndex ++;
                }
                break;
            case Custo.COULEUR:
                if (currentColorIndex == listColor.Count - 1)
                {
                    currentColorIndex = 0;
                    
                }
                else
                {
                    currentColorIndex ++;
                }
                break;
        }
        Refresh();
    }
    private void MenuNavigateLeft(float x, float y)
    {
        switch (enumCusto)
        {
            case Custo.RACE:
                if (currentRaceIndex == 0)
                {
                    currentRaceIndex = Convert.ToSByte(listRaces.Count - 1);
                }
                else
                {
                    currentRaceIndex --;
                }
                SwitchRace();
                break;
            case Custo.TETE:
                if (currentTeteIndex == 0)
                {
                    currentTeteIndex = Convert.ToSByte(listCurrentTete.Count - 1);
                    
                }
                else
                {
                    currentTeteIndex --;
                }
                break;
            case Custo.CORPS:
                if (currentCorpsIndex == 0)
                {
                    currentCorpsIndex = Convert.ToSByte(listCurrentCorps.Count - 1);
                    
                }
                else
                {
                    currentCorpsIndex --;
                }
                break;
            case Custo.COULEUR:
                if (currentColorIndex == 0)
                {
                    currentColorIndex = Convert.ToSByte(listColor.Count - 1);
                    
                }
                else
                {
                    currentColorIndex --;
                }
                break;
        }
        Refresh();
    }

    private void Refresh()
    {
        listCurrentTete.Clear();
        listCurrentCorps.Clear();
        
        foreach (var r in listRaces)
        {
            if (String.Equals(r.nomRace, enumRace.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                currentRace = r;
            }
        }
        foreach (var t in listTetes)
        {
            if (String.Equals(t.nomTete, enumRace.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var s in t.listTête)
                {
                    listCurrentTete.Add(s);
                }
            }
        }
        foreach (var c in listCorps)
        {
            if (String.Equals(c.nomCorps, enumRace.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var s in c.listCorps)
                {
                    listCurrentCorps.Add(s);
                }
            }
        }

        raceIMG.sprite = currentRace.spriteRace;
        teteIMG.sprite = listCurrentTete[currentTeteIndex];
        currentTeteIMG.sprite = listCurrentTete[currentTeteIndex];
        currentTeteIMG.color = listColor[currentColorIndex];
        corpsIMG.sprite = listCurrentCorps[currentCorpsIndex];
        currentCorpsIMG.sprite = listCurrentCorps[currentCorpsIndex];
        currentCorpsIMG.color = listColor[currentColorIndex];
        colorIMG.color = listColor[currentColorIndex];
        
        // menuManager.selectCorps.Add(listCurrentCorps[currentCorpsIndex]); //A deplacer quand le joueur fera start pour valider
                                                                            //La validation a save dans menumanager en fonction du nb de gens ayant fait start et du nb de joueur actuel
                                                                            //S'il est deja pret, start l'enleve. le j1 devras lancer avec start quand tt le monde sera pret
        // menuManager.selectTete.Add(listCurrentTete[currentTeteIndex]);
    }

    public void InitCusto()
    {
        enumCusto = Custo.RACE;
        
        indicNavTeteGO.SetActive(false);
        indicNavCorpsGO.SetActive(false);
        indicNavColorGO.SetActive(false);
        indicNavRaceGO.SetActive(true);
        
        currentRace = listRaces[0];
        currentRaceIndex = 0;
        currentColorIndex = 0;
        SwitchRace();
        Refresh();
    }

    private void SwitchRace()
    {
        raceIMG.sprite = listRaces[currentRaceIndex].spriteRace;
        switch (listRaces[currentRaceIndex].nomRace)
        {
            case "Goblin" :
                enumRace = Race.GOBLIN;
                break;
            case "Chevalier" :
                enumRace = Race.CHEVALIER;
                break;
            case "Diablotin" :
                enumRace = Race.DIABLOTIN;
                break;
            case "HommePoisson" :
                enumRace = Race.HOMMEPOISSON;
                break;
        }
        currentTeteIndex = 0;
        currentCorpsIndex = 0;
    }

    private void Ready()
    {
        if (menuManager.readyCount.Equals(1/*compte du nb de joueurs*/) /*&& j1*/) //Que si j1 et que nb joueur egal readycount
        {
            SceneManager.LoadScene(playSceneIndex);
        }
        if (isReady)
        {
            //Anim du parchemin qui se ferme et remonte + possibilité au joueur de jouer avec son perso
            menuManager.selectTete.Add(listCurrentTete[currentTeteIndex]);
            menuManager.selectCorps.Add(listCurrentCorps[currentCorpsIndex]);
            menuManager.readyCount++;
            //Ajout et attribution des sprites tete, corps et couleur au player concerné (cf scriptable object)
            //Faire le check aussi
            Debug.Log("Ajout des tete et corps");
        }
        else
        {
            //Anim du parchemin qui s'ouvre et redscent + peut plus joeur avec son perso
            menuManager.selectTete.Remove(listCurrentTete[currentTeteIndex]);
            menuManager.selectCorps.Remove(listCurrentCorps[currentCorpsIndex]);
            menuManager.readyCount--;
            Debug.Log("suppresssion des tete et corps");
        }

        isReady = !isReady;
        Refresh();
    }

    //pas 2 fois la meme tete ou corps mais meme race possible
}

[Serializable]
public struct Races
{
    public Sprite spriteRace;
    public string nomRace;
}
[Serializable]
public struct Tetes
{
    public List<Sprite> listTête;
    public string nomTete;
}
[Serializable]
public struct Corps
{
    public List<Sprite> listCorps;
    public string nomCorps;
}

public enum Custo
{
    RACE,TETE,CORPS,COULEUR
}
public enum Race
{
    GOBLIN,CHEVALIER,DIABLOTIN,HOMMEPOISSON
}
