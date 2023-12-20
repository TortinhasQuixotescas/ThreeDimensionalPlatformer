using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Transform[] spawnPoints;
    public GameObject bossObject;
    public float spawnDelay;
    private int nextPoint;
    public float maxAliveTime = 10;
    private int lastSpawn;
    private Vector3 aux;
    public GameObject appearingEffect;
    public float animationOffset;
    public GameObject shot;
    public GameObject appearingShotEffect;
    public Transform shotOrigin;
    public float shotDelay;
    private float shotCounter;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        lastSpawn = 0;
        AppearingAnimation();
        shotCounter = shotDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossObject.activeSelf)
        {
            bossObject.transform.LookAt(PlayerController.uniqueInstance.transform);
            bossObject.transform.rotation = Quaternion.Euler(0f, bossObject.transform.rotation.eulerAngles.y, 0f);

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                Instantiate(shot, shotOrigin.position, shotOrigin.rotation);
                if (appearingShotEffect != null)
                    Instantiate(appearingShotEffect, shotOrigin.position, shotOrigin.rotation);

                shotDelay = Random.Range(0.5f, 1.5f) / (6 - currentHealth);
                shotCounter = shotDelay;
            }


            maxAliveTime -= Time.deltaTime;
            if (maxAliveTime <= 0)
                NoDelaySpawn();
        }

    }

    public void DamageBoss()
    {
        --currentHealth;
        if (currentHealth <= 0)
            StartCoroutine(EndBattle());
        else
            StartCoroutine(Spawn());

    }

    private IEnumerator Spawn()
    {
        bossObject.SetActive(false);

        AppearingAnimation();
        yield return new WaitForSeconds(spawnDelay);

        do
        {
            nextPoint = Random.Range(0, spawnPoints.Length);
        }
        while (nextPoint == lastSpawn);
        lastSpawn = nextPoint;
        bossObject.transform.position = spawnPoints[nextPoint].position;

        AppearingAnimation();
        bossObject.SetActive(true);
        maxAliveTime = 10;
    }

    private void NoDelaySpawn()
    {
        bossObject.SetActive(false);

        AppearingAnimation();
        do
        {
            nextPoint = Random.Range(0, spawnPoints.Length);
        }
        while (nextPoint == lastSpawn);
        lastSpawn = nextPoint;
        bossObject.transform.position = spawnPoints[nextPoint].position;
        AppearingAnimation();
        bossObject.SetActive(true);
        maxAliveTime = 10;
    }


    private void AppearingAnimation()
    {
        if (appearingEffect != null)
        {
            aux = new Vector3(bossObject.transform.position.x, bossObject.transform.position.y + animationOffset, bossObject.transform.position.z);
            Instantiate(appearingEffect, aux, bossObject.transform.rotation);
        }
    }

    private IEnumerator EndBattle()
    {
        bossObject.SetActive(false);
        AppearingAnimation();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
