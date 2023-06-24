using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExplosionController : MiniGameController
{
    [SerializeField] private MenuManager menuManager;
    public GameObject actualPanel;
    [SerializeField] private float explosionDelay;
    private bool vibrate = false;
    private Coroutine backBtnCoroutine = null;
    private new void Awake()
    {
        base.Awake();
    }

    public override void AddListeners()
    {
        player.yJustPressed.AddListener(BaseMacronExplosion);
        // player.yJustPressed.AddListener(MacronExplosion); //TODO Same
        // player.yJustReleased.AddListener(CancelExplosion); //TODO y reactivé apres le fix de christophe
        player.bJustPressed.AddListener(LauchBackToMain);
        player.bJustReleased.AddListener(BackToMainReleased);
        player.startPressed.AddListener(ReadyClick);
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void FixedUpdate()
    {
        if (vibrate)
        {
            //Anim de vibration
        }
    }

    private void MacronExplosion(float delay)
    {
        if (delay < explosionDelay) vibrate = true;
        else
        {
            EndExplosionAnimMenu();
            //lancement anim d'explosion
        }
    }
    private void BaseMacronExplosion()
    {
        EndExplosionAnimMenu();
    }
    private void MacronExplosion()
    {
        if (!player.CanAct()) return;
        
        player.isExplosing = true;
        player.MExplosion(true);
        //EndExplosionAnimMenu();
    }
    private void CancelExplosion()
    {
        player.isExplosing = false;
        player.MExplosion(false);
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
            if (!menuManager.isPressingBack.Item1.Equals(player.index)) yield break;
        }
        menuManager.isPressingBack = (player.index, true);
        
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

    private void ReadyClick()
    {
        menuManager.LauchGame();
    }

    public void EndExplosionAnimMenu()
    {
        menuManager.listPersonnages[menuManager.multiplayerManager.players.IndexOf(player)].SpawnSelectionScreen();
        gameObject.SetActive(false);
    }
}
