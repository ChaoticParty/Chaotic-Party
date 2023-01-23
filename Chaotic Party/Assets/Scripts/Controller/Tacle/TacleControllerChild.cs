using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacleControllerChild : MonoBehaviour
{
    private BoxCollider2D myCollider;
    private BoxCollider2D parentCollider;
    private Transform parentTransform;
    private PlayerController player;
    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        parentTransform = transform.parent;
        player = parentTransform.GetComponent<PlayerController>();
        parentCollider = parentTransform.GetComponent<BoxCollider2D>();
        
        transform.localScale = parentTransform.localScale; //Pour avoir la même taille
        myCollider.size = parentCollider.size;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (player.isTackling && other.TryGetComponent(out HitController hitController))
        {
            hitController.Hited(player.gameObject);
        }
    }
}
