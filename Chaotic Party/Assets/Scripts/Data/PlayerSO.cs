using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/Players", order = 1)]
public class PlayerSO : ScriptableObject
{
    #region PlayerInfo

    [Header("PlayerInfo")] 
    public int id;
    public int points;
    public int ranking;

    #endregion

    #region Customisation

    [Header("Customisation")] 
    public SelectedSkin head;
    public SelectedSkin body;
    public Color color;
    public Races race;

    #endregion
}
