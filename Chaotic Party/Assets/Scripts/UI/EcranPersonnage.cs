using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EcranPersonnage : MonoBehaviour
{
    public MenuManager menuManager;
    public SkinSelector skinSelector;
    public GameObject actualPanel;
    [SerializeField] private sbyte playerSOIndex = 0;

    private bool isReady = false;

    [Header("Listes")]
    public List<Races> listRaces = new List<Races>();
    public List<Tetes> listTetes = new List<Tetes>();
    public List<Corps> listCorps = new List<Corps>();
    public List<Color> listColor = new List<Color>();
    [SerializeField] private List<SelectedSkin> listCurrentTete = new List<SelectedSkin>();
    [SerializeField] private List<SelectedSkin> listCurrentCorps = new List<SelectedSkin>();

    [Header("Reférences Custo")]
    [SerializeField] private GameObject ActivateCustoGO;
    [SerializeField] private GameObject DesactivateCustoGO;
    [SerializeField] private List<Sprite> listSpriteRace = new List<Sprite>();
    [SerializeField] private Image imageRace;
    [SerializeField] private TextMeshProUGUI nomRaceTMP;
    [Space]
    [SerializeField] private List<GameObject> listIndicNavTeteGO = new List<GameObject>();
    [SerializeField] private List<GameObject> listIndicNavCorpsGO = new List<GameObject>();
    [Space]
    [SerializeField] private Image colorImage;
    [SerializeField] private Sprite smallColor;
    [SerializeField] private Sprite bigColor;
    [Space]
    [SerializeField] private GameObject indicNavReadyGO;

    [Header("Affichage")] 
    [SerializeField] private Image readyIMG;
    [SerializeField] private Image backIMG;
    
    [Header("Animator")] 
    [SerializeField] private Animator leftStickToLeft;
    [SerializeField] private Animator leftStickToRight;
    [SerializeField] private Animator leftBumperClick;
    [SerializeField] private Animator rightBumperClick;
    [SerializeField] private Animator leftTriggerClick;
    [SerializeField] private Animator rightTriggerClick;
    [SerializeField] private Animator rightStickToLeft;
    [SerializeField] private Animator rightStickToRight;
    [SerializeField] private Animator aClick;
    private Races currentRace;
    private sbyte currentRaceIndex = 0; //De -128 à 128
    private sbyte currentTeteIndex = 0;
    private sbyte currentCorpsIndex = 0;
    private sbyte currentColorIndex = 0;
    
    [HideInInspector] public PlayerController myPlayerController;
    //Id des animations
    private static readonly int Burning = Animator.StringToHash("Burning");
    private static readonly int BackBurning = Animator.StringToHash("BackBurning");

    private Coroutine backBtnCoroutine = null;


    private void Awake()
    {
        InitCusto();
    }

    private void OnEnable()
    {
        InitCusto();
        
        AddAllListeners();
    }
    
    private void OnDisable()
    {
        BackToMainReleased();
    }

    public void AddAllListeners()
    {
        myPlayerController.leftStickJustMovedLeft.AddListener(RaceChangeLeft);
        myPlayerController.dPadLeft.AddListener(RaceChangeLeft);
        myPlayerController.leftStickJustMovedRight.AddListener(RaceChangeRight);
        myPlayerController.dPadRight.AddListener(RaceChangeRight);
        
        myPlayerController.leftBumperClick.AddListener(HeadChangeLeft);
        myPlayerController.rightBumperClick.AddListener(HeadChangeRight);
        
        myPlayerController.leftTriggerClick.AddListener(BodyChangeLeft);
        myPlayerController.rightTriggerClick.AddListener(BodyChangeRight);
        
        myPlayerController.rightStickJustMovedLeft.AddListener(ColorChangeLeft);
        myPlayerController.rightStickJustMovedRight.AddListener(ColorChangeRight);
        
        myPlayerController.aJustPressed.AddListener(Ready);
        myPlayerController.bJustPressed.AddListener(LauchBackToMain);
        myPlayerController.bJustReleased.AddListener(BackToMainReleased);
    }
    public void RemoveAllListeners()
    {
        myPlayerController.leftStickJustMovedLeft.RemoveListener(RaceChangeLeft);
        myPlayerController.dPadLeft.RemoveListener(RaceChangeLeft);
        myPlayerController.leftStickJustMovedRight.RemoveListener(RaceChangeRight);
        myPlayerController.dPadRight.RemoveListener(RaceChangeRight);
        
        myPlayerController.leftBumperClick.RemoveListener(HeadChangeLeft);
        myPlayerController.rightBumperClick.RemoveListener(HeadChangeRight);
        
        myPlayerController.leftTriggerClick.RemoveListener(BodyChangeLeft);
        myPlayerController.rightTriggerClick.RemoveListener(BodyChangeRight);
        
        myPlayerController.rightStickJustMovedLeft.RemoveListener(ColorChangeLeft);
        myPlayerController.rightStickJustMovedRight.RemoveListener(ColorChangeRight);
        
        myPlayerController.aJustPressed.RemoveListener(Ready);
        myPlayerController.bJustPressed.RemoveListener(LauchBackToMain);
        myPlayerController.bJustReleased.RemoveListener(BackToMainReleased);
    }

    private void RaceChangeRight(float x = 0, float y = 0)
    {
        RaceChangeRight();
    }
    private void RaceChangeRight()
    {
        if (isReady) return;
        leftStickToRight.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
        currentTeteIndex = 0;
        currentCorpsIndex = 0;
        if (currentRaceIndex == listRaces.Count - 1)
        {
            currentRaceIndex = 0;
        }
        else
        {
            currentRaceIndex ++;
        }
        VisualRefresh();
    }
    private void RaceChangeLeft(float x = 0, float y = 0)
    {
        RaceChangeLeft();
    }
    private void RaceChangeLeft()
    {
        if (isReady) return;
        leftStickToLeft.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
        currentTeteIndex = 0;
        currentCorpsIndex = 0;
        if (currentRaceIndex == 0)
        {
            currentRaceIndex = Convert.ToSByte(listRaces.Count - 1);
        }
        else
        {
            currentRaceIndex --;
        }
        VisualRefresh();
    }
    
    
    private void HeadChangeRight()
    {
        if (isReady) return;
        rightBumperClick.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
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
    }
    private void HeadChangeLeft()
    {
        if (isReady) return;
        leftBumperClick.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
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
    }
    
    private void BodyChangeRight()
    {
        if (isReady) return;
        rightTriggerClick.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
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
    }
    private void BodyChangeLeft()
    {
        if (isReady) return;
        leftTriggerClick.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
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
    }
    
    
    private void ColorChangeRight(float x = 0, float y = 0)
    {
        ColorChangeRight();
    }
    private void ColorChangeRight()
    {
        if (isReady) return;
        rightStickToRight.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
        if (currentColorIndex == listColor.Count - 1)
        {
            currentColorIndex = 0;
            while (!IsColorDispo(currentColorIndex))
            {
                currentColorIndex++;
            }
        }
        else
        {
            currentColorIndex ++;
            while (!IsColorDispo(currentColorIndex))
            {
                if (currentColorIndex == listColor.Count - 1)
                {
                    currentColorIndex = 0;
                }
                else
                {
                    currentColorIndex++;
                }
            }
        }
        VisualRefresh();
    }
    private void ColorChangeLeft(float x = 0, float y = 0)
    {
        ColorChangeLeft();
    }
    private void ColorChangeLeft()
    {
        if (isReady) return;
        rightStickToLeft.SetTrigger("Push");
        menuManager.soundManager.PlaySelfSound();
        
        if (currentColorIndex == 0)
        {
            currentColorIndex = Convert.ToSByte(listColor.Count - 1);
            while (!IsColorDispo(currentColorIndex))
            {
                currentColorIndex--;
            }
        }
        else
        {
            currentColorIndex --;
            while (!IsColorDispo(currentColorIndex))
            {
                if (currentColorIndex == 0)
                {
                    currentColorIndex = Convert.ToSByte(listColor.Count - 1);
                }
                else
                {
                    currentColorIndex--;
                }
            }
        }
        VisualRefresh();
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

        if (!IsColorDispo(currentColorIndex))
        {
            if (currentColorIndex == listColor.Count - 1)
            {
                currentColorIndex = 0;
                while (!IsColorDispo(currentColorIndex))
                {
                    currentColorIndex++;
                }
            }
            else
            {
                currentColorIndex ++;
                while (!IsColorDispo(currentColorIndex))
                {
                    if (currentColorIndex == listColor.Count - 1)
                    {
                        currentColorIndex = 0;
                    }
                    else
                    {
                        currentColorIndex++;
                    }
                }
            }
        }

        imageRace.sprite = listSpriteRace[currentRaceIndex];
        
        colorImage.color = listColor[currentColorIndex];
        
        nomRaceTMP.text = listRaces[currentRaceIndex].nomRace;

        SelectedSkin teteTemp = SelectedSkin.GOBLIN_BASE;
        SelectedSkin corpsTemp = SelectedSkin.GOBLIN_BASE;

        switch (currentRaceIndex)
        {
            case 0 :
                switch (currentTeteIndex)
                {
                    case 0 :
                        teteTemp = SelectedSkin.GOBLIN_BASE;
                        break;
                    case 1 :
                        teteTemp = SelectedSkin.GOBLIN_FRANCAIS;
                        break;
                    case 2 :
                        teteTemp = SelectedSkin.GOBLIN_CARTON;
                        break;
                    case 3 :
                        teteTemp = SelectedSkin.GOBLIN_FEE;
                        break;
                }
                switch (currentCorpsIndex)
                {
                    case 0 :
                        corpsTemp = SelectedSkin.GOBLIN_BASE;
                        break;
                    case 1 :
                        corpsTemp = SelectedSkin.GOBLIN_FRANCAIS;
                        break;
                    case 2 :
                        corpsTemp = SelectedSkin.GOBLIN_CARTON;
                        break;
                    case 3 :
                        corpsTemp = SelectedSkin.GOBLIN_FEE;
                        break;
                }
                break;
            case 1 :
                switch (currentTeteIndex)
                {
                    case 0 :
                        teteTemp = SelectedSkin.CHEVALIER_BASE;
                        break;
                    case 1 :
                        teteTemp = SelectedSkin.CHEVALIER_LAMPE;
                        break;
                    case 2 :
                        teteTemp = SelectedSkin.CHEVALIER_UWU;
                        break;
                    case 3 :
                        teteTemp = SelectedSkin.CHEVALIER_COFFRE;
                        break;
                }
                switch (currentCorpsIndex)
                {
                    case 0 :
                        corpsTemp = SelectedSkin.CHEVALIER_BASE;
                        break;
                    case 1 :
                        corpsTemp = SelectedSkin.CHEVALIER_LAMPE;
                        break;
                    case 2 :
                        corpsTemp = SelectedSkin.CHEVALIER_UWU;
                        break;
                    case 3 :
                        corpsTemp = SelectedSkin.CHEVALIER_COFFRE;
                        break;
                }
                break;
            case 2 :
                switch (currentTeteIndex)
                {
                    case 0 :
                        teteTemp = SelectedSkin.DIABLOTIN_BASE;
                        break;
                    case 1 :
                        teteTemp = SelectedSkin.DIABLOTIN_DODO;
                        break;
                    case 2 :
                        teteTemp = SelectedSkin.DIABLOTIN_PUTE;
                        break;
                    case 3 :
                        teteTemp = SelectedSkin.DIABLOTIN_CYCLOPE;
                        break;
                }
                switch (currentCorpsIndex)
                {
                    case 0 :
                        corpsTemp = SelectedSkin.DIABLOTIN_BASE;
                        break;
                    case 1 :
                        corpsTemp = SelectedSkin.DIABLOTIN_DODO;
                        break;
                    case 2 :
                        corpsTemp = SelectedSkin.DIABLOTIN_PUTE;
                        break;
                    case 3 :
                        corpsTemp = SelectedSkin.DIABLOTIN_CYCLOPE;
                        break;
                }
                break;
            case 3 :
                switch (currentTeteIndex)
                {
                    case 0 :
                        teteTemp = SelectedSkin.HOMMEPOISSON_BASE;
                        break;
                    case 1 :
                        teteTemp = SelectedSkin.HOMMEPOISSON_LANTERNE;
                        break;
                    case 2 :
                        teteTemp = SelectedSkin.HOMMEPOISSON_GOTHIC;
                        break;
                    case 3 :
                        teteTemp = SelectedSkin.HOMMEPOISSON_REQUIN;
                        break;
                }
                switch (currentCorpsIndex)
                {
                    case 0 :
                        corpsTemp = SelectedSkin.HOMMEPOISSON_BASE;
                        break;
                    case 1 :
                        corpsTemp = SelectedSkin.HOMMEPOISSON_LANTERNE;
                        break;
                    case 2 :
                        corpsTemp = SelectedSkin.HOMMEPOISSON_GOTHIC;
                        break;
                    case 3 :
                        corpsTemp = SelectedSkin.HOMMEPOISSON_REQUIN;
                        break;
                }
                break;
        }
        
        skinSelector.SetupSkin(teteTemp, corpsTemp, listColor[currentColorIndex]);
    }

    public void InitCusto()
    {
        isReady = false;
        foreach (var item in listIndicNavTeteGO)
        {
            item.transform.rotation = new Quaternion(0,0,0,0);
        }  
        
        foreach (var item in listIndicNavCorpsGO)
        {
            item.transform.rotation = new Quaternion(0,0,0,0);
        }

        indicNavReadyGO.SetActive(false);

        currentRace = listRaces[0];
        currentRaceIndex = 0;
        currentTeteIndex = 0;
        currentCorpsIndex = 0;
        currentColorIndex = 0;
        
        if (menuManager.selectColor != null)
        {
            if (menuManager.selectColor.ContainsValue(0))
            {
                if (menuManager.selectColor.ContainsValue(1))
                {
                    if (menuManager.selectColor.ContainsValue(2))
                    {
                        currentColorIndex = 3;
                    }
                    currentColorIndex = 2;
                }
                currentColorIndex = 1;
            }
        }
        
        VisualRefresh();
    }

    private void LauchBackToMain()
    {
        menuManager.backTrembleAnim.SetTrigger("Tremble");
        backBtnCoroutine = StartCoroutine(BackToMain());
    }

    private IEnumerator BackToMain()
    {
        if (menuManager.isPressingBack.Item2)
        {
            if (!menuManager.isPressingBack.Item1.Equals(playerSOIndex)) yield break;
        }
        menuManager.isPressingBack = (playerSOIndex, true);
        
        if (menuManager.currentBackBtnTime >= menuManager.backBtnTimeMax)
        {
            BackToMainReleased();
            menuManager.partyBackMask.sizeDelta = new Vector2(menuManager.maxWeightPartyBackmask, menuManager.partyBackMask.sizeDelta.y);
            menuManager.backAnim.SetTrigger("Push");
            menuManager.Back(actualPanel);
            EventSystem.current.SetSelectedGameObject(menuManager.partyBTN.gameObject);
        }
        else
        {
            menuManager.partyBackMask.sizeDelta = new Vector2(menuManager.currentBackBtnTime / menuManager.backBtnTimeMax * menuManager.maxWeightPartyBackmask, menuManager.partyBackMask.sizeDelta.y);
        }

        menuManager.currentBackBtnTime += Time.deltaTime;

        yield return null;
        
        backBtnCoroutine = StartCoroutine(BackToMain());
    }

    private void BackToMainReleased()
    {
        if (backBtnCoroutine != null) StopCoroutine(backBtnCoroutine);
        
        menuManager.backTrembleAnim.SetTrigger("Idle");
        backBtnCoroutine = null;
        menuManager.currentBackBtnTime = 0;
        menuManager.isPressingBack = (-1, false);
        menuManager.partyBackMask.sizeDelta = new Vector2(0, menuManager.partyBackMask.sizeDelta.y);
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
            menuManager.selectColor.Add(playerSOIndex, currentColorIndex);
        else
            menuManager.selectColor.Remove(playerSOIndex);
    }

    private bool IsColorDispo(sbyte colorIndex)
    {
        if (menuManager.selectColor.ContainsValue(colorIndex) && !menuManager.selectColor.ContainsKey(playerSOIndex))
        {
            return false;
        }

        return true;
    }
    
    private void Ready()
    {
        if (!isReady)
        {
            aClick.SetTrigger("Push");
            //Anim du parchemin qui se ferme et remonte + possibilité au joueur de jouer avec son perso
            menuManager.readyCount++;
            //Faire le check aussi
        }
        else
        {
            //Anim du parchemin qui s'ouvre et redscent + peut plus jouer avec son perso
            menuManager.readyCount--;
        }
        LockColor(isReady);

        isReady = !isReady;
        foreach (EcranPersonnage ecranPersonnage in menuManager.listPersonnages)
        {
            ecranPersonnage.VisualRefresh();
        }
        menuManager.partyBandeauReadyGO.SetActive(menuManager.IsLaunchPossible());
        if (isReady)
        {
            SpawnPlayer();
        }
    }

    private void UiCloseAnim()
    {
        indicNavReadyGO.SetActive(true);
        imageRace.sprite = listSpriteRace[^1];
        colorImage.sprite = bigColor;
    }
    private void UiOpenAnim()
    {
        indicNavReadyGO.SetActive(false);
        imageRace.sprite = listSpriteRace[currentRaceIndex];
        colorImage.sprite = smallColor;
    }

    private void SpawnPlayer()
    {
        UiCloseAnim();
        menuManager.listMaskPersonnagesAnimator[playerSOIndex].SetTrigger(Burning);
        menuManager.multiplayerManager.players[playerSOIndex] = menuManager.listInGamePlayerControllers[playerSOIndex];
        FillSO();
        menuManager.listInGamePlayerControllers[playerSOIndex].gameObject.SetActive(true);
        menuManager.listInGamePlayerControllers[playerSOIndex].ActivateBulle(true);
        menuManager.listInGamePlayerControllers[playerSOIndex].ChangeBulleText("Y");
        menuManager.listInGamePlayerControllers[playerSOIndex].transform.position = Camera.main.ScreenToWorldPoint(transform.position);
        menuManager.listInGamePlayerControllers[playerSOIndex].transform.localScale = new Vector3(1,1,1);
        menuManager.listInGamePlayerControllers[playerSOIndex].SetupSprite(menuManager.playersListSO.players[playerSOIndex]);
        RemoveAllListeners();
        menuManager.multiplayerManager.InitMultiplayer();
    }

    public void SpawnSelectionScreen()
    {
        UiOpenAnim();
        menuManager.listMaskPersonnagesAnimator[playerSOIndex].SetTrigger(BackBurning);
        menuManager.multiplayerManager.players[playerSOIndex] = menuManager.listUiPlayerControllers[playerSOIndex];
        AddAllListeners();
        menuManager.multiplayerManager.InitMultiplayer();
        Ready();
        VisualRefresh();
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
    public string nomRace;
}
[Serializable]
public struct Tetes
{
    public List<SelectedSkin> listTête;
    public string nomTete;
}
[Serializable]
public struct Corps
{
    public List<SelectedSkin> listCorps;
    public string nomCorps;
}
