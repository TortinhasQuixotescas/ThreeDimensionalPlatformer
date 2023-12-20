using UnityEngine;

public class DestroyerController : ThreatController
{
    private void Start()
    {
        this.instantKill = true;
    }

    new protected void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            this.HurtPlayer();
        else
            Destroy(collider.gameObject);
    }

}
