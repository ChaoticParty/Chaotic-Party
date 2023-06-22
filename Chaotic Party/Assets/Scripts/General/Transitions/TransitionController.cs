using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    public Animator animator;
    private static readonly int Launch = Animator.StringToHash("Launch");
    private Action OnTransitionStarted;
    private Action OnTransitionDone;
    private Action OnTransitionFinisherStarted;
    private Action OnTransitionFinisherDone;
    private Scene _sceneToUnload;
    private Scene _sceneToLoad;

    public void StartTransition(Action transitionStartedAction, Action transitionDoneAction, 
        Action transitionFinisherStartedAction, Action transitionFinisherDoneAction, Vector3 position = default)
    {
        Debug.Log("start transi");
        OnTransitionStarted += transitionStartedAction;
        OnTransitionDone += transitionDoneAction;
        OnTransitionFinisherStarted += transitionFinisherStartedAction;
        OnTransitionFinisherDone += transitionFinisherDoneAction;
        animator.SetTrigger(Launch);
        OnTransitionStarted?.Invoke();
        SetPosition(position);

        EnableInputs(false);
    }

    private void EnableInputs(bool enable = true)
    {
        MultiplayerManager multiplayerManager = FindObjectOfType<MultiplayerManager>();
        if (!multiplayerManager) return;
        
        foreach (PlayerController player in multiplayerManager.players)
        {
            if(!player.gameObject.activeInHierarchy) continue;
            if(enable) 
                player.EnableAllInputs();
            else
                player.DisableAllInputs();  
        }
    }

    public void SetPosition(Vector3 position)
    {
        animator.transform.position = position;
    }

    public void SetUIPosition(Vector3 position)
    {
        animator.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    public void TransitionDone()
    {
        OnTransitionDone?.Invoke();
    }

    public IEnumerator WaitTillSceneLoad(AsyncOperation asyncOperation, Scene sceneToUnload)
    {
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        asyncOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        FinishTransition();
    }

    public void FinishTransition()
    {
        OnTransitionFinisherStarted?.Invoke();
        animator.SetTrigger(Launch);
    }

    public void TransitionFinisherDone()
    {
        OnTransitionFinisherDone?.Invoke();
        ResetEvents();
        EnableInputs(true);
        Time.timeScale = 1;
        DeactivateTransition();
    }

    private void ResetEvents()
    {
        OnTransitionStarted = null;
        OnTransitionDone = null;
        OnTransitionFinisherStarted = null;
        OnTransitionFinisherDone = null;
    }

    public void DeactivateTransition()
    {
        gameObject.SetActive(false);
    }
}