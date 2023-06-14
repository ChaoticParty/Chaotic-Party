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
        OnTransitionStarted += transitionStartedAction;
        OnTransitionDone += transitionDoneAction;
        OnTransitionFinisherStarted += transitionFinisherStartedAction;
        OnTransitionFinisherDone += transitionFinisherDoneAction;
        animator.SetTrigger(Launch);
        OnTransitionStarted?.Invoke();
        SetPosition(position);
    }

    public void SetPosition(Vector3 position)
    {
        animator.transform.position = position;
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