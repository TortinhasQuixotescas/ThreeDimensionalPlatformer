using UnityEngine;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    public GameObject creditsScreen;


    void Start()
    {
        AudioManager.instance.PlayMusic(3);
    }
    public void StartGame()
    {
        MainManager.Instance.RestartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void CreditsController(bool mustClose)
    {
        creditsScreen.SetActive(!mustClose);
    }

}
