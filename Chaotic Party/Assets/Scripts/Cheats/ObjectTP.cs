using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTP : MonoBehaviour
{
    public Vector2 coordonées;
    public KeyCode inputActivation;

    private void Update()
    {
        if (Input.GetKeyDown(inputActivation))
        {
            transform.position = coordonées;
        }
    }
}
