using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public int maxHealth;
    public PlayerData playerData;

    public void RestartGame()
    {
        this.playerData = new PlayerData(this.maxHealth);
        Cursor.visible = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("TestArea", LoadSceneMode.Single);
        SceneManager.LoadScene("Interface", LoadSceneMode.Additive);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FinishGame(bool victory)
    {
        // Scene interfaceScene = SceneManager.GetSceneByName("Interface");
        // SceneManager.SetActiveScene(interfaceScene);
        // PlayerInterfaceController interfaceController = FindFirstObjectByType<PlayerInterfaceController>();
        // interfaceController.ShowFinishGamePanel(victory);
        Cursor.visible = true;
        // Time.timeScale = 0;
        Debug.Log(victory ? "Victory!" : "Game Over");
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}