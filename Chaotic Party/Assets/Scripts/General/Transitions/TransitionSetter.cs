using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TransitionSetter : MonoBehaviour
{
    public List<TransitionController> transitionControllers = new();
    private readonly Dictionary<TransitionController, TransitionController> _transitionObjects = new();

    public void StartTransition(Action transitionStartedAction, Action transitionDoneAction, 
        Action transitionFinisherStartedAction, Action transitionFinisherDoneAction)
    {
        StartTransition(Random.Range(0, transitionControllers.Count), transitionStartedAction, transitionDoneAction, 
            transitionFinisherStartedAction, transitionFinisherDoneAction);
    }

    public void StartTransition(int index, Action transitionStartedAction, Action transitionDoneAction, 
        Action transitionFinisherStartedAction, Action transitionFinisherDoneAction)
    {
        TransitionController prefab = transitionControllers[index];
        bool isPrefabInstantiated = _transitionObjects.ContainsKey(prefab);
        TransitionController transitionScript = isPrefabInstantiated 
            ? Instantiate(prefab) : _transitionObjects[prefab];
        
        if(!isPrefabInstantiated) _transitionObjects.Add(prefab, transitionScript);
        
        transitionScript.gameObject.SetActive(true);
        transitionScript.StartTransition(transitionStartedAction, transitionDoneAction, 
            transitionFinisherStartedAction, transitionFinisherDoneAction);
    }
}
