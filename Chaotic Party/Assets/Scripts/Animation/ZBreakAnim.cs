using UnityEngine;

public class ZBreakAnim : MonoBehaviour
{
    public void Desactivate() //En fin d'anim
    {
        gameObject.SetActive(false);
    }
}
