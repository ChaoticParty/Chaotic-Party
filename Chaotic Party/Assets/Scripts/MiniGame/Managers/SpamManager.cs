using TMPro;
using UnityEngine;

public abstract class SpamManager : MiniGameManager
{
    [SerializeField] protected TextMeshProUGUI[] spamTexts;
    protected float nbClicks;
    protected float[] clicksArray;
    [Header("Valeurs de spam"), Space(10)]
    [Range(0, 1000), Tooltip("Valeur qui sera ajoutée au conteur d'un joueur quand il spam")] public float spamValue;
    [Range(0, 1000), Tooltip("Valeur qui sera soustraite au conteur d'un joueur quand il spam")] public float versusSpamValue;

    protected void Start()
    {
        clicksArray = new float[players.Count];
    }

    public abstract void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any);
}
