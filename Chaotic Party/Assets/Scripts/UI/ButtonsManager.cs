using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] public GameObject firstSelected;

    private void Start()
    {
        if(firstSelected) EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public static void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
