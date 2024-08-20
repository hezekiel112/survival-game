using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IPlayerInventory
{
    [SerializeField] PlayerVitals _playerVitals;

    [Header("Hotbar :")]
    public ItemSlot[] SlotBarSlots;

    [Header("Inventory :")]
    public ItemSlot[] InventorySlots;
    public GameObject[] InventorySlotsGameObjects;

    public Dictionary<GameObject, ItemSlot> InventorySlotsCollection = new();

    public static PlayerInventory Instance {
        get; set;
    }

    /*readonly KeyCode[] _hotBarKeys = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
    };*/

    private void OnEnable() {
        Destroy(Instance);
        Instance = this;
    }

    public ItemSlot SwapSlot(ref ItemSlot slot, int newSlotID) {
        var newSlot = FindInventorySlotWithID(newSlotID);

        newSlot.AddItemToSlot(ItemManager.Instance.Items[0], slot.Stack);

        slot.RemoveItemFromSlot();

        return FindInventorySlotWithID(newSlotID);
    }

    public ItemSlot SwapSlot(ref ItemSlot slot, int newSlotID, bool removeSlotItems) {
        ItemSlot newSlot = FindInventorySlotWithID(newSlotID);

        if (!newSlot.HasItem()) {
            if (removeSlotItems)
                slot.RemoveItemFromSlot();

            newSlot.AddItemToSlot(ItemManager.Instance.Items[0], slot.Stack);

            slot.RemoveItemFromSlot();
        } 
        else if (newSlot.HasItem()) {
            // the targeted slot has item, swap them
            if (slot.GetItem() == newSlot.GetItem()) {
                (slot.Stack, newSlot.Stack) = (newSlot.Stack, slot.Stack);

                PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);
                PlayerHUD.OnItemAdded(newSlot.SlotID, newSlot.Stack);
            }
        }
        return FindInventorySlotWithID(newSlotID);
    }

    private void Start() {
        for (int i = 0; i < InventorySlotsGameObjects.Length; i++) {
            InventorySlotsCollection.Add(InventorySlotsGameObjects[i], InventorySlots[i]);
        }
    }

    /*    private void Update() {
            for (int i = 0; i < _hotBarKeys.Length; i++) {
                if (Input.GetKeyDown(_hotBarKeys[i])) {
                    if (_playerInventoryCells.InventorySlots[i].HasItem()) {
                        UseItemFromSlot(_playerInventoryCells.InventorySlots[i]);

                        return;
                    }
                }

            }
        }*/

    public void UseItemFromSlot(ItemSlot slot) {
        IPlayerItem itemFromSlot = ItemManager.Instance.Items[slot.Item.ItemID];

        Debug.Log(slot.GetItem().ItemName);

        if ((slot.Stack - 1) == 0) {
            itemFromSlot.UseItem(_playerVitals);
            slot.RemoveItemFromSlot();
            return;
        }

        slot.Stack -= 1;
        itemFromSlot.UseItem(_playerVitals);

        PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);
    }

    /// <summary>
    /// Return true if an ItemSlot contains the following wanted item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool GetSlotWithItem(IPlayerItem item, out ItemSlot slot) {
        for (int i = 0; i < InventorySlots.Length; i++) {
            if (FindInventorySlotWithID(i).Stack < item.Item.MaxStackSize) {
                if (FindInventorySlotWithID(i).HasItem() && FindInventorySlotWithID(i).GetItem().ItemID == item.Item.ItemID) {
                    slot = FindInventorySlotWithID(i);
                    return true;
                }
            }
        }

        slot = null;
        return false;
    }

    public bool GetSlotWithItem(IPlayerItem item) {
        for (int i = 0; i < InventorySlots.Length; i++) {
            if (FindInventorySlotWithID(i).Stack < item.Item.MaxStackSize) {
                return FindInventorySlotWithID(i).GetItem().ItemID == item.Item.ItemID;
            }
        }

        return false;
    }
    /// <summary>
    /// Return the first free ItemSlot found from the slots inside the inventory of the player
    /// </summary>
    /// <returns></returns>
    public ItemSlot FindFirstFreeSlot() {
        for (int i = 0; i < InventorySlots.Length; i++) {
            if (!FindInventorySlotWithID(i).HasItem())
                return FindInventorySlotWithID(i);
        }

        return null;
    }

    public ItemSlot FindInventorySlotWithID(int slotID) {
        return InventorySlots[slotID] ?? null;
    }
}
