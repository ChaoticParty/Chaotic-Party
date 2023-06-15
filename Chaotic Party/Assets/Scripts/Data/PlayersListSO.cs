using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersListSO", menuName = "ScriptableObjects/PlayersList", order = 1)]
public class PlayersListSO : ScriptableObject
{
    public List<PlayerSO> players;
}
