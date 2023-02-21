using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/Players", order = 1)]
public class PlayerSO : ResetOnMainMenu
{
    #region PlayerInfo

    [Header("PlayerInfo")] 
    public int id;
    public int points;
    public int ranking;

    #endregion

    #region Customisation

    [Header("Customisation")] 
    public Sprite head;
    public Sprite body;
    public Color color;

    #endregion

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("PlayerSO reset");
        points = 0;
        ranking = 0;
    }
}
