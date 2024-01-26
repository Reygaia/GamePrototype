using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialIamge;

    public void SetupCraftWindow(ItemData_Equipment _itemdata)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialIamge.Length; i++)
        {
            materialIamge[i].color = Color.clear;
            materialIamge[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _itemdata.craftingMaterials.Count; i++)
        {
            materialIamge[i].sprite = _itemdata.craftingMaterials[i].data.icon;
            materialIamge[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialIamge[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _itemdata.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _itemdata.icon;
        itemName.text = _itemdata.name;
        itemDescription.text = _itemdata.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_itemdata,_itemdata.craftingMaterials));
    }
}
