using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        Action transitionFinisherStartedAction, Action transitionFinisherDoneAction)
    {
        OnTransitionStarted += transitionStartedAction;
        OnTransitionDone += transitionDoneAction;
        OnTransitionFinisherStarted += transitionFinisherStartedAction;
        OnTransitionFinisherDone += transitionFinisherDoneAction;
        animator.SetTrigger(Launch);
        OnTransitionStarted?.Invoke();
    }

    public void TransitionDone()
    {
        OnTransitionDone?.Invoke();
    }

    private IEnumerator WaitTillSceneLoad(AsyncOperation asyncOperation)
    {
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        
        SceneManager.UnloadSceneAsync(_sceneToUnload);
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