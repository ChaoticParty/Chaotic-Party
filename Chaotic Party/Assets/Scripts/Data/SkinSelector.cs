using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] private Transform skinParent;
    [SerializeField] private List<SkinValues> headSkinValues;
    [SerializeField] private List<SkinValues> bodySkinValues;

    public void SetupSkin(SelectedSkin tete, SelectedSkin corps, Color color)
    {
        foreach (Transform child in skinParent)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
                if (child.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.enabled = false;
                }
            }
        }
        
        foreach (SkinValues sv in headSkinValues)
        {
            if (!sv.selectedSkin.Equals(tete)) continue;
            
            foreach (GameObject ap in sv.activatedPart)
            {
                ap.SetActive(true);
                ap.gameObject.GetComponent<SpriteRenderer>().enabled = true;
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
                ap.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            
            foreach (SpriteRenderer cp in sv.coloredPart)
            {
                cp.color = color;
            }
        }
    } //Pour tout le player
    public void SetupSkin(SelectedSkin tete, Color color)
    {
        foreach (Transform child in skinParent)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
                child.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
        foreach (SkinValues sv in headSkinValues)
        {
            if (!sv.selectedSkin.Equals(tete)) continue;
            
            foreach (GameObject ap in sv.activatedPart)
            {
                ap.SetActive(true);
                ap.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            foreach (SpriteRenderer cp in sv.coloredPart)
            {
                cp.color = color;
            }
        }
    } //Pour seulement la tête du player
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
