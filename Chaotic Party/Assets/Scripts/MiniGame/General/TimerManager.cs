using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private MiniGameManager gameManager;
    [SerializeField] private Image timerImage;
    [SerializeField] private Image timerFleche;
    [SerializeField] private float originTime;
    public float currentTime;
    public bool isPaused = false;

    private void Awake()
    {
        gameManager ??= FindObjectOfType<MiniGameManager>();
        gameManager.timerManager = this;
    }

    public void SetTimer(float timerInSecond)
    {
        originTime = timerInSecond;
        currentTime = originTime;
    }

    private void Update()
    {
        if(!gameManager.isMinigamelaunched || isPaused) return;
        
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            timerImage.gameObject.SetActive(false);
            timerFleche.transform.localRotation = Quaternion.Euler(0,0,-360);
            gameManager.FinishTimer();
            gameObject.SetActive(false);
        }
        else
        {
            timerImage.fillAmount = currentTime / originTime; //0 l'image est transparante, 1 elle est pleine.
            timerFleche.transform.localRotation = Quaternion.Euler(0,0,timerImage.fillAmount * 360); //1 fill = 0° et 0.5 fill = -90°
        }
    }
}
