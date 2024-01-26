using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equipment _itemData)
    {
        if (_itemData == null)
            return;

        item.data = _itemData;

        itemImage.sprite = _itemData.icon;
        itemText.text = _itemData.itemName;

        if (itemText.text.Length > 12)
            itemText.fontSize = itemText.fontSize * .8f;
        else
            itemText.fontSize = 24;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
