using UnityEngine;

public class PrizeController : MonoBehaviour
{
    public GameObject pickUpEffect;
    public int coinsGiven = 10;
    public int maximumIterations = 1;
    public bool vanishWhenEmpty = true;
    private DataItem_Int storage;

    private void Start()
    {
        this.storage = new DataItem_Int(0, this.maximumIterations);
        this.storage.IncreaseCurrentQuantity(this.maximumIterations);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (!this.storage.IsEmpty())
            {
                if (pickUpEffect != null)
                    Instantiate(pickUpEffect, transform.position, Quaternion.identity);
                MainManager.Instance.playerData.IncreaseCoins(this.coinsGiven);
                this.storage.IncreaseCurrentQuantity(-1);
                if (this.vanishWhenEmpty && this.storage.IsEmpty())
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

}
