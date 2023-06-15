using UnityEngine;

public class LaunchGamePostAnim : MonoBehaviour
{
    [SerializeField] private MiniGameManager _miniGameManager;
    [SerializeField] private SoundManager soundManager;

    private void Awake()
    {
        _miniGameManager ??= FindObjectOfType<MiniGameManager>();
        soundManager ??= FindObjectOfType<SoundManager>();
    }

    public void LaunchMinigame()
    {
        _miniGameManager.StartMiniGame();
    }

    public void SoundPlay()
    {
        soundManager.PlaySelfSound(gameObject.GetComponent<AudioSource>());
    }
}
