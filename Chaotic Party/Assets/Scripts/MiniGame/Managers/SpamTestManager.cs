using UnityEngine;

public class SpamTestManager : SpamManager
{
    public float nbClicks { get; private set; }
    public float[] clicksArray { get; private set; }

    private void Start()
    {
        clicksArray = new float[players.Count];
    }

    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        nbClicks += value;
        if(spamButton == SpamButton.A)
        {
            clicksArray[playerIndex] += value;
        }
        else
        {
            int otherPlayerindex = 0;
            switch (spamButton)
            {
                case SpamButton.B:
                    otherPlayerindex = 0;
                    break;
                case SpamButton.X:
                    otherPlayerindex = 1;
                    break;
                case SpamButton.Y:
                    otherPlayerindex = 2;
                    break;
            }

            if (otherPlayerindex <= playerIndex)
            {
                otherPlayerindex++;
            }
            
            players[otherPlayerindex].GetComponent<Spam>().Malus();
        }
    }
}
