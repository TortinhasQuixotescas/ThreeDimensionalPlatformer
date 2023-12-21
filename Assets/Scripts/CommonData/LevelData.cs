using System;
using UnityEngine;

public class LevelData
{
    private GameObject[] checkPoints;
    private bool[] visitedCheckPoints;
    private int lastVisitedCheckPointIndex;
    private DataItem_Int bossHealth;
    private DataItem_Int enemiesKilled;
    private DataItem_Int coinsCollected;

    public LevelData(int maxBosshealth, int maxCoins, int maxEnemies)
    {
        this.lastVisitedCheckPointIndex = -1;
        this.bossHealth = new DataItem_Int(0, maxBosshealth);
        this.enemiesKilled = new DataItem_Int(0, maxEnemies);
        this.coinsCollected = new DataItem_Int(0, maxCoins);
    }

    /// Getters 
    public GameObject GetLastVisitedCheckPoint()
    {
        return this.checkPoints[this.lastVisitedCheckPointIndex];
    }

    public int GetBossHealth()
    {
        return this.bossHealth.GetCurrentQuantity();
    }

    public int GetBossMaxHealth()
    {
        return this.bossHealth.GetMaxQuantity();
    }

    /* Get the number in the name of the checkpoint.
    If the name is not in the format 'CheckPoint_X', where X is a number,
    or if the number is lesser than 1, returns -1.
    */
    public int GetCheckPointNumber(GameObject checkPoint)
    {
        try
        {
            string numberT = checkPoint.name[11..];
            int number = int.Parse(numberT);
            if (number <= 0)
                return -1;
            return number;
        }
        catch (System.Exception)
        {
            Debug.LogError("CheckPoint name must be in format 'CheckPoint_X', where X is a number");
            return -1;
        }
    }

    public int GetCheckPointIndex(int number)
    {
        for (int i = 0; i < this.checkPoints.Length; i++)
        {
            if (this.GetCheckPointNumber(this.checkPoints[i]) == number)
                return i;
        }
        return -1;
    }

    /// Setters
    public void SetFinalCheckPointActive(bool active)
    {
        this.checkPoints[^1].SetActive(active);
    }

    public bool SetCheckPointAsVisited(GameObject checkPoint)
    {
        int number = this.GetCheckPointNumber(checkPoint);
        int index = this.GetCheckPointIndex(number);
        if (index == -1 || index >= this.checkPoints.Length)
            return false;
        bool hasBeenVisited = this.visitedCheckPoints[index];
        if (!hasBeenVisited)
        {
            lastVisitedCheckPointIndex = index;
            this.visitedCheckPoints[index] = true;
            return false;
        }
        else
            return true;
    }

    /// Methods
    public void InitializeCheckPoints(GameObject[] checkPoints)
    {
        // Initialize checkPoints and visitedCheckPoints
        int count = checkPoints.Length;
        this.checkPoints = new GameObject[count];
        this.visitedCheckPoints = new bool[count];

        // Copy checkPoints to this.checkPoints
        int aux = 0;
        for (int i = 0; i < count; i++)
        {
            int number = this.GetCheckPointNumber(checkPoints[i]);
            if (number == -1)
                continue;
            this.checkPoints[aux] = checkPoints[i];
            this.visitedCheckPoints[aux] = false;
            aux++;
        }
        Array.Sort(this.checkPoints, (x, y) => this.GetCheckPointNumber(x) - this.GetCheckPointNumber(y));

        // Set first checkpoint as visited
        this.SetCheckPointAsVisited(this.checkPoints[0]);
        this.checkPoints[0].GetComponent<CheckPointController>().SetAsShiny();
        this.checkPoints[^1].GetComponent<CheckPointController>().SetAsFinal();
    }

    public void StartBossFight()
    {
        this.bossHealth.IncreaseCurrentQuantity(this.bossHealth.GetMaxQuantity());
    }

    public void DecreaseBossHealth()
    {
        this.bossHealth.IncreaseCurrentQuantity(-1);
    }

}