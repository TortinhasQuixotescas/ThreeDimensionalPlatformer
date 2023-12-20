public class PlayerData
{
    // Storage
    private DataItem_Int health;
    private DataItem_Int coins;

    // Animation
    private int invulnerabilityDuration;
    public float invulnerabilityCounter;
    public float flashCounter;
    public float blinkDuration;

    public PlayerData(int maxHealth, int invulnerabilityDuration, float blinkDuration)
    {
        this.health = new DataItem_Int(0, maxHealth);
        this.health.IncreaseCurrentQuantity(maxHealth);
        this.coins = new DataItem_Int(0, 999999);
        this.invulnerabilityDuration = invulnerabilityDuration;
        this.invulnerabilityCounter = 0;
        this.flashCounter = 0;
        this.blinkDuration = blinkDuration;
    }

    // Getters
    public int GetHealthValue()
    {
        return this.health.GetCurrentQuantity();
    }

    public int GetMaxHealthValue()
    {
        return this.health.GetMaxQuantity();
    }

    public int GetCoinsQuantity()
    {
        return this.coins.GetCurrentQuantity();
    }

    // Methods
    public void IncreaseHealth(int increaseAmount)
    {
        this.health.IncreaseCurrentQuantity(increaseAmount);
        if (this.health.IsEmpty())
        {
            MainManager.Instance.Respawn();
        }
    }

    public void IncreaseCoins(int increaseAmount)
    {
        this.coins.IncreaseCurrentQuantity(increaseAmount);
    }

    public void ResetInvulnerabilityCounter()
    {
        this.invulnerabilityCounter = this.invulnerabilityDuration;
    }

}
