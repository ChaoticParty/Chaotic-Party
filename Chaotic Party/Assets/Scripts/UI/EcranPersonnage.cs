using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

public class EcranPersonnage : MonoBehaviour
{
    public MenuManager menuManager;
    public GameObject actualPanel;
    public sbyte playSceneIndex = 1;
    [SerializeField] private sbyte playerSOIndex = 0;

    private bool isReady = false;

    [Header("Listes")]
    public List<Races> listRaces = new List<Races>();
    public List<Tetes> listTetes = new List<Tetes>();
    public List<Corps> listCorps = new List<Corps>();
    public List<Color> listColor = new List<Color>();
    [SerializeField] private List<Sprite> listCurrentTete = new List<Sprite>();
    [SerializeField] private List<Sprite> listCurrentCorps = new List<Sprite>();

    [Header("Reférences Custo")] 
    [SerializeField] private GameObject indicNavGoblinGO;
    [SerializeField] private GameObject selectedGoblinGO;
    [SerializeField] private GameObject indicNavDiablotinGO;
    [SerializeField] private GameObject selectedDiablotinGO;
    [SerializeField] private GameObject indicNavPoissonGO;
    [SerializeField] private GameObject selectedPoissonGO;
    [SerializeField] private GameObject indicNavChevalierGO;
    [SerializeField] private GameObject selectedChevalierGO;
    [Space]
    [SerializeField] private Image teteIMG;
    [SerializeField] private GameObject indicNavTeteGO;
    [SerializeField] private Image corpsIMG;
    [SerializeField] private GameObject indicNavCorpsGO;
    [Space]
    [SerializeField] private GameObject indicNavBlancGO;
    [SerializeField] private GameObject selectedBlancGO;
    [SerializeField] private GameObject lockBlancGO;
    [SerializeField] private GameObject indicNavVertGO;
    [SerializeField] private GameObject selectedVertGO;
    [SerializeField] private GameObject lockVertGO;
    [SerializeField] private GameObject indicNavVioletGO;
    [SerializeField] private GameObject selectedVioletGO;
    [SerializeField] private GameObject lockVioletGO;
    [SerializeField] private GameObject indicNavRougeGO;
    [SerializeField] private GameObject selectedRougeGO;
    [SerializeField] private GameObject lockRougeGO;
    [SerializeField] private GameObject indicNavOrangeGO;
    [SerializeField] private GameObject selectedOrangeGO;
    [SerializeField] private GameObject lockOrangeGO;
    [SerializeField] private GameObject indicNavJauneGO;
    [SerializeField] private GameObject selectedJauneGO;
    [SerializeField] private GameObject lockJauneGO;
    [Space]
    [SerializeField] private GameObject indicNavReadyGO;

    [Header("Affichage")] 
    [SerializeField] private Image readyIMG;
    [SerializeField] private Image backIMG;
    private Races currentRace;
    private sbyte currentRaceIndex = 0; //De -128 à 128
    private sbyte currentTeteIndex = 0;
    private sbyte currentCorpsIndex = 0;
    private sbyte currentColorIndex = 0;
    private Custo enumCusto = Custo.RACE;
    private Race enumRace = Race.GOBLIN;
    private ColorEnum enumColor = ColorEnum.BLANC;
    
    [HideInInspector] public PlayerController myPlayerController;


    private void Awake()
    {
        InitCusto();
    }

    private void OnEnable()
    {
        myPlayerController.leftStickJustMovedDown.AddListener(MenuNavigateDown);
        myPlayerController.rightStickMovedDown.AddListener(MenuNavigateDown);
        myPlayerController.dPadDown.AddListener(MenuNavigateDown);
        
        myPlayerController.leftStickJustMovedLeft.AddListener(MenuNavigateLeft);
        myPlayerController.rightStickMovedLeft.AddListener(MenuNavigateLeft);
        myPlayerController.dPadLeft.AddListener(MenuNavigateLeft);
        
        myPlayerController.leftStickJustMovedRight.AddListener(MenuNavigateRight);
        myPlayerController.rightStickMovedRight.AddListener(MenuNavigateRight);
        myPlayerController.dPadRight.AddListener(MenuNavigateRight);
        
        myPlayerController.leftStickJustMovedUp.AddListener(MenuNavigateUp);
        myPlayerController.rightStickMovedUp.AddListener(MenuNavigateUp);
        myPlayerController.dPadUp.AddListener(MenuNavigateUp);
        
        myPlayerController.startPressed.AddListener(LauchGame);
        myPlayerController.aJustPressed.AddListener(ValidateCusto);
        myPlayerController.bLongPressed.AddListener(BackToMain);
    }

    private void MenuNavigateUp()
    {
        MenuNavigateUp(0,0);
    }
    private void MenuNavigateUp(float x, float y)
    {
        if (isReady) return;
        switch (enumCusto)
        {
            case Custo.RACE:
                switch (enumRace)
                {
                    case Race.CHEVALIER:
                        indicNavChevalierGO.SetActive(false);
                        indicNavDiablotinGO.SetActive(true);
                        enumRace = Race.DIABLOTIN;
                        break;
                    case Race.HOMMEPOISSON:
                        indicNavPoissonGO.SetActive(false);
                        indicNavGoblinGO.SetActive(true);
                        enumRace = Race.GOBLIN;
                        break;
                }
                break;
            case Custo.TETE:
                enumCusto = Custo.RACE;
                indicNavTeteGO.SetActive(false);
                SwitchRace();
                switch (enumRace)
                {
                    case Race.GOBLIN:
                        indicNavGoblinGO.SetActive(true);
                        break;
                    case Race.CHEVALIER:
                        indicNavChevalierGO.SetActive(true);
                        break;
                    case Race.DIABLOTIN:
                        indicNavDiablotinGO.SetActive(true);
                        break;
                    case Race.HOMMEPOISSON:
                        indicNavPoissonGO.SetActive(true);
                        break;
                }
                break;
            case Custo.CORPS:
                enumCusto = Custo.TETE;
                indicNavCorpsGO.SetActive(false);
                indicNavTeteGO.SetActive(true);
                break;
            case Custo.COULEUR:
                enumCusto = Custo.CORPS;
                indicNavBlancGO.SetActive(false);
                indicNavVertGO.SetActive(false);
                indicNavVioletGO.SetActive(false);
                indicNavRougeGO.SetActive(false);
                indicNavOrangeGO.SetActive(false);
                indicNavJauneGO.SetActive(false);
                
                indicNavCorpsGO.SetActive(true);
                break;
            case Custo.READY:
                enumCusto = Custo.COULEUR;
                indicNavReadyGO.SetActive(false);
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        indicNavBlancGO.SetActive(true);
                        break;
                    case ColorEnum.VERT:
                        indicNavVertGO.SetActive(true);
                        break;
                    case ColorEnum.VIOLET:
                        indicNavVioletGO.SetActive(true);
                        break;
                    case ColorEnum.ROUGE:
                        indicNavRougeGO.SetActive(true);
                        break;
                    case ColorEnum.ORANGE:
                        indicNavOrangeGO.SetActive(true);
                        break;
                    case ColorEnum.JAUNE:
                        indicNavJauneGO.SetActive(true);
                        break;
                }
                break;
        }
    }
    private void MenuNavigateDown()
    {
        MenuNavigateDown(0,0);
    }
    private void MenuNavigateDown(float x, float y)
    {
        if (isReady) return;
        switch (enumCusto)
        {
            case Custo.RACE:
                if (enumRace is Race.GOBLIN or Race.DIABLOTIN)
                {
                    switch (enumRace)
                    {
                        case Race.DIABLOTIN:
                            indicNavChevalierGO.SetActive(true);
                            indicNavDiablotinGO.SetActive(false);
                            enumRace = Race.CHEVALIER;
                            break;
                        case Race.GOBLIN:
                            indicNavPoissonGO.SetActive(true);
                            indicNavGoblinGO.SetActive(false);
                            enumRace = Race.HOMMEPOISSON;
                            break;
                    }
                }
                else
                {
                    enumCusto = Custo.TETE;
                    indicNavChevalierGO.SetActive(false);
                    indicNavPoissonGO.SetActive(false);
                    indicNavTeteGO.SetActive(true);    
                }
                break;
            case Custo.TETE:
                enumCusto = Custo.CORPS;
                indicNavTeteGO.SetActive(false);
                indicNavCorpsGO.SetActive(true);
                break;
            case Custo.CORPS:
                enumCusto = Custo.COULEUR;
                indicNavCorpsGO.SetActive(false);
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        indicNavBlancGO.SetActive(true);
                        break;
                    case ColorEnum.VERT:
                        indicNavVertGO.SetActive(true);
                        break;
                    case ColorEnum.VIOLET:
                        indicNavVioletGO.SetActive(true);
                        break;
                    case ColorEnum.ROUGE:
                        indicNavRougeGO.SetActive(true);
                        break;
                    case ColorEnum.ORANGE:
                        indicNavOrangeGO.SetActive(true);
                        break;
                    case ColorEnum.JAUNE:
                        indicNavJauneGO.SetActive(true);
                        break;
                }
                break;
            case Custo.COULEUR:
                enumCusto = Custo.READY;
                indicNavReadyGO.SetActive(true);
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        indicNavBlancGO.SetActive(false);
                        break;
                    case ColorEnum.VERT:
                        indicNavVertGO.SetActive(false);
                        break;
                    case ColorEnum.VIOLET:
                        indicNavVioletGO.SetActive(false);
                        break;
                    case ColorEnum.ROUGE:
                        indicNavRougeGO.SetActive(false);
                        break;
                    case ColorEnum.ORANGE:
                        indicNavOrangeGO.SetActive(false);
                        break;
                    case ColorEnum.JAUNE:
                        indicNavJauneGO.SetActive(false);
                        break;
                }
                break;
        }
    }

    private void MenuNavigateRight()
    {
        MenuNavigateRight(0,0);
    }
    private void MenuNavigateRight(float x, float y)
    {
        if (isReady) return;
        switch (enumCusto)
        {
            case Custo.RACE:
                switch (enumRace)
                {
                    case Race.GOBLIN:
                        indicNavGoblinGO.SetActive(false);
                        indicNavDiablotinGO.SetActive(true);
                        enumRace = Race.DIABLOTIN;
                        break;
                    case Race.HOMMEPOISSON:
                        indicNavPoissonGO.SetActive(false);
                        indicNavChevalierGO.SetActive(true);
                        enumRace = Race.CHEVALIER;
                        break;
                }
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
                VisualRefresh();
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
                VisualRefresh();
                break;
            case Custo.COULEUR:
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        indicNavBlancGO.SetActive(false);
                        indicNavVertGO.SetActive(true);
                        enumColor = ColorEnum.VERT;
                        break;
                    case ColorEnum.VERT:
                        indicNavVertGO.SetActive(false);
                        indicNavVioletGO.SetActive(true);
                        enumColor = ColorEnum.VIOLET;
                        break;
                    case ColorEnum.VIOLET:
                        indicNavVioletGO.SetActive(false);
                        indicNavRougeGO.SetActive(true);
                        enumColor = ColorEnum.ROUGE;
                        break;
                    case ColorEnum.ROUGE:
                        indicNavRougeGO.SetActive(false);
                        indicNavOrangeGO.SetActive(true);
                        enumColor = ColorEnum.ORANGE;
                        break;
                    case ColorEnum.ORANGE:
                        indicNavOrangeGO.SetActive(false);
                        indicNavJauneGO.SetActive(true);
                        enumColor = ColorEnum.JAUNE;
                        break;
                }
                break;
        }
    }

    private void MenuNavigateLeft()
    {
        MenuNavigateLeft(0,0);
    }
    private void MenuNavigateLeft(float x, float y)
    {
        if (isReady) return;
        switch (enumCusto)
        {
            case Custo.RACE:
                switch (enumRace)
                {
                    case Race.DIABLOTIN:
                        indicNavDiablotinGO.SetActive(false);
                        indicNavGoblinGO.SetActive(true);
                        enumRace = Race.GOBLIN;
                        break;
                    case Race.CHEVALIER:
                        indicNavChevalierGO.SetActive(false);
                        indicNavPoissonGO.SetActive(true);
                        enumRace = Race.HOMMEPOISSON;
                        break;
                }
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
                VisualRefresh();
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
                VisualRefresh();
                break;
            case Custo.COULEUR:
                switch (enumColor)
                {
                    case ColorEnum.VERT:
                        indicNavVertGO.SetActive(false);
                        indicNavBlancGO.SetActive(true);
                        enumColor = ColorEnum.BLANC;
                        break;
                    case ColorEnum.VIOLET:
                        indicNavVioletGO.SetActive(false);
                        indicNavVertGO.SetActive(true);
                        enumColor = ColorEnum.VERT;
                        break;
                    case ColorEnum.ROUGE:
                        indicNavRougeGO.SetActive(false);
                        indicNavVioletGO.SetActive(true);
                        enumColor = ColorEnum.VIOLET;
                        break;
                    case ColorEnum.ORANGE:
                        indicNavOrangeGO.SetActive(false);
                        indicNavRougeGO.SetActive(true);
                        enumColor = ColorEnum.ROUGE;
                        break;
                    case ColorEnum.JAUNE:
                        indicNavJauneGO.SetActive(false);
                        indicNavOrangeGO.SetActive(true);
                        enumColor = ColorEnum.ORANGE;
                        break;
                }
                break;
        }
    }

    private void VisualRefresh()
    {
        Debug.Log(enumCusto);
        listCurrentTete.Clear();
        listCurrentCorps.Clear();
        
        foreach (var t in listTetes)
        {
            if (String.Equals(t.nomTete, currentRace.nomRace, StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var s in t.listTête)
                {
                    listCurrentTete.Add(s);
                }
            }
        }
        foreach (var c in listCorps)
        {
            if (String.Equals(c.nomCorps, currentRace.nomRace, StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (var s in c.listCorps)
                {
                    listCurrentCorps.Add(s);
                }
            }
        }
        
        teteIMG.sprite = listCurrentTete[currentTeteIndex];
        teteIMG.color = listColor[currentColorIndex];
        corpsIMG.sprite = listCurrentCorps[currentCorpsIndex];
        corpsIMG.color = listColor[currentColorIndex];
    }

    public void InitCusto()
    {
        indicNavGoblinGO.SetActive(true);
        selectedGoblinGO.SetActive(true);
        indicNavTeteGO.SetActive(false);
        indicNavCorpsGO.SetActive(false);
        
        
        currentRace = listRaces[0];
        currentRaceIndex = 0;
        currentTeteIndex = 0;
        currentCorpsIndex = 0;
        currentColorIndex = 0;
        
        SwitchRace();
        VisualRefresh();
    }

    private void SwitchRace()
    {
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

    private void ValidateCusto()
    {
        switch (enumCusto)
        {
            case Custo.RACE:
                selectedGoblinGO.SetActive(false);
                selectedDiablotinGO.SetActive(false);
                selectedChevalierGO.SetActive(false);
                selectedPoissonGO.SetActive(false);
                switch (enumRace)
                {
                    case Race.GOBLIN:
                        currentRaceIndex = 0;
                        selectedGoblinGO.SetActive(true);
                        break;
                    case Race.CHEVALIER:
                        currentRaceIndex = 1;
                        selectedChevalierGO.SetActive(true);
                        break;
                    case Race.DIABLOTIN:
                        currentRaceIndex = 2;
                        selectedDiablotinGO.SetActive(true);
                        break;
                    case Race.HOMMEPOISSON:
                        currentRaceIndex = 3;
                        selectedPoissonGO.SetActive(true);
                        break;
                }
                currentRace = listRaces[currentRaceIndex];
                break;
            case Custo.COULEUR:
                selectedBlancGO.SetActive(false);
                selectedVertGO.SetActive(false);
                selectedVioletGO.SetActive(false);
                selectedRougeGO.SetActive(false);
                selectedOrangeGO.SetActive(false);
                selectedJauneGO.SetActive(false);
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        if (!lockBlancGO.activeSelf)
                        {
                            currentColorIndex = 0;
                            selectedBlancGO.SetActive(true);
                        }
                        break;
                    case ColorEnum.VERT:
                        if (!lockVertGO.activeSelf)
                        {
                            currentColorIndex = 1;
                            selectedVertGO.SetActive(true);
                        }
                        break;
                    case ColorEnum.VIOLET:
                        if (!lockVioletGO.activeSelf)
                        {
                            currentColorIndex = 2;
                            selectedVioletGO.SetActive(true);
                        }
                        break;
                    case ColorEnum.ROUGE:
                        if (!lockRougeGO.activeSelf)
                        {
                            currentColorIndex = 3;
                            selectedRougeGO.SetActive(true);
                        }
                        break;
                    case ColorEnum.ORANGE:
                        if (!lockOrangeGO.activeSelf)
                        {
                            currentColorIndex = 4;
                            selectedOrangeGO.SetActive(true);
                        }
                        break;
                    case ColorEnum.JAUNE:
                        if (!lockJauneGO.activeSelf)
                        {
                            currentColorIndex = 5;
                            selectedJauneGO.SetActive(true);
                        }
                        break;
                }
                break;
            case Custo.READY:
                Ready();
                break;
        }
        VisualRefresh();
    }

    private void BackToMain(float t)
    {
        //Anim du retour qui se complète
        backIMG.color = Color.red;
        if (t > 2)
        {
            menuManager.Back(actualPanel);
            EventSystem.current.SetSelectedGameObject(menuManager.partyBTN.gameObject);
        }
        //Ajout d'un panel de confirmation ou tt le monde y a acces ?
    }

    public void FillSO()
    {
        menuManager.playersListSO.players[playerSOIndex].head = listTetes[currentRaceIndex].listTête[currentTeteIndex];
        menuManager.playersListSO.players[playerSOIndex].body = listCorps[currentRaceIndex].listCorps[currentCorpsIndex];
        menuManager.playersListSO.players[playerSOIndex].color = listColor[currentColorIndex];
    }

    public void LockColor(bool _isReady)
    {
        if (!_isReady)
            menuManager.selectColor.Add(enumColor);
        else
            menuManager.selectColor.Remove(enumColor);
        
        switch (currentColorIndex)
        {
            case 0:
                lockBlancGO.SetActive(!_isReady);
                break;
            case 1:
                lockVertGO.SetActive(!_isReady);
                break;
            case 2:
                lockVioletGO.SetActive(!_isReady);
                break;
            case 3:
                lockRougeGO.SetActive(!_isReady);
                break;
            case 4:
                lockOrangeGO.SetActive(!_isReady);
                break;
            case 5:
                lockJauneGO.SetActive(!_isReady);
                break;
        }
    }
    
    private void Ready()
    {
        if (!isReady)
        {
            //Anim du parchemin qui se ferme et remonte + possibilité au joueur de jouer avec son perso
            menuManager.readyCount++;
            readyIMG.color = Color.red;
            LockColor(isReady);
            //Faire le check aussi
        }
        else
        {
            //Anim du parchemin qui s'ouvre et redscent + peut plus joeur avec son perso
            menuManager.readyCount--;
            readyIMG.color = Color.white;
            LockColor(isReady);
        }

        isReady = !isReady;
        VisualRefresh();
        if (IsLaunchPossible())
        {
            //Apparition bandeau a la smash (placeholder a voir si on garde)
            // foreach (EcranPersonnage ecranPerso in menuManager.listPersonnages)
            // {
            //     if (ecranPerso.gameObject.activeSelf)
            //     {
            //         myPlayerController.startPressed.AddListener(LauchGame);   
            //     }
            // }
            menuManager.partyBandeauReadyGO.SetActive(true);
        }
        else
        {
            //Disparition bandeau
            // myPlayerController.startPressed.RemoveAllListeners();
            menuManager.partyBandeauReadyGO.SetActive(false);
        }
    }

    private bool IsLaunchPossible()
    {
        sbyte playerCountTemp = 0;
        foreach (EcranPersonnage ecranPersonnage in menuManager.listPersonnages)
        {
            if (ecranPersonnage.gameObject.activeSelf)
            {
                playerCountTemp++;
            }
        }
        return menuManager.readyCount.Equals(playerCountTemp) /*&& playerCountTemp > 1*/;
    }

    private void LauchGame()
    {
        if (IsLaunchPossible())
        {
            foreach (EcranPersonnage ecranPerso in menuManager.listPersonnages)
            {
                if (ecranPerso.gameObject.activeSelf)
                {
                    ecranPerso.FillSO();    
                }
            }
            SceneManager.LoadScene(playSceneIndex);
        }
    }
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
    RACE,TETE,CORPS,COULEUR,READY
}
public enum Race
{
    GOBLIN,CHEVALIER,DIABLOTIN,HOMMEPOISSON
}

public enum ColorEnum
{
    BLANC,VERT,VIOLET,ROUGE,ORANGE,JAUNE
}
