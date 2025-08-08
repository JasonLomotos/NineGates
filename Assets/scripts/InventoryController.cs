using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Add this to use TextMeshPro

// NEW: A class to define a slot in our inventory
[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    public InventorySlot(ItemData item, int amount)
    {
        itemData = item;
        quantity = amount;
    }

    public void AddToStack(int amount)
    {
        quantity += amount;
    }
}

public class InventoryController : MonoBehaviour
{
    [Header("Input")]
    public KeyCode inventoryKey = KeyCode.Tab;
    public KeyCode pickupKey = KeyCode.E;

    [Header("UI")]
    public GameObject inventoryPanel;
    public Transform slotsParent;

    [Header("Inventory Data")]
    public int inventorySize = 16;
    // UPDATED: The inventory is now a list of InventorySlots
    public List<InventorySlot> slots = new List<InventorySlot>();

    // Internal variables
    private ItemPickup currentPickupItem;

    void Start()
    {
        inventoryPanel.SetActive(false);
        // Initialize the slots list to have a fixed size
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot(null, 0));
        }
        UpdateInventoryUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        if (currentPickupItem != null && Input.GetKeyDown(pickupKey))
        {
            AddItem(currentPickupItem.itemData);
            Destroy(currentPickupItem.gameObject);
            currentPickupItem = null;
        }
    }

    // --- UPDATED ADD ITEM LOGIC ---
    public void AddItem(ItemData itemToAdd)
    {
        // Check if the item is stackable and if a stack already exists
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                // If the slot contains the same item and the stack is not full
                if (slot.itemData == itemToAdd && slot.quantity < itemToAdd.maxStackSize)
                {
                    slot.AddToStack(1);
                    UpdateInventoryUI();
                    return; // Item added to existing stack, so we're done.
                }
            }
        }

        // If no existing stack was found (or item is not stackable), find an empty slot
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemData == null) // Check for an empty slot
            {
                slots[i] = new InventorySlot(itemToAdd, 1);
                UpdateInventoryUI();
                return; // Item added to empty slot, so we're done.
            }
        }

        Debug.Log("Inventory is full!");
    }

    // --- UPDATED UI LOGIC ---
    private void UpdateInventoryUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Transform slot = slotsParent.GetChild(i);
            Image itemIcon = slot.GetComponentInChildren<Image>(true); // Find the child icon
            TextMeshProUGUI quantityText = slot.GetComponentInChildren<TextMeshProUGUI>();

            if (slots[i].itemData != null)
            {
                // This slot has an item, so update the icon and quantity
                itemIcon.sprite = slots[i].itemData.itemIcon;
                itemIcon.gameObject.SetActive(true);

                // Show quantity text only if stack is greater than 1
                if (slots[i].quantity > 1)
                {
                    quantityText.text = slots[i].quantity.ToString();
                    quantityText.gameObject.SetActive(true);
                }
                else
                {
                    quantityText.gameObject.SetActive(false);
                }
            }
            else
            {
                // This slot is empty
                itemIcon.gameObject.SetActive(false);
                quantityText.gameObject.SetActive(false);
            }
        }
    }

    // --- Pickup Detection (no changes needed here) ---
    private void OnTriggerEnter(Collider other)
    {
        ItemPickup pickup = other.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            currentPickupItem = pickup;
            Debug.Log("Near item: " + pickup.itemData.itemName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ItemPickup>() == currentPickupItem)
        {
            currentPickupItem = null;
        }
    }
}