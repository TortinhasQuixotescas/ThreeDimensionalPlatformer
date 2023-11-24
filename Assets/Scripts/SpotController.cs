using UnityEngine;

public class SpotController : MonoBehaviour
{
    public enum SpotType
    {
        Danger,
        Prize
    }

    public SpotType type;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            switch (this.type)
            {
                case SpotType.Danger:
                    MainManager.Instance.playerData.IncreaseHealth(-1);
                    break;
                case SpotType.Prize:
                    MainManager.Instance.playerData.IncreaseCoins(15);
                    break;
                default:
                    break;
            }
        }
    }

}
