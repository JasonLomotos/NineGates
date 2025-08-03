using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxSlots = 20;

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChangedCallback;

    public bool Add(InventoryItem item)
    {
        if (items.Count >= maxSlots)
            return false;

        items.Add(item);
        onInventoryChangedCallback?.Invoke();
        return true;
    }

    public void Remove(InventoryItem item)
    {
        items.Remove(item);
        onInventoryChangedCallback?.Invoke();
    }
}