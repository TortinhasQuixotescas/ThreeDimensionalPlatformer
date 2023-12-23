using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public int bossHealth;
    public int enemiesQuantity;
    public int coinsQuantity;

    private void Start()
    {
        MainManager.Instance.currentLevelData = new LevelData(this.bossHealth, this.enemiesQuantity, this.coinsQuantity);
        AudioManager.instance.PlayMusic(2);
    }
}
