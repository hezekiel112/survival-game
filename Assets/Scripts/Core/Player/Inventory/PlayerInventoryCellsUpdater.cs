using UnityEngine;

public class PlayerInventoryCellsUpdater : MonoBehaviour
{
    [Header("Inventory Slots :")]
    [SerializeField] ItemSlot[] _inventorySlots;
    public ItemSlot[] InventorySlots => _inventorySlots;
}