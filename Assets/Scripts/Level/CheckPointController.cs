using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public CharacterController player;

    void Start()
    {
        player = FindObjectOfType<CharacterController>();

        Transform tenthChild = transform.GetChild(7);
        tenthChild.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool hasBeenVisited = MainManager.Instance.currentLevel.SetCheckPointAsVisited(this.gameObject);
            if (!hasBeenVisited)
            {
                this.SetAsShiny();
            }
        }
    }

    public void SetAsShiny()
    {
        Transform tenthChild = transform.GetChild(7);
        tenthChild.gameObject.SetActive(true);
    }
}
