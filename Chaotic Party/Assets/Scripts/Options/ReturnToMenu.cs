using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    private void Update()
    {
        if (Hinput.anyGamepad.back)
        {
            ButtonsManager.LoadScene("SelectionTrainingRoom");
        }
    }
}
