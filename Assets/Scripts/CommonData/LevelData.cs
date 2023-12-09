using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private CharacterController player;
    private List<GameObject> checkPoints;
    private int lastCheckPointIndex;

    public LevelData(CharacterController player)
    {
        this.player = player;
        this.checkPoints = new List<GameObject>();
        this.lastCheckPointIndex = -1;
    }

    /// Getters 
    public CharacterController GetPlayer()
    {
        return this.player;
    }

    public GameObject GetLastCheckPoint()
    {
        return this.checkPoints[this.lastCheckPointIndex];
    }

    /// Methods
    public void InitializeCheckPoints(GameObject[] checkPoints)
    {
        this.checkPoints.AddRange(checkPoints);
        if (checkPoints.Length > 0)
            this.lastCheckPointIndex = this.checkPoints.Count - 1;
    }

}
