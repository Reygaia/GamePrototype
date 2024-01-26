using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player Drop")]
    [SerializeField] private float chanceToLoseItem;
    [SerializeField] private float chanceToLostMats;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> matsToDrop = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLoseItem)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0,100) <= chanceToLostMats)
            {
                DropItem(item.data);
                matsToDrop.Add(item);
            }
        }

        for (int i = 0; i < matsToDrop.Count; i++)
        {
            inventory.RemoveItem(matsToDrop[i].data);
        }
    }
}
