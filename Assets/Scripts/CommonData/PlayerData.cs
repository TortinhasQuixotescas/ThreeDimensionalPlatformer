public class PlayerData
{
    private DataItem_Int health;
    private DataItem_Int coins;

    public PlayerData(int maxHealth)
    {
        this.health = new DataItem_Int(0, maxHealth);
        this.health.IncreaseCurrentQuantity(maxHealth);
        this.coins = new DataItem_Int(0, 999999);
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
            MainManager.Instance.FinishGame(false);
        }
    }

    public void IncreaseCoins(int increaseAmount)
    {
        this.coins.IncreaseCurrentQuantity(increaseAmount);
    }

}
