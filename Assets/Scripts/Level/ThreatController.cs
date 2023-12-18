using UnityEngine;

public class ThreatController : MonoBehaviour
{
    public int healthTaken = 1;
    public bool instantKill = false;
    public GameObject[] playerDisplay;
    private float flashCounter;
    public float blinkDuration = 0.1f;
    private float invulnerabilityDuration;
    private float invulnerabilityCounter;

    void Start()
    {
        invulnerabilityCounter = 0;
        invulnerabilityDuration = PlayerController.uniqueInstance.invulnerabilityDuration;

        playerDisplay = PlayerController.uniqueInstance.GetModelParts();
    }
    void Update()
    {
        if (invulnerabilityCounter > 0)
        {
            invulnerabilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                flashCounter = blinkDuration;
                foreach (GameObject part in playerDisplay)
                    part.SetActive(!part.activeSelf);
            }
            if (invulnerabilityCounter <= 0)
            {
                foreach (GameObject part in playerDisplay)
                    part.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (this.instantKill)
                MainManager.Instance.playerData.IncreaseHealth(-1 * MainManager.Instance.playerData.GetMaxHealthValue());
            if (invulnerabilityCounter <= 0)
            {
                MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
                invulnerabilityCounter = invulnerabilityDuration;
            }
        }
    }


}
