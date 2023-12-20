// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PatrollerEnemyControler : MonoBehaviour
// {
//     public float moveSpeed;
//     public Transform[] routePoints;
//     private int nextPoint = 0;
//     public Rigidbody rb;
//     private Vector3 moveDirection;

//     private float yAux;

//     // Start is called before the first frame update
//     void Start()
//     {
//         foreach (Transform routePoint in routePoints)
//             routePoint.parent = null;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         yAux = rb.velocity.y;
//         moveDirection = routePoints[nextPoint].position - transform.position;
//         moveDirection.y = 0;
//         moveDirection.Normalize();
//         rb.velocity = moveDirection * moveSpeed;
//         rb.velocity = new Vector3(rb.velocity.x, yAux, rb.velocity.z);

//         if (Vector3.Distance(transform.position, routePoints[nextPoint].position) <= 0.1f)
//             NextDestinyPoint();

//         transform.LookAt(routePoints[nextPoint].position);
//     }

//     public void NextDestinyPoint()
//     {
//         if (nextPoint == routePoints.Length - 1)
//             nextPoint = 0;
//         else
//             ++nextPoint;
//     }
// }
