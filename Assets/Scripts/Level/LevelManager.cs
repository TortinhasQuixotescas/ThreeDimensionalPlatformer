using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager uniqueInstance;
    public bool respawning;
    public float respawnDelay = 0.5f;
    public CharacterController player;
    private GameObject[] checkPoints;
    GameObject lastCheckPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        uniqueInstance = this;
        player = FindAnyObjectByType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawn()
    {
        if (!respawning)
        {
            respawning = true;
            StartCoroutine(RespawnCoroutine());
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        lastCheckPoint = GetLastCheckPoint();

        player.gameObject.SetActive(false);
        UIController.uniqueInstance.FadeOut();
        yield return new WaitForSeconds(respawnDelay);

        player.transform.position = lastCheckPoint.transform.position;

        player.gameObject.SetActive(true);
        UIController.uniqueInstance.FadeIn();
        respawning = false;
    }

    private GameObject GetLastCheckPoint()
    {
        if (checkPoints.Length > 0)
            return checkPoints[checkPoints.Length - 1];
        else
            return null;
    }


}
