using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CP_FX_Spark : MonoBehaviour
{
    [ChildGameObjectsOnly]
    public List<Transform> positionSpawn = new(); 
    public List<Transform> objectPrefab;
    [MinMaxSlider(0.1f, 2f,showFields: true)]
    public Vector2 scaleRange;
    public int minSpawn = 1;

    public void SpawnFXBurst()
    {
        //Random pour définir combien de FX vont apparaître
        int randomIndex = Random.Range(0, positionSpawn.Count);
        
        //Stocke le transform du gameobject résultant du random
        Transform positionTemp = positionSpawn[randomIndex];
        
        //Spawn d'un FX
        Transform smokeTemp = Instantiate(objectPrefab[Random.Range(0, objectPrefab.Count)], positionTemp);
    
        //Reset ses transform
        smokeTemp.localPosition = Vector3.zero;
        smokeTemp.localRotation = Quaternion.identity;

        //Définition de la taille
        float scaleRandomTemp = Random.Range(scaleRange.x, scaleRange.y);
        smokeTemp.localScale = new Vector3(scaleRandomTemp, scaleRandomTemp, scaleRandomTemp);
    }

    [Button("Lancement des opérations")]
    public void RepeatSpawn()
    {
        //Répéter l'opération du Spawn aléatoirement
        for (int i = 0; i < Random.Range(minSpawn, positionSpawn.Count); i++)
        {
            SpawnFXBurst();
        }
    }

    private void Awake()
    {
        RepeatSpawn();
    }
}
