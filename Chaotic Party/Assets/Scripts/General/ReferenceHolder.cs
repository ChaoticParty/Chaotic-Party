using UnityEngine;

public class ReferenceHolder : MonoBehaviour
{
    private static ReferenceHolder instance;
    public static ReferenceHolder Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            }
            else
            {
                return instance = new GameObject("SingletonHolder").AddComponent<ReferenceHolder>();
            }
        }
    }
    public PlayersListSO players;
    public MiniGameData miniGameData;
    public GameObject oldEventSystem;
    public TransitionSetter transitionSetter;

    private void Awake()
    {
        instance = this;
        miniGameData ??= Resources.Load<MiniGameData>("ScriptableObjects/MiniGameData");
        players ??= Resources.Load<PlayersListSO>("ScriptableObjects/Players/Players");
        ResetPlayerData();
        ResetMiniGameData();
        DontDestroyOnLoad(gameObject);
    }

    private void ResetPlayerData()
    {
        foreach (PlayerSO player in players.players)
        {
            player.points = 0;
            player.ranking = 0;
        }
    }

    private void ResetMiniGameData()
    {
        //miniGameData.chosenMiniGames = new List<string>();
        miniGameData.currentMiniGameIndex = 0;
    }
}
