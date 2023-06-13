using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInvoker : MonoBehaviour
{
    public List<UnityEvent> unityEvents;

    public void InvokeEvent(int index = 0)
    {
        unityEvents[index].Invoke();
    }
}
