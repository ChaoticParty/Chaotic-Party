using UnityEditor;
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
                return null;
            }
        }
    }
    public PlayersListSO players;
    public MiniGameData miniGameData;
    public GameObject oldEventSystem;
    public TransitionSetter transitionSetter;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        miniGameData ??= Resources.Load<MiniGameData>("ScriptableObjects/MiniGameData");
        players ??= Resources.Load<PlayersListSO>("ScriptableObjects/Players/Players");
        ResetPlayerData();
        ResetMiniGameData();
        
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
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
