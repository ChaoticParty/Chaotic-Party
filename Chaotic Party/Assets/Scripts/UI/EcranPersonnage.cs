using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<Sprite> listSpriteRace = new List<Sprite>(); //Dernier = selectionner
    [SerializeField] private List<Image> listImageRace = new List<Image>();
    [SerializeField] private List<GameObject> listIndicNavRace = new List<GameObject>();
    [Space]
    [SerializeField] private Image teteIMG;
    [SerializeField] private List<GameObject> listIndicNavTeteGO = new List<GameObject>();
    [SerializeField] private Image corpsIMG;
    [SerializeField] private List<GameObject> listIndicNavCorpsGO = new List<GameObject>();
    [Space]
    [SerializeField] private List<Image> listIndicNavColor = new List<Image>();
    [SerializeField] private List<Sprite> listBaseColorSpt = new List<Sprite>();
    [SerializeField] private List<Sprite> listSelectedColorSpt = new List<Sprite>();
    [SerializeField] private GameObject colorLayout;
    [SerializeField] private Image bigColor;
    [SerializeField] private List<Sprite> listBigColorSpt;
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
    //Id des animations
    private static readonly int Burning = Animator.StringToHash("Burning");
    private static readonly int BackBurning = Animator.StringToHash("BackBurning");


    private void Awake()
    {
        InitCusto();
    }

    private void OnEnable()
    {
        InitCusto();
        
        AddAllListeners();
    }

    public void AddAllListeners()
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
    public void RemoveAllListeners()
    {
        myPlayerController.leftStickJustMovedDown.RemoveListener(MenuNavigateDown);
        myPlayerController.rightStickMovedDown.RemoveListener(MenuNavigateDown);
        myPlayerController.dPadDown.RemoveListener(MenuNavigateDown);
        
        myPlayerController.leftStickJustMovedLeft.RemoveListener(MenuNavigateLeft);
        myPlayerController.rightStickMovedLeft.RemoveListener(MenuNavigateLeft);
        myPlayerController.dPadLeft.RemoveListener(MenuNavigateLeft);
        
        myPlayerController.leftStickJustMovedRight.RemoveListener(MenuNavigateRight);
        myPlayerController.rightStickMovedRight.RemoveListener(MenuNavigateRight);
        myPlayerController.dPadRight.RemoveListener(MenuNavigateRight);
        
        myPlayerController.leftStickJustMovedUp.RemoveListener(MenuNavigateUp);
        myPlayerController.rightStickMovedUp.RemoveListener(MenuNavigateUp);
        myPlayerController.dPadUp.RemoveListener(MenuNavigateUp);
        
        myPlayerController.startPressed.RemoveListener(LauchGame);
        myPlayerController.aJustPressed.RemoveListener(ValidateCusto);
        myPlayerController.bLongPressed.RemoveListener(BackToMain);
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
                        listIndicNavRace[1].transform.rotation = new Quaternion(0,0,0,0);
                        listIndicNavRace[2].transform.rotation = Quaternion.Euler(0,0,-10);
                        enumRace = Race.DIABLOTIN;
                        break;
                    case Race.HOMMEPOISSON:
                        listIndicNavRace[3].transform.rotation = new Quaternion(0,0,0,0);
                        listIndicNavRace[0].transform.rotation = Quaternion.Euler(0,0,10);
                        enumRace = Race.GOBLIN;
                        break;
                }
                break;
            case Custo.TETE:
                enumCusto = Custo.RACE;
                foreach (var item in listIndicNavTeteGO)
                {
                    item.transform.rotation = new Quaternion(0,0,0,0);
                }
                SwitchRace();
                switch (enumRace)
                {
                    case Race.GOBLIN:
                        listIndicNavRace[0].transform.rotation = Quaternion.Euler(0,0,10);
                        break;
                    case Race.CHEVALIER:
                        listIndicNavRace[1].transform.rotation = Quaternion.Euler(0,0,-10);
                        break;
                    case Race.DIABLOTIN:
                        listIndicNavRace[2].transform.rotation = Quaternion.Euler(0,0,-10);
                        break;
                    case Race.HOMMEPOISSON:
                        listIndicNavRace[3].transform.rotation = Quaternion.Euler(0,0,10);
                        break;
                }
                break;
            case Custo.CORPS:
                enumCusto = Custo.TETE;
                foreach (var item in listIndicNavCorpsGO)
                {
                    item.transform.rotation = new Quaternion(0,0,0,0);
                }
                foreach (var item in listIndicNavTeteGO)
                {
                    item.transform.rotation = Quaternion.Euler(0,0,10 * item.transform.localScale.x);
                }
                break;
            case Custo.COULEUR:
                enumCusto = Custo.CORPS;
                foreach (var objects in listIndicNavColor)
                {
                    objects.sprite = listBaseColorSpt[listIndicNavColor.IndexOf(objects)];
                }
                
                foreach (var item in listIndicNavCorpsGO)
                {
                    item.transform.rotation = Quaternion.Euler(0,0,10 * item.transform.localScale.x);
                }
                
                colorLayout.SetActive(false);
                bigColor.enabled = true;
                break;
            case Custo.READY:
                enumCusto = Custo.COULEUR;
                indicNavReadyGO.SetActive(false);
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        listIndicNavColor[0].sprite = listSelectedColorSpt[0];
                        break;
                    case ColorEnum.VERT:
                        listIndicNavColor[1].sprite = listSelectedColorSpt[1];
                        break;
                    case ColorEnum.VIOLET:
                        listIndicNavColor[2].sprite = listSelectedColorSpt[2];
                        break;
                    case ColorEnum.ROUGE:
                        listIndicNavColor[3].sprite = listSelectedColorSpt[3];
                        break;
                    case ColorEnum.ORANGE:
                        listIndicNavColor[4].sprite = listSelectedColorSpt[4];
                        break;
                    case ColorEnum.JAUNE:
                        listIndicNavColor[5].sprite = listSelectedColorSpt[5];
                        break;
                }
                
                colorLayout.SetActive(true);
                bigColor.enabled = false;
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
                            listIndicNavRace[1].transform.rotation = Quaternion.Euler(0,0,-10);
                            listIndicNavRace[2].transform.rotation = new Quaternion(0, 0, 0, 0);
                            enumRace = Race.CHEVALIER;
                            break;
                        case Race.GOBLIN:
                            listIndicNavRace[3].transform.rotation = Quaternion.Euler(0,0,10);
                            listIndicNavRace[0].transform.rotation = new Quaternion(0,0,0,0);
                            enumRace = Race.HOMMEPOISSON;
                            break;
                    }
                }
                else
                {
                    enumCusto = Custo.TETE;
                    listIndicNavRace[1].transform.rotation = new Quaternion(0,0,0,0);
                    listIndicNavRace[3].transform.rotation = new Quaternion(0,0,0,0);
                    foreach (var item in listIndicNavTeteGO)
                    {
                        item.transform.rotation = Quaternion.Euler(0,0,10 * item.transform.localScale.x);
                    }  
                }
                break;
            case Custo.TETE:
                enumCusto = Custo.CORPS;
                foreach (var item in listIndicNavTeteGO)
                {
                    item.transform.rotation = new Quaternion(0,0,0,0);
                }
                foreach (var item in listIndicNavCorpsGO)
                {
                    item.transform.rotation = Quaternion.Euler(0,0,10 * item.transform.localScale.x);
                }
                break;
            case Custo.CORPS:
                enumCusto = Custo.COULEUR;
                foreach (var item in listIndicNavCorpsGO)
                {
                    item.transform.rotation = new Quaternion(0,0,0,0);
                }
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        listIndicNavColor[0].sprite = listSelectedColorSpt[0];
                        break;
                    case ColorEnum.VERT:
                        listIndicNavColor[1].sprite = listSelectedColorSpt[1];
                        break;
                    case ColorEnum.VIOLET:
                        listIndicNavColor[2].sprite = listSelectedColorSpt[2];
                        break;
                    case ColorEnum.ROUGE:
                        listIndicNavColor[3].sprite = listSelectedColorSpt[3];
                        break;
                    case ColorEnum.ORANGE:
                        listIndicNavColor[4].sprite = listSelectedColorSpt[4];
                        break;
                    case ColorEnum.JAUNE:
                        listIndicNavColor[5].sprite = listSelectedColorSpt[5];
                        break;
                }
                
                colorLayout.SetActive(true);
                bigColor.enabled = false;
                break;
            case Custo.COULEUR:
                enumCusto = Custo.READY;
                indicNavReadyGO.SetActive(true);
                foreach (var objects in listIndicNavColor)
                {
                    objects.sprite = listBaseColorSpt[listIndicNavColor.IndexOf(objects)];
                }
                
                colorLayout.SetActive(false);
                bigColor.enabled = true;
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
                        listIndicNavRace[0].transform.rotation = new Quaternion(0,0,0,0);
                        listIndicNavRace[2].transform.rotation = Quaternion.Euler(0,0,-10);
                        enumRace = Race.DIABLOTIN;
                        break;
                    case Race.HOMMEPOISSON:
                        listIndicNavRace[3].transform.rotation = new Quaternion(0,0,0,0);
                        listIndicNavRace[1].transform.rotation = Quaternion.Euler(0,0,-10);
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

                foreach (var item in listIndicNavTeteGO.Where(item => item.transform.rotation.x.Equals(1)))
                {
                    StartCoroutine(HandAnim(item));
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
                
                foreach (var item in listIndicNavCorpsGO.Where(item => item.transform.rotation.x.Equals(1)))
                {
                    StartCoroutine(HandAnim(item));
                }
                VisualRefresh();
                break;
            case Custo.COULEUR:
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                        listIndicNavColor[0].sprite = listBaseColorSpt[0];
                        listIndicNavColor[1].sprite = listSelectedColorSpt[1];
                        enumColor = ColorEnum.VERT;
                        break;
                    case ColorEnum.VERT:
                        listIndicNavColor[1].sprite = listBaseColorSpt[1];
                        listIndicNavColor[2].sprite = listSelectedColorSpt[2];
                        enumColor = ColorEnum.VIOLET;
                        break;
                    case ColorEnum.VIOLET:
                        listIndicNavColor[2].sprite = listBaseColorSpt[2];
                        listIndicNavColor[3].sprite = listSelectedColorSpt[3];
                        enumColor = ColorEnum.ROUGE;
                        break;
                    case ColorEnum.ROUGE:
                        listIndicNavColor[3].sprite = listBaseColorSpt[3];
                        listIndicNavColor[4].sprite = listSelectedColorSpt[4];
                        enumColor = ColorEnum.ORANGE;
                        break;
                    case ColorEnum.ORANGE:
                        listIndicNavColor[4].sprite = listBaseColorSpt[4];
                        listIndicNavColor[5].sprite = listSelectedColorSpt[5];
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
                        listIndicNavRace[2].transform.rotation = new Quaternion(0,0,0,0);
                        listIndicNavRace[0].transform.rotation = Quaternion.Euler(0,0,10);
                        enumRace = Race.GOBLIN;
                        break;
                    case Race.CHEVALIER:
                        listIndicNavRace[1].transform.rotation = new Quaternion(0,0,0,0);
                        listIndicNavRace[3].transform.rotation = Quaternion.Euler(0,0,10);
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
                
                foreach (var item in listIndicNavTeteGO.Where(item => item.transform.rotation.x.Equals(-1)))
                {
                    StartCoroutine(HandAnim(item));
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
                
                foreach (var item in listIndicNavCorpsGO.Where(item => item.transform.rotation.x.Equals(-1)))
                {
                    StartCoroutine(HandAnim(item));
                }
                VisualRefresh();
                break;
            case Custo.COULEUR:
                switch (enumColor)
                {
                    case ColorEnum.VERT:
                        listIndicNavColor[1].sprite = listBaseColorSpt[1];
                        listIndicNavColor[0].sprite = listSelectedColorSpt[0];
                        enumColor = ColorEnum.BLANC;
                        break;
                    case ColorEnum.VIOLET:
                        listIndicNavColor[2].sprite = listBaseColorSpt[2];
                        listIndicNavColor[1].sprite = listSelectedColorSpt[1];
                        enumColor = ColorEnum.VERT;
                        break;
                    case ColorEnum.ROUGE:
                        listIndicNavColor[3].sprite = listBaseColorSpt[3];
                        listIndicNavColor[2].sprite = listSelectedColorSpt[2];
                        enumColor = ColorEnum.VIOLET;
                        break;
                    case ColorEnum.ORANGE:
                        listIndicNavColor[4].sprite = listBaseColorSpt[4];
                        listIndicNavColor[3].sprite = listSelectedColorSpt[3];
                        enumColor = ColorEnum.ROUGE;
                        break;
                    case ColorEnum.JAUNE:
                        listIndicNavColor[5].sprite = listBaseColorSpt[5];
                        listIndicNavColor[4].sprite = listSelectedColorSpt[4];
                        enumColor = ColorEnum.ORANGE;
                        break;
                }
                break;
        }
    }

    private void VisualRefresh()
    {
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

        for (int i = 0; i < listImageRace.Count; i++)
        {
            if (i.Equals(currentRaceIndex))
                listImageRace[currentRaceIndex].sprite = listSpriteRace[^1];
            else
            {
                listImageRace[i].sprite = listSpriteRace[i];
            }
        }
        bigColor.sprite = listBigColorSpt[currentColorIndex];

        teteIMG.sprite = listCurrentTete[currentTeteIndex];
        teteIMG.color = listColor[currentColorIndex];
        corpsIMG.sprite = listCurrentCorps[currentCorpsIndex];
        corpsIMG.color = listColor[currentColorIndex];
    }

    public void InitCusto()
    {
        enumCusto = Custo.RACE;
        enumRace = Race.GOBLIN;
        enumColor = ColorEnum.BLANC;
        
        listIndicNavRace[0].transform.rotation = Quaternion.Euler(0,0,10);

        foreach (var item in listIndicNavTeteGO)
        {
            item.transform.rotation = new Quaternion(0,0,0,0);
        }  
        
        foreach (var item in listIndicNavCorpsGO)
        {
            item.transform.rotation = new Quaternion(0,0,0,0);
        }

        colorLayout.SetActive(false);
        bigColor.enabled = true;

        indicNavReadyGO.SetActive(false);


        currentRace = listRaces[0];
        currentRaceIndex = 0;
        currentTeteIndex = 0;
        currentCorpsIndex = 0;
        currentColorIndex = 0;
        
        if (menuManager.selectColor != null)
        {
            if (menuManager.selectColor.Contains(ColorEnum.BLANC))
            {
                if (menuManager.selectColor.Contains(ColorEnum.VERT))
                {
                    if (menuManager.selectColor.Contains(ColorEnum.VIOLET))
                    {
                        currentColorIndex = 3;
                    }
                    currentColorIndex = 2;
                }
                currentColorIndex = 1;
            }
        }
        
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
                switch (enumRace)
                {
                    case Race.GOBLIN:
                        currentRaceIndex = 0;
                        break;
                    case Race.CHEVALIER:
                        currentRaceIndex = 1;
                        break;
                    case Race.DIABLOTIN:
                        currentRaceIndex = 2;
                        break;
                    case Race.HOMMEPOISSON:
                        currentRaceIndex = 3;
                        break;
                }
                currentRace = listRaces[currentRaceIndex];
                break;
            case Custo.COULEUR:
                switch (enumColor)
                {
                    case ColorEnum.BLANC:
                            currentColorIndex = 0;
                            bigColor.sprite = listBigColorSpt[0];
                        break;
                    case ColorEnum.VERT:
                            currentColorIndex = 1;
                            bigColor.sprite = listBigColorSpt[1];
                            break;
                    case ColorEnum.VIOLET:
                            currentColorIndex = 2;
                            bigColor.sprite = listBigColorSpt[2];
                            break;
                    case ColorEnum.ROUGE:
                            currentColorIndex = 3;
                            bigColor.sprite = listBigColorSpt[3];
                            break;
                    case ColorEnum.ORANGE:
                            currentColorIndex = 4;
                            bigColor.sprite = listBigColorSpt[4];
                            break;
                    case ColorEnum.JAUNE:
                            currentColorIndex = 5;
                            bigColor.sprite = listBigColorSpt[5];
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
        
        // switch (currentColorIndex)
        // {
        //     case 0:
        //         lockBlancGO.SetActive(!_isReady);
        //         break;
        //     case 1:
        //         lockVertGO.SetActive(!_isReady);
        //         break;
        //     case 2:
        //         lockVioletGO.SetActive(!_isReady);
        //         break;
        //     case 3:
        //         lockRougeGO.SetActive(!_isReady);
        //         break;
        //     case 4:
        //         lockOrangeGO.SetActive(!_isReady);
        //         break;
        //     case 5:
        //         lockJauneGO.SetActive(!_isReady);
        //         break;
        // }
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
            //Anim du parchemin qui s'ouvre et redscent + peut plus jouer avec son perso
            menuManager.readyCount--;
            readyIMG.color = Color.white;
            LockColor(isReady);
        }

        isReady = !isReady;
        VisualRefresh();
        menuManager.partyBandeauReadyGO.SetActive(IsLaunchPossible());
        if (isReady)
        {
            SpawnPlayer();
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

    private void SpawnPlayer()
    {
        menuManager.listMaskPersonnagesAnimator[playerSOIndex].SetTrigger(Burning);
        menuManager.multiplayerManager.players[playerSOIndex] = menuManager.listInGamePlayerControllers[playerSOIndex];
        FillSO();
        menuManager.listInGamePlayerControllers[playerSOIndex].SetupSprite(menuManager.playersListSO.players[playerSOIndex]);
        readyIMG.GetComponent<Button>().interactable = false;
        RemoveAllListeners();
        menuManager.listInGamePlayerControllers[playerSOIndex].gameObject.SetActive(true);
        menuManager.multiplayerManager.InitMultiplayer();
    }

    public void SpawnSelectionScreen()
    {
        menuManager.listMaskPersonnagesAnimator[playerSOIndex].SetTrigger(BackBurning);
        menuManager.multiplayerManager.players[playerSOIndex] = menuManager.listUiPlayerControllers[playerSOIndex];
        readyIMG.GetComponent<Button>().interactable = true;
        AddAllListeners();
        menuManager.multiplayerManager.InitMultiplayer();
        Ready();
    }

    private IEnumerator HandAnim(GameObject obj)
    {
        Quaternion quatTemp = obj.transform.rotation;
        obj.transform.rotation = new Quaternion(0, 0, 0, 0);
        yield return new WaitForSeconds(1);
        obj.transform.rotation = quatTemp;
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
