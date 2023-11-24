using System;

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
        if (this.health.isEmpty())
        {
            MainManager.Instance.FinishGame(false);
        }
    }

    public void IncreaseCoins(int increaseAmount)
    {
        this.coins.IncreaseCurrentQuantity(increaseAmount);
    }

}

public class DataItem_Int
{
    private int currentQuantity;
    private int minQuantity;
    private int maxQuantity;

    public DataItem_Int(int minQuantity, int maxQuantity)
    {
        this.currentQuantity = 0;
        this.maxQuantity = maxQuantity;
        this.minQuantity = minQuantity;
    }

    public int GetCurrentQuantity()
    {
        return this.currentQuantity;
    }

    public int GetMinQuantity()
    {
        return this.minQuantity;
    }

    public int GetMaxQuantity()
    {
        return this.maxQuantity;
    }

    public void IncreaseCurrentQuantity(int increaseAmount)
    {
        this.currentQuantity = Math.Clamp(this.currentQuantity + increaseAmount, this.minQuantity, this.maxQuantity);
    }

    public bool isFull()
    {
        bool full = this.currentQuantity >= this.maxQuantity;
        return full;
    }

    public bool isEmpty()
    {
        bool empty = this.currentQuantity <= this.minQuantity;
        return empty;
    }

}
