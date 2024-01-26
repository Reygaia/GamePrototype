using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemData.ItemType == ItemType.Equiment)
        {
            rb.velocity = new Vector2(0, 6);
            return;
        }

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
