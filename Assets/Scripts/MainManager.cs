using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private float TIME_LIMIT = 5f;
    public static MainManager Instance;

    // Player Data
    public GameObject player;
    public PlayerData playerData;
    public PlayerController playerController;
    public int maxHealth;
    public int healingCost = 10;
    public int invulnerabilityDuration;
    public float blinkDuration = 0.1f;

    // Level Data
    public bool respawning;
    public float respawnDelay = 0.5f;
    public LevelData currentLevel;
    public GameObject levelInterfaceCanvas;
    private LevelInterfaceController levelInterfaceController;

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

    /// Game Cycle Manipulation
    public void RestartGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(0));
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

    /// Level Manipulation 
    IEnumerator LoadAsyncLevel(string sceneName, LoadSceneMode mode)
    {
        // Load level scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        asyncLoad.allowSceneActivation = false;
        float time = 0;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;
            time += Time.deltaTime;
            if (time > TIME_LIMIT)
                break;
            yield return null;
        }
    }

    public IEnumerator LoadLevel(int levelNumber)
    {
        string sceneName = "";
        switch (levelNumber)
        {
            case 0:
                sceneName = "TestArea";
                break;
            default:
                sceneName = "TestArea";
                break;
        }

        yield return StartCoroutine(LoadAsyncLevel(sceneName, LoadSceneMode.Single));
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.playerController = this.player.GetComponent<PlayerController>();
        this.playerData = new PlayerData(this.maxHealth, this.invulnerabilityDuration, this.blinkDuration);
        this.currentLevel = new LevelData();
        this.currentLevel.InitializeCheckPoints(GameObject.FindGameObjectsWithTag("CheckPoint"));

        yield return StartCoroutine(LoadAsyncLevel("Interface", LoadSceneMode.Additive));
        this.levelInterfaceCanvas = GameObject.FindGameObjectWithTag("LevelInterfaceCanvas");
        this.levelInterfaceController = this.levelInterfaceCanvas.GetComponent<LevelInterfaceController>();
    }

    public void Respawn()
    {
        if (!respawning)
        {
            respawning = true;
            StartCoroutine(RespawnCoroutine());
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        // Disable player
        this.playerController.characterController.gameObject.SetActive(false);
        this.levelInterfaceController.FadeOut();
        yield return new WaitForSeconds(respawnDelay);

        // Move player to last checkpoint
        this.playerController.characterController.transform.position = currentLevel.GetLastCheckPoint().transform.position;

        // Heal player
        int coins = playerData.GetCoinsQuantity();
        int healedHealth = Math.Min(coins / this.healingCost, playerData.GetMaxHealthValue());
        if (healedHealth == 0)
        { // Check game end
            this.FinishGame(false);
            yield break;
        }
        int spentCoins = healedHealth * this.healingCost;
        playerData.IncreaseHealth(healedHealth);
        playerData.IncreaseCoins(-spentCoins);

        this.playerController.characterController.gameObject.SetActive(true);
        this.levelInterfaceController.FadeIn();
        respawning = false;
    }

}