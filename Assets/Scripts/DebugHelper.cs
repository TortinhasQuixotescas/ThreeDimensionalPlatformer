using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    public int startLevelNumber = 0;

    void Start()
    {
        MainManager.Instance.RestartGame(this.startLevelNumber);
    }

}
