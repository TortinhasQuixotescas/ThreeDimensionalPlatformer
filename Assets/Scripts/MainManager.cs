using System;
using System.Collections;
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
    public int healingCost = 10;
    public int maxPlayerHealth = 6;
    public int invulnerabilityDuration = 1;
    public float blinkDuration = 0.1f;

    // Level Data
    private int levelsQuantity = 3;
    public LevelInfo[] levelsInfo;
    public LevelData currentLevelData;
    private int currentLevelNumber = 0;
    private string currentLevelName = "";
    private bool respawning;
    public float respawnDelay = 0.5f;

    // Interface
    public GameObject overlayInterfaceCanvas;
    private OverlayInterfaceController overlayInterfaceController;
    public GameObject gameOverInterfaceCanvas;

    private void Awake()
    {
        this.levelsInfo = new LevelInfo[this.levelsQuantity];
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// Game Cycle Manipulation
    public void RestartGame(int startLevelNumber = 1)
    {
        this.respawning = false;
        for (int i = 0; i < this.levelsQuantity; i++)
            this.levelsInfo[i] = new LevelInfo(
                new DataItem_Int(0, 0),
                new DataItem_Int(0, 1),
                new DataItem_Int(0, 1)
            );
        this.currentLevelNumber = startLevelNumber - 1;
        this.currentLevelData = null;
        Cursor.visible = false;
        Time.timeScale = 1;
        LoadNextLevel();
    }

    public void FinishGame(bool victory)
    {
        gameOverInterfaceCanvas.SetActive(true);
        overlayInterfaceCanvas.SetActive(false);
        GameObject playerInterfaceCanvas = GameObject.FindGameObjectWithTag("PlayerInterfaceCanvas");
        playerInterfaceCanvas.SetActive(false);
        gameOverInterfaceCanvas.GetComponent<GameOverInterfaceController>().ShowGameOverScreen(victory, this.levelsInfo);
        Cursor.visible = true;
        SceneManager.UnloadSceneAsync(this.currentLevelName);
    }

    public void LoadNextLevel()
    {
        if (this.currentLevelData != null)
            this.levelsInfo[this.currentLevelNumber - 1] = this.currentLevelData.GetLevelInfo();
        this.currentLevelNumber++;
        switch (this.currentLevelNumber)
        {
            case 0:
                this.currentLevelName = "TestArea";
                break;
            case 1:
                this.currentLevelName = "Level_1";
                break;
            case 2:
                this.currentLevelName = "Level_2";
                break;
            case 3:
                this.currentLevelName = "Level_3";
                break;
            case 4:
                this.currentLevelName = "Level_4";
                break;
            default:
                this.FinishGame(true);
                return;
        }
        StartCoroutine(LoadLevel(this.currentLevelName));
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

    public IEnumerator LoadLevel(string sceneName)
    {
        yield return StartCoroutine(LoadAsyncLevel(sceneName, LoadSceneMode.Single));
        this.playerData = new PlayerData(this.maxPlayerHealth, this.invulnerabilityDuration, this.blinkDuration);
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.playerController = this.player.GetComponent<PlayerController>();
        this.currentLevelData.InitializeCheckPoints(GameObject.FindGameObjectsWithTag("CheckPoint"));

        yield return StartCoroutine(LoadAsyncLevel("Interface", LoadSceneMode.Additive));
        this.overlayInterfaceCanvas = GameObject.FindGameObjectWithTag("OverlayInterfaceCanvas");
        this.overlayInterfaceController = this.overlayInterfaceCanvas.GetComponent<OverlayInterfaceController>();
        this.gameOverInterfaceCanvas = GameObject.FindGameObjectWithTag("GameOverInterfaceCanvas");
        this.gameOverInterfaceCanvas.SetActive(false);
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
        playerController.characterController.gameObject.SetActive(false);
        overlayInterfaceController.FadeOut();

        AudioManager.instance.PlaySFX(10);

        yield return new WaitForSeconds(respawnDelay);

        // Move player to last checkpoint
        playerController.characterController.transform.position = currentLevelData.GetLastVisitedCheckPoint().transform.position;

        // Heal player
        int coins = playerData.GetCoinsQuantity();
        int healedHealth = Math.Min(coins / this.healingCost, playerData.GetMaxHealthValue());
        if (healedHealth == 0)
        { // Check game end
            if (this.currentLevelData != null)
                this.levelsInfo[this.currentLevelNumber - 1] = this.currentLevelData.GetLevelInfo();
            this.FinishGame(false);
            yield break;
        }
        int spentCoins = healedHealth * this.healingCost;
        playerData.IncreaseHealth(healedHealth);
        playerData.IncreaseCoins(-spentCoins);

        playerController.characterController.gameObject.SetActive(true);
        overlayInterfaceController.FadeIn();
        respawning = false;
    }

}