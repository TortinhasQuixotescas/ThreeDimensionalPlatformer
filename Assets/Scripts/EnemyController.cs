using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public Transform[] routePoints;
    private int nextPoint = 0;
    public Rigidbody rb;
    private Vector3 moveDirection;
    private float yAux;
    private PlayerController player;
    private Vector3 lookDirection;
    public enum EnemyState
    {
        idle,
        patrolling,
        chasing,
        returning
    };
    public EnemyState currentState;
    public float waitDelay;
    public float waitCounter;
    public float waitProbability;
    public float chaseDistance;
    public float loseDistance;
    public float chaseSpeed;
    public float returningDelay;
    public float returnCounter;
    public float jumpSpeed;
    public float chaseDelay;
    public float chaseDelayCounter;
    public int healthTaken = 1;
    private float flashCounter;
    public float blinkDuration = 0.1f;
    public GameObject[] playerDisplay;
    public float dyingDelay;
    public float dyingCounter;
    public float deformationSpeed;

    public GameObject deathEffect;


    // Start is called before the first frame update
    void Start()
    {
        playerDisplay = PlayerController.uniqueInstance.GetModelParts();
        waitCounter = waitDelay;
        player = FindAnyObjectByType<PlayerController>();
        foreach (Transform routePoint in routePoints)
            routePoint.parent = null;

        currentState = EnemyState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        Blink();
        if (dyingCounter > 0)
        {
            dyingCounter -= Time.deltaTime;
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.25f, 0.5f, 1.25f), deformationSpeed * Time.deltaTime);
            if (dyingCounter <= 0)
            {
                if (deathEffect != null)
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {

            switch (currentState)
            {
                case EnemyState.idle:
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    waitCounter -= Time.deltaTime;
                    if (waitCounter <= 0)
                    {
                        currentState = EnemyState.patrolling;
                        NextDestinyPoint();
                    }
                    break;

                case EnemyState.patrolling:
                    yAux = rb.velocity.y;
                    moveDirection = routePoints[nextPoint].position - transform.position;
                    moveDirection.y = 0;
                    moveDirection.Normalize();
                    rb.velocity = moveDirection * moveSpeed;
                    rb.velocity = new Vector3(rb.velocity.x, yAux, rb.velocity.z);

                    if (Vector3.Distance(transform.position, routePoints[nextPoint].position) <= 0.1f)
                        NextDestinyPoint();
                    else
                        lookDirection = routePoints[nextPoint].position;

                    break;

                case EnemyState.chasing:
                    lookDirection = player.transform.position;
                    if (chaseDelayCounter > 0)
                        chaseDelayCounter -= Time.deltaTime;
                    else
                    {
                        yAux = rb.velocity.y;
                        moveDirection = player.transform.position - transform.position;
                        moveDirection.y = 0;
                        moveDirection.Normalize();
                        rb.velocity = moveDirection * chaseSpeed;
                        rb.velocity = new Vector3(rb.velocity.x, yAux, rb.velocity.z);
                    }
                    if (Vector3.Distance(player.transform.position, this.transform.position) > loseDistance)
                    {
                        currentState = EnemyState.returning;
                        returnCounter = returningDelay;
                    }
                    break;

                case EnemyState.returning:
                    returnCounter -= Time.deltaTime;
                    if (returnCounter <= 0)
                        currentState = EnemyState.patrolling;
                    break;

            }
        }

        if (Vector3.Distance(player.transform.position, this.transform.position) < chaseDistance && currentState != EnemyState.chasing)
        {
            currentState = EnemyState.chasing;
            rb.velocity = Vector3.up * jumpSpeed;
            chaseDelayCounter = chaseDelay;
        }

        lookDirection.y = transform.position.y;
        // transform.LookAt(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection - transform.position), rotationSpeed * Time.deltaTime);
    }

    public void NextDestinyPoint()
    {
        if (Random.Range(0f, 100f) < waitProbability)
        {
            waitCounter = waitDelay;
            currentState = EnemyState.idle;
        }
        else
        {

            if (nextPoint == routePoints.Length - 1)
                nextPoint = 0;
            else
                ++nextPoint;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && player.invulnerabilityCounter <= 0 && dyingCounter == 0)
        {
            MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
            player.invulnerabilityCounter = player.invulnerabilityDuration;
            chaseDelayCounter = chaseDelay;

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && player.invulnerabilityCounter <= 0 && dyingCounter == 0)
        {
            MainManager.Instance.playerData.IncreaseHealth(-1 * this.healthTaken);
            player.invulnerabilityCounter = player.invulnerabilityDuration;
            chaseDelayCounter = chaseDelay;

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player.Bounce();
            dyingCounter = dyingDelay;
        }

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