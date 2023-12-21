using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverInterfaceController : MonoBehaviour
{
    public GameObject fullStarPrefab;
    public GameObject emptyStarPrefab;
    private TMP_Text[] enemiesTMPText;
    private TMP_Text[] coinsTMPText;
    private List<Image>[] stars;
    private int levelQuantity = 4;
    private int starsQuantity = 3;

    private void Start()
    {
        this.enemiesTMPText = new TMP_Text[this.levelQuantity];
        this.coinsTMPText = new TMP_Text[this.levelQuantity];
        this.stars = new List<Image>[this.levelQuantity];
        for (int i = 0; i < this.levelQuantity; i++)
        {
            GameObject levelPanel = this.transform.GetChild(0).GetChild(1).GetChild(i).gameObject;
            GameObject enemiesPanel = levelPanel.transform.GetChild(1).gameObject;
            GameObject coinsPanel = levelPanel.transform.GetChild(2).gameObject;
            GameObject starsPanel = levelPanel.transform.GetChild(3).gameObject;
            this.enemiesTMPText[i] = enemiesPanel.transform.GetChild(1).GetComponent<TMP_Text>();
            this.coinsTMPText[i] = coinsPanel.transform.GetChild(1).GetComponent<TMP_Text>();
            this.stars[i] = new List<Image>();
            for (int j = 0; j < this.starsQuantity; j++)
            {
                GameObject star = Instantiate(emptyStarPrefab);
                star.transform.SetParent(starsPanel.transform, false);
                Image image = star.GetComponent<Image>();
                stars[i].Add(image);
            }
        }
    }

    public void UpdateLevelData(int level, DataItem_Int enemies, DataItem_Int coins)
    {
        this.enemiesTMPText[level].SetText(enemies.ToString().PadLeft(2, '0'));
        this.coinsTMPText[level].SetText(coins.ToString().PadLeft(2, '0'));
        float enemiesPercentage = (float)enemies.GetCurrentQuantity() / (float)enemies.GetMaxQuantity();
        float coinsPercentage = (float)coins.GetCurrentQuantity() / (float)coins.GetMaxQuantity();
        float score = (enemiesPercentage + coinsPercentage) / 2f;
        this.UpdateStars(level, score);
    }

    private void UpdateStars(int level, float score)
    {
        for (int i = 0; i < this.starsQuantity; i++)
        {
            if (score >= (i + 1) * 0.33f)
                this.stars[level][i].sprite = fullStarPrefab.GetComponent<Image>().sprite;
            else
                this.stars[level][i].sprite = emptyStarPrefab.GetComponent<Image>().sprite;
        }
    }

}
