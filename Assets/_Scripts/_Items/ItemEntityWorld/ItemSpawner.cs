using System.Collections.Generic;
using UnityEngine;
public class ItemSpawner : MyMonobehaviour
{
    [SerializeField] private ItemController itemPrefab;
    [SerializeField] private List<ItemController> itemList = new List<ItemController>();

    [SerializeField] private List<ItemSO> itemSOList = new List<ItemSO>();
    public GameObject itemHolder;






    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRes();
    }
    private void SpawnItem(Vector3 spawnPosition, ItemSO itemSO, int amount)
    {

        ItemController item = TakeFromPool();
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


    }
    ItemController TakeFromPool()
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
        foreach (var item in itemSOList)
        {
            Debug.Log(item.itemName);
        }
    }
    void SpawnRandomItem()
    {
        int randomIndex = Random.Range(0, itemSOList.Count);
        ItemSO randomItem = itemSOList[randomIndex];
        SpawnItem(PlayerEntity.Instance.transform.position, randomItem, 1);
    }
    #endregion

}