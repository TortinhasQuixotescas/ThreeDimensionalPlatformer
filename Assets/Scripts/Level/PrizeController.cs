using UnityEngine;

public class PrizeController : MonoBehaviour
{
    public int coinsGiven = 10;
    public int maximumIterations = 1;
    private DataItem_Int storage;

    private void Start()
    {
        this.storage = new DataItem_Int(0, this.maximumIterations);
        this.storage.IncreaseCurrentQuantity(this.maximumIterations);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!storage.IsEmpty())
        {
            if (collider.CompareTag("Player"))
            {
                MainManager.Instance.playerData.IncreaseCoins(this.coinsGiven);
                this.storage.IncreaseCurrentQuantity(-1);
            }
        }
    }

}
