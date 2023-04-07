using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] private Transform skinParent;
    [SerializeField] private List<SkinValues> headSkinValues;
    [SerializeField] private List<SkinValues> bodySkinValues;

    public void SetupSkin(SelectedSkin tete, SelectedSkin corps, Color color)
    {
        return;
        foreach (Transform child in skinParent)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }
        
        foreach (SkinValues sv in headSkinValues)
        {
            if (!sv.selectedSkin.Equals(tete)) continue;
            
            foreach (GameObject ap in sv.activatedPart)
            {
                ap.SetActive(true);
            }

            foreach (SpriteRenderer cp in sv.coloredPart)
            {
                cp.color = color;
            }
        }

        foreach (SkinValues sv in bodySkinValues)
        {
            if (!sv.selectedSkin.Equals(corps)) continue;
            
            foreach (GameObject ap in sv.activatedPart)
            {
                ap.SetActive(true);
            }
            
            foreach (SpriteRenderer cp in sv.coloredPart)
            {
                cp.color = color;
            }
        }
    }
}

public enum SelectedSkin
{
    GOBLIN_BASE, GOBLIN_FRANCAIS, GOBLIN_CARTON, GOBLIN_FEE,
    CHEVALIER_BASE, CHEVALIER_LAMPE, CHEVALIER_UWU, CHEVALIER_COFFRE,
    DIABLOTIN_BASE, DIABLOTIN_DODO, DIABLOTIN_PUTE, DIABLOTIN_CYCLOPE,
    HOMMEPOISSON_BASE, HOMMEPOISSON_LANTERNE, HOMMEPOISSON_GOTHIC, HOMMEPOISSON_REQUIN
}

[Serializable]
public struct SkinValues
{
    public SelectedSkin selectedSkin;
    public List<GameObject> activatedPart;
    public List<SpriteRenderer> coloredPart;
}
