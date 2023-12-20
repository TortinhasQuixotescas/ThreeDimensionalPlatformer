using UnityEngine;

public class ThreatController : MonoBehaviour
{
    public int healthTaken = 1;
    public bool instantKill = false;

    protected void HurtPlayer()
    {
        if (this.instantKill)
            MainManager.Instance.playerData.IncreaseHealth(-1 * MainManager.Instance.playerData.GetMaxHealthValue());
        if (MainManager.Instance.playerData.invulnerabilityCounter <= 0)
        {
            MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
            MainManager.Instance.playerData.ResetInvulnerabilityCounter();
            MainManager.Instance.playerController.Blink();
        }
    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            this.HurtPlayer();
    }

}
