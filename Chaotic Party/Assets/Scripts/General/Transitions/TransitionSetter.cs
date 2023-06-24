using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TransitionSetter : MonoBehaviour
{
    public List<TransitionController> transitionControllers = new();
    private readonly Dictionary<TransitionController, TransitionController> _transitionObjects = new();
    public TransitionController lastTransition;

    public void StartTransition(Action transitionStartedAction, Action transitionDoneAction, 
        Action transitionFinisherStartedAction, Action transitionFinisherDoneAction, Vector3 position = default)
    {
        StartTransition(Random.Range(0, transitionControllers.Count), transitionStartedAction, transitionDoneAction, 
            transitionFinisherStartedAction, transitionFinisherDoneAction, position);
    }

    public void StartTransition(int index, Action transitionStartedAction, Action transitionDoneAction, 
        Action transitionFinisherStartedAction, Action transitionFinisherDoneAction, Vector3 position = default)
    {
        TransitionController prefab = transitionControllers[index];
        bool isPrefabInstantiated = _transitionObjects.ContainsKey(prefab);
        TransitionController transitionScript = isPrefabInstantiated 
            ?  _transitionObjects[prefab] : Instantiate(prefab, transform);
        
        if(!isPrefabInstantiated) _transitionObjects.Add(prefab, transitionScript);
        
        transitionScript.gameObject.SetActive(true);
        transitionScript.StartTransition(transitionStartedAction, transitionDoneAction, 
            transitionFinisherStartedAction, transitionFinisherDoneAction, position);

        lastTransition = transitionScript;
    }

    public void WaitTillSceneLoad(AsyncOperation asyncOperation, Scene sceneToUnload)
    {
        StartCoroutine(lastTransition.WaitTillSceneLoad(asyncOperation, sceneToUnload));
    }

    public void SetTriggerLaunch()
    {
        lastTransition.FinishTransition();
    }
}
