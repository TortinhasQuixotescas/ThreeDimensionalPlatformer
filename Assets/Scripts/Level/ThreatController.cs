using UnityEngine;

public class ThreatController : MonoBehaviour
{
    public int healthTaken = 1;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
        }
    }

}
