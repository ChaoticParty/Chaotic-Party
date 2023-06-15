using UnityEngine;

public abstract class MiniGameController : MonoBehaviour
{
    protected PlayerController player;
    
    protected void Awake()
    {
        player ??= GetComponent<PlayerController>();
        player?.miniGameControllers.Add(this);
    }

    public abstract void AddListeners();
}
