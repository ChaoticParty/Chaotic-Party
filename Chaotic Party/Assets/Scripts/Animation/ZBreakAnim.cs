using UnityEngine;

public class ZBreakAnim : MonoBehaviour
{
    public void Desactivate() //En fin d'anim
    {
        gameObject.SetActive(false);
    }
    
    public void SoundPlay()
    {
        if (gameObject.TryGetComponent<AudioSource>(out AudioSource source))
        {
            if (source.clip != null)
            {
                source.Play();
            }
        }
    }
}
