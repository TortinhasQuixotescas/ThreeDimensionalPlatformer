using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterfaceController : MonoBehaviour
{

    public GameObject livesPanel;
    public GameObject coinsPanel;
    private TMP_Text coinsTMPText;
    public GameObject fullHeartPrefab;
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;
    private List<Image> hearts;

    private void Start()
    {
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
    }

    public void ShowFinishGamePanel(bool victory)
    {
        // TMP_Text title = this.FinishGamePanel.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        // title.SetText(victory ? "You Won!" : "Game Over");
        // this.FinishGamePanel.SetActive(true);
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

}