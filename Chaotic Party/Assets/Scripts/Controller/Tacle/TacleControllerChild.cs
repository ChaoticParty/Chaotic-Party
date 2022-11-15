using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacleControllerChild : MonoBehaviour
{
    private BoxCollider2D myCollider;
    private BoxCollider2D parentCollider;
    private Transform parentTransform;
    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        parentTransform = transform.parent;
        parentCollider = parentTransform.GetComponent<BoxCollider2D>();
        
        transform.localScale = parentTransform.localScale; //Pour avoir la même taille
        myCollider.size = parentCollider.size;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<HitController>().Hited();
    }
}
