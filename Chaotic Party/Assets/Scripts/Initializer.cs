using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public TextAsset optionsJson;
    public SOOptions optionsSO;
    
    //appeler une premiere fois la fonction d'attribution des options
    //ranger le so et le json dans un fichier et le drag & drop dans la prefab initializer et son script
    private void Awake()
    {
        
    }
}
