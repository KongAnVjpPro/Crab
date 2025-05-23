using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class ItemSpawner : MyMonobehaviour
{
    [SerializeField] private ItemController itemPrefab;
    [SerializeField] private List<ItemController> itemList = new List<ItemController>();

    [SerializeField] private List<ItemSO> itemSOList = new List<ItemSO>();
    public GameObject itemHolder;

    public Vector3 offsetDrop = new Vector3(0, 1, 0);
    [Header("Spawn Coin: ")]
    [SerializeField] List<Coin> coinList = new List<Coin>();
    [SerializeField] Coin coinPrefab;
    public GameObject coinHolder;



    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRes();
    }
    public void SpawnItem(Vector3 spawnPosition, ItemSO itemSO, int amount)
    {
        Vector3 randomX = new Vector3(Random.Range(-1, 1), 0, 0);
        ItemController item = TakeItemFromPool();
        if (item == null)
        {
            item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            item.SetSpawner(this);
            itemList.Add(item);
            item.transform.SetParent(itemHolder.transform);

        }
        item.SetItemData(itemSO, amount);
        item.transform.position = spawnPosition;

        item.Init();
        item.transform.DOMove(spawnPosition + offsetDrop + randomX, 1f);

    }
    public void SpawnCoin(Vector3 spawnPosition, int amount)
    {
        Vector3 randomX = new Vector3(Random.Range(-1, 1), 0, 0);
        Coin coin = TakeCoinFromPool();
        if (coin == null)
        {
            coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            coin.SetSpawner(this);
            coinList.Add(coin);
            coin.transform.SetParent(coinHolder.transform);
        }
        coin.transform.position = spawnPosition;
        coin.gameObject.SetActive(true);
        coin.transform.DOMove(spawnPosition + offsetDrop + randomX, 1f);
    }
    ItemController TakeItemFromPool()
    {
        foreach (var item in itemList)
        {
            if (item.gameObject.activeSelf == false)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }
        return null;
    }
    Coin TakeCoinFromPool()
    {
        foreach (var c in coinList)
        {
            if (c.gameObject.activeSelf == false)
            {
                // c.gameObject.SetActive(true);
                return c;
            }
        }
        return null;
    }




    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SpawnRandomItem();
        // }
    }
    #region  Test Spawn Item
    void LoadRes()
    {
        itemSOList = new List<ItemSO>(Resources.LoadAll<ItemSO>("ScriptableObject/Item/"));
        // foreach (var item in itemSOList)
        // {
        // Debug.Log(item.itemName);
        // }
    }
    void SpawnRandomItem()
    {
        int randomIndex = Random.Range(0, itemSOList.Count);
        ItemSO randomItem = itemSOList[randomIndex];
        SpawnItem(PlayerEntity.Instance.transform.position, randomItem, 1);
    }
    #endregion

}