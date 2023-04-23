using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cast_Shadows : MonoBehaviour
{
    //Quelle est l'ombre 
    public GameObject ombre;
    //Quelle est la distance entre le player et le sol
    public float distance;
    //Sur quel layer le Raycast agit il
    public LayerMask layer;
    //L'objet servant de pied du joueur
    private Transform _footObject;
    //Le character controller du player
    private PlayerController _player;
    
    //Au commencement
    private void Start()
    {
        //Attribuer la variable _footObject à la variable située dans le script JumpController
        _footObject = GetComponent<JumpController>().footObject;
        
        _player = GetComponent<PlayerController>();
    }
/// <summary>
/// Lancement du raycast
/// </summary>
    [Button("Lancer le Raycast")]
    public void ShootRaycast()
    {
        //Lancement d'un raycast vers le bas, sur le layer attribué à la variable
        RaycastHit2D hit = Physics2D.Raycast(_footObject.position, -Vector2.up, Mathf.Infinity, layer);

        //Faire en sorte que la position de l'ombre soit la même que celle du joueur en x, et celle du sol en y
        Transform transform1 = transform;
        Vector3 position = transform1.position;
        ombre.transform.position = new Vector3(position.x, hit.point.y);
        
        //Calcule de la distance entre le player et le sol
        distance = Vector3.Distance(hit.point, position);
    
        //Calcule de la taille de l'ombre en fonction de la distance
        float newScale = 1 / distance;
        ombre.transform.localScale = Vector3.one * newScale;

    }
private void Update()
{
    if (!_player.isInTheAir)
        {
            return;
        }
    
        ShootRaycast();
    }
}
