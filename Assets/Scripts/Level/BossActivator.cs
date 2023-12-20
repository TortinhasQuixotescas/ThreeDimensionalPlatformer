using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject boss;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            boss.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
