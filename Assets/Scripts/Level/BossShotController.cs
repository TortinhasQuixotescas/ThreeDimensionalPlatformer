using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotController : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    private PlayerController player;
    public int healthTaken = 1;
    public float dyingCounter;
    private float flashCounter;
    public float blinkDuration = 0.1f;
    public GameObject[] playerDisplay;
    public GameObject destructionEffect;
    private Vector3 startingPosition;
    public int maxDistance = 35;

    // Start is called before the first frame update
    void Start()
    {
        playerDisplay = PlayerController.uniqueInstance.GetModelParts();
        player = PlayerController.uniqueInstance;
        transform.LookAt(player.transform.position + Vector3.up);
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;


        if (Vector3.Distance(startingPosition, transform.position) > maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && player.invulnerabilityCounter <= 0 && dyingCounter == 0)
        {
            MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
            player.invulnerabilityCounter = player.invulnerabilityDuration;
            Blink();
        }

        if (destructionEffect != null)
            Instantiate(destructionEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void Blink()
    {
        if (player.invulnerabilityCounter > 0)
        {
            player.invulnerabilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                flashCounter = blinkDuration;
                foreach (GameObject part in playerDisplay)
                    part.SetActive(!part.activeSelf);
            }
            if (player.invulnerabilityCounter <= 0)
            {
                foreach (GameObject part in playerDisplay)
                    part.SetActive(true);
            }
        }
    }
}
