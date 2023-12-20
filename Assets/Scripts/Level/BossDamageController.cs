using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageController : MonoBehaviour
{
    public BossFightController boss;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            boss.DamageBoss();
            MainManager.Instance.playerController.Bounce();
        }
    }
}
