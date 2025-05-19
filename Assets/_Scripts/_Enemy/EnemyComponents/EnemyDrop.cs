using UnityEngine;
public class EnemyDrop : EnemyComponent
{
    [Header("All the function can use for both enemy and object, ignore enemy ctrl and bindFunction to object")]
    [SerializeField] float dropRateItem = 0;
    [SerializeField] ItemSO itemDropAble;

    [SerializeField] int amountRange = 1;
    [Header("Coin: ")]
    [SerializeField] int coinAmount = 5;
    public void Drop()
    {
        DropCoin();
        if (itemDropAble == null) return;
        if (amountRange <= 0) return;
        // float rd = Random.Range(0, 1f);
        // if (rd < dropRateItem)
        if (Random.value >= dropRateItem) return;
        for (int i = 0; i < amountRange; i++)
        {
            GameController.Instance.itemSpawner.SpawnItem(transform.position, itemDropAble, 1);
        }

    }
    public void DropCoin()
    {
        int coinRange = Mathf.FloorToInt(Random.Range(1, coinAmount));
        for (int i = 0; i < coinRange; i++)
        {
            GameController.Instance.itemSpawner.SpawnCoin(transform.position, 1);
        }
    }
}