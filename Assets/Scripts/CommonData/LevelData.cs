using System;
using UnityEngine;

public class LevelData
{
    private GameObject[] checkPoints;
    private bool[] visitedCheckPoints;
    private int lastCheckPointIndex;

    public LevelData()
    {
        this.lastCheckPointIndex = -1;
    }

    /// Getters 
    public GameObject GetLastCheckPoint()
    {
        return this.checkPoints[this.lastCheckPointIndex];
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
            lastCheckPointIndex = index;
            this.visitedCheckPoints[index] = true;
            return false;
        }
        else
            return true;
    }

}