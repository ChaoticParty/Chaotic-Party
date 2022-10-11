using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpamManager
{
    int nbClicks { get; }

    public void Click(int playerIndex);
}
