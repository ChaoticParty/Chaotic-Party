using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spam_EcrousImpact : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public List<Material> impactMaterial;

    [Button]
    public void EcrousImpact()
    {
        particleSystem.Play();
        int RandomImpact = Random.Range(0, impactMaterial.Count);
        particleSystem.GetComponent<Renderer>().material = impactMaterial[RandomImpact];
    }
}
