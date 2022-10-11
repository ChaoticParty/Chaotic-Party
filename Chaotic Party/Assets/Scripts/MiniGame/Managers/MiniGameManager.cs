using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameManager : MonoBehaviour
{
    public List<PlayerController> players;

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }
}
