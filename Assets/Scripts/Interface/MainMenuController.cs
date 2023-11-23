using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject creditsScreen;
    
    public void StartGame()
    {
        SceneManager.LoadScene("TestArea", LoadSceneMode.Single);
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
