// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class FollowerEnemyController : MonoBehaviour
// {
//     public float moveSpeed;
//     public float rotationSpeed;
//     public Transform[] routePoints;
//     private int nextPoint = 0;
//     public Rigidbody rb;
//     private Vector3 moveDirection;
//     private float yAux;
//     private PlayerController player;

//     private Vector3 lookDirection;

//     // Distância de perseguição
//     public float chaseDistance = 5f;

//     // Start is called before the first frame update
//     void Start()
//     {
//         player = FindObjectOfType<PlayerController>(); // Alterado de FindAnyObjectByType para FindObjectOfType
//         foreach (Transform routePoint in routePoints)
//             routePoint.parent = null;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         yAux = rb.velocity.y;

//         // Verificar a distância entre o jogador e o inimigo
//         float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

//         // Se estiver a menos de 5 unidades de distância, perseguir o jogador
//         if (distanceToPlayer < chaseDistance)
//         {
//             moveDirection = player.transform.position - transform.position;
//         }
//         else
//         {
//             moveDirection = routePoints[nextPoint].position - transform.position;
//         }

//         moveDirection.y = 0;
//         moveDirection.Normalize();
//         rb.velocity = moveDirection * moveSpeed;
//         rb.velocity = new Vector3(rb.velocity.x, yAux, rb.velocity.z);

//         if (distanceToPlayer >= chaseDistance && Vector3.Distance(transform.position, routePoints[nextPoint].position) <= 0.1f)
//         {
//             NextDestinyPoint();
//         }
//         else
//         {
//             lookDirection = distanceToPlayer < chaseDistance ? player.transform.position : routePoints[nextPoint].position;
//         }

//         lookDirection.y = transform.position.y;
//         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection - transform.position), rotationSpeed * Time.deltaTime);
//     }

//     public void NextDestinyPoint()
//     {
//         if (nextPoint == routePoints.Length - 1)
//             nextPoint = 0;
//         else
//             ++nextPoint;
//     }
// }
