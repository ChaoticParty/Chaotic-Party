using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class RulesManager : MiniGameManager
{
    
    protected void Start()
    {
        
    }
    
    protected override int GetWinner()
    {
        return -1;
    }
}
