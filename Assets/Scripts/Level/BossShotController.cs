using UnityEngine;

public class BossShotController : MonoBehaviour
{
    // Animation
    public Rigidbody rb;
    public GameObject destructionEffect;

    // Movement
    private Vector3 startingPosition;
    public int maxDistance = 35;
    public float speed;
    public float dyingCounter;
    public int healthTaken = 1;

    private void Start()
    {
        transform.LookAt(MainManager.Instance.playerController.transform.position + Vector3.up);
        startingPosition = transform.position;
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;

        if (Vector3.Distance(startingPosition, transform.position) > maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player"
            && MainManager.Instance.playerData.invulnerabilityCounter <= 0
            && dyingCounter == 0)
        {
            MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
            MainManager.Instance.playerData.ResetInvulnerabilityCounter();
            MainManager.Instance.playerController.Blink();
        }

        if (destructionEffect != null)
            Instantiate(destructionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
