using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject creditsScreen;

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
