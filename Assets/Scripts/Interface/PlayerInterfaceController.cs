using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterfaceController : MonoBehaviour
{
    public GameObject livesPanel;
    public GameObject coinsPanel;
    public GameObject bossHealthPanel;
    private Image bossHealthBar;
    private TMP_Text coinsTMPText;
    public GameObject fullHeartPrefab;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;
    private List<Image> hearts;

    private void Start()
    {
        this.bossHealthBar = bossHealthPanel.transform.GetChild(0).GetComponent<Image>();
        this.hearts = new List<Image>();
        this.coinsTMPText = coinsPanel.transform.GetChild(2).GetComponent<TMP_Text>();
        this.coinsTMPText.SetText(MainManager.Instance.playerData.GetCoinsQuantity().ToString().PadLeft(6, '0'));
        int heartsNumber = MainManager.Instance.playerData.GetMaxHealthValue() / 2;
        for (int i = 0; i < heartsNumber; i++)
        {
            GameObject heart = Instantiate(fullHeartPrefab);
            heart.transform.SetParent(livesPanel.transform, false);
            Image image = heart.GetComponent<Image>();
            hearts.Add(image);
        }
    }

    private void Update()
    {
        this.UpdateHearts(MainManager.Instance.playerData.GetHealthValue());
        this.coinsTMPText.SetText(MainManager.Instance.playerData.GetCoinsQuantity().ToString().PadLeft(6, '0'));
        this.UpdateBossHealth();
    }

    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int livesToBeFull = (i + 1) * 2;
            if (currentHealth >= livesToBeFull)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else if (currentHealth == livesToBeFull - 1)
            {
                hearts[i].sprite = halfHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }

    private void UpdateBossHealth()
    {
        if (MainManager.Instance.currentLevelData.GetBossMaxHealth() <= 0)
        {
            this.bossHealthPanel.SetActive(false);
            return;
        }
        float fillAmount = (float)MainManager.Instance.currentLevelData.GetBossHealth() / (float)MainManager.Instance.currentLevelData.GetBossMaxHealth();
        if (fillAmount <= 0)
        {
            fillAmount = 0;
            this.bossHealthPanel.SetActive(false);
        }
        else
        {
            this.bossHealthPanel.SetActive(true);
            this.bossHealthBar.fillAmount = fillAmount;
        }
    }

}