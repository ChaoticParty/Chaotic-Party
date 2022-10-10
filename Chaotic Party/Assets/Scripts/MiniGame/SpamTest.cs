using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamTest : MonoBehaviour
{
    private void Awake()
    {
        PlayerController player = GetComponent<PlayerController>();
        player.aJustPressed.AddListener(Spam);
        player.aLongPressed.AddListener(LongPress);
        player.leftStickMoved.AddListener(TitillageStick);
    }

    private void Spam()
    {
        Debug.Log("nique");
    }

    private void LongPress(float pressDuration)
    {
        Debug.Log("nique long : " + pressDuration);
    }
    private void TitillageStick(float hor, float vert)
    {
        Debug.Log("titillage stick hor : "+ hor + "vert : "+vert);
    }
    
}
