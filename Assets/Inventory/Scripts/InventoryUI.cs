using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemsParent;
    public GameObject itemSlotPrefab;

    void Start()
    {
        inventory.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform child in itemsParent)
            Destroy(child.gameObject);

        foreach (var item in inventory.items)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemsParent);
            slot.GetComponentInChildren<Text>().text = item.itemName;
            slot.GetComponentInChildren<Image>().sprite = item.icon;
        }
    }
}