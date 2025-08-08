using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite itemIcon = null;

    [Header("Stacking")]
    public bool isStackable = true;
    public int maxStackSize = 16;
}