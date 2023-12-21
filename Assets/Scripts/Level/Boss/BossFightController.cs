using System.Collections;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    // Objects
    public GameObject bossObject;
    public GameObject spawnPointsContainer;
    private Transform[] spawnPoints;

    // Spawn
    public GameObject appearingEffect;
    public float maxAliveTime = 10;
    public float spawnDelay;
    public float animationOffset;
    private Vector3 aux;

    // Shot
    public GameObject shot;
    public GameObject appearingShotEffect;
    public Transform shotOrigin;
    public float shotDelay;
    private float shotCounter;

    private void Start()
    {
        this.spawnPoints = this.spawnPointsContainer.GetComponentsInChildren<Transform>();
        MainManager.Instance.currentLevel.StartBossFight();
        AppearingAnimation();
        shotCounter = shotDelay;
    }

    private void Update()
    {
        if (bossObject.activeSelf)
        {
            bossObject.transform.LookAt(MainManager.Instance.playerController.transform);
            bossObject.transform.rotation = Quaternion.Euler(0f, bossObject.transform.rotation.eulerAngles.y, 0f);

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                Instantiate(shot, shotOrigin.position, shotOrigin.rotation);
                if (appearingShotEffect != null)
                    Instantiate(appearingShotEffect, shotOrigin.position, shotOrigin.rotation);

                shotDelay = Random.Range(0.5f, 1.5f) / (6 - MainManager.Instance.currentLevel.GetBossHealth());
                shotCounter = shotDelay;
            }

            maxAliveTime -= Time.deltaTime;
            if (maxAliveTime <= 0)
                InstantSpawn();
        }
    }

    public void DamageBoss()
    {
        MainManager.Instance.currentLevel.DecreaseBossHealth();
        if (MainManager.Instance.currentLevel.GetBossHealth() <= 0)
            StartCoroutine(EndBattle());
        else
            StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        bossObject.SetActive(false);
        AppearingAnimation();
        yield return new WaitForSeconds(this.spawnDelay);
        this.Spawn();
    }

    private void InstantSpawn()
    {
        this.bossObject.SetActive(false);
        this.AppearingAnimation();
        this.Spawn();
    }

    private void Spawn()
    {
        int nextSpawnPointIndex = Random.Range(0, this.spawnPoints.Length);
        this.bossObject.transform.localPosition = spawnPoints[nextSpawnPointIndex].localPosition;
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
        MainManager.Instance.currentLevel.SetFinalCheckPointActive(true);
        gameObject.SetActive(false);
    }

}
