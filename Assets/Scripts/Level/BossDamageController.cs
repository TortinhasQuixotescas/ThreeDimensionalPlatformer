using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageController : MonoBehaviour
{
    public BossFightController boss;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }
    
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            boss.DamageBoss();
            player.Bounce();
        }
    }
}
