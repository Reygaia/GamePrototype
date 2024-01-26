using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrops;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;


    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for (int i = 0; i < possibleItemDrops; i++)
        {
            if (dropList.Count <= 0)
                return;
            else if (dropList.Count != 0)
            {
                ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
                dropList.Remove(randomItem);
                DropItem(randomItem);
            }

        }

    }

    protected void DropItem(ItemData _itemdata)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(16, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemdata, randomVelocity);
    }
}
