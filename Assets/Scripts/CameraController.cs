using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalDistanceToPlayer = 5;
    public float horizontalDistanceToPlayer = 6;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.uniqueInstance.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + verticalDistanceToPlayer, player.position.z - horizontalDistanceToPlayer);
    }
}
