using System.Collections;
using System.Collections.Generic;
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
