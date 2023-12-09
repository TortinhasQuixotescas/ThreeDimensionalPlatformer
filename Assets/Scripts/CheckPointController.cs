using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public CharacterController player;
    private List<GameObject> checkPoints;

    void Start()
    {
        player = FindObjectOfType<CharacterController>();
        checkPoints = new List<GameObject>();

        // Desativa o décimo filho inicialmente
        Transform tenthChild = transform.GetChild(7); // Assumindo que o décimo filho está na posição 7 da lista de filhos
        tenthChild.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkPoints.Contains(this.gameObject))
        {
            checkPoints.Add(this.gameObject);
            this.gameObject.tag = "CheckPoint";

            // Ativa o décimo filho quando o jogador colidir
            Transform tenthChild = transform.GetChild(7); // Assumindo que o décimo filho está na posição 9 da lista de filhos
            tenthChild.gameObject.SetActive(true);
        }
    }
}
