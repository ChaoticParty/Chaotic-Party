using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingRoomSpam : SpamController
{
    protected new void Awake()
    {
        base.Awake();
        player.aJustPressed.AddListener(Click);
    }
    
    protected override void Click()
    {
        Debug.Log("click");
        spamManager.Click(player.index, spamValue);
    }
}
