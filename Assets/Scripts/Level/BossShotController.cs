using UnityEngine;

public class BossShotController : ThreatController
{
    // Animation
    public Rigidbody rb;
    public GameObject destructionEffect;

    // Movement
    private Vector3 startingPosition;
    public int maxDistance = 35;
    public float speed;

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

    new protected void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (destructionEffect != null)
            Instantiate(destructionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
