using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public CharacterController player;
    private List<GameObject> visitedCheckPoints;

    void Start()
    {
        player = FindObjectOfType<CharacterController>();
        visitedCheckPoints = new List<GameObject>();

        Transform tenthChild = transform.GetChild(7);
        tenthChild.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !visitedCheckPoints.Contains(this.gameObject))
        {
            visitedCheckPoints.Add(this.gameObject);
            this.gameObject.tag = "CheckPoint";
            Transform tenthChild = transform.GetChild(7);
            tenthChild.gameObject.SetActive(true);
        }
    }
}
