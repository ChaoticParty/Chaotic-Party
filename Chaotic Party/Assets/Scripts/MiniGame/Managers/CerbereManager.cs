using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerbereManager : SpamManager
{
    public override void Click(int playerIndex, float value, SpamButton spamButton = SpamButton.Any)
    {
        //playerIndex de 0 à 3, player.index en gros
        //Stock la value totale dans le click array
        //value est le nb de points
    }
}
