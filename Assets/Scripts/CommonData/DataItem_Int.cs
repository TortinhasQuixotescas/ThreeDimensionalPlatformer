using System;

public class DataItem_Int
{
    private int currentQuantity;
    private int minQuantity;
    private int maxQuantity;

    public DataItem_Int(int minQuantity, int maxQuantity)
    {
        this.currentQuantity = minQuantity;
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

    public bool IsFull()
    {
        bool full = this.currentQuantity >= this.maxQuantity;
        return full;
    }

    public bool IsEmpty()
    {
        bool empty = this.currentQuantity <= this.minQuantity;
        return empty;
    }

}
