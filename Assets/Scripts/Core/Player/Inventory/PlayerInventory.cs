using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IPlayerInventory
{
    [SerializeField] PlayerVitals _playerVitals;

    [Header("Hotbar :")]
    public ItemSlot[] SlotBarSlots;
    public GameObject[] SlotBarSlotsGameObjects;

    [Header("Inventory :")]
    public ItemSlot[] InventorySlots;
    public GameObject[] InventorySlotsGameObjects;

    public Dictionary<GameObject, ItemSlot> InventorySlotsCollection = new();
    public Dictionary<GameObject, ItemSlot> SlotBarsSlotsCollection = new();

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

    private void Start() {
        for (int i = 0; i < InventorySlotsGameObjects.Length; i++) {
            InventorySlotsCollection.Add(InventorySlotsGameObjects[i], InventorySlots[i]);
        }

        for (int i = 0; i < SlotBarSlotsGameObjects.Length; i++) {
            SlotBarsSlotsCollection.Add(SlotBarSlotsGameObjects[i], SlotBarSlots[i]);
        }
    }

    /// <summary>
    /// This function is used to add slot stack by other slot
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="slotIdToCombineStack"></param>
    /// <returns></returns>

    public bool CombineStackToSlot(ref ItemSlot slot, int slotIdToCombineStack) {
        var slotToCombine = FindInventorySlotWithID(slotIdToCombineStack);

        if (slot.GetItem().ItemID.Equals(slotToCombine.GetItem().ItemID)) {
            // Calculate the total stack
            int totalStack = slotToCombine.Stack + slot.Stack;

            // Check if the total stack exceeds the maximum
            if (totalStack > slotToCombine.GetItem().MaxStackSize) {
                // Fill the target slot to its maximum
                slotToCombine.Stack = slotToCombine.GetItem().MaxStackSize;

                // Calculate the remaining stack and update the source slot
                slot.Stack = totalStack - slotToCombine.GetItem().MaxStackSize;
            } else {
                // Combine the stacks without exceeding the maximum
                slotToCombine.Stack = totalStack;
                slot.RemoveItemFromSlot();
            }

            // Update the UI
            PlayerHUD.OnItemAdded(slotToCombine.SlotID, slotToCombine.Stack);
            PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);

            return true;
        }

        return false;

    }

    /// <summary>
    /// This function is used to swap a slot a cell position to another cell position
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="newSlotID"></param>
    /// <returns></returns>
    public ItemSlot SwapSlot(ref ItemSlot slot, int newSlotID) {
        var newSlot = FindInventorySlotWithID(newSlotID);

        newSlot.AddItemToSlot(ItemManager.Instance.Items[0], slot.Stack);

        slot.RemoveItemFromSlot();

        return FindInventorySlotWithID(newSlotID);
    }

    /// <summary>
    /// This function is used to transfer a inventory cell to a slotbar inventory
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="newSlotBarSlotID"></param>
    /// <returns></returns>
    public ItemSlot TransferInventorySlotToSlotBar(ref ItemSlot slot, int newSlotBarSlotID) {
        var newSlot = FindSlotbarSlotWithID(newSlotBarSlotID);

        if (!newSlot.HasItem()) {
            newSlot.AddItemToSlot(ItemManager.Instance.Items[0], slot.Stack);
            slot.RemoveItemFromSlot();

            PlayerHUD.OnItemAdded(newSlot.SlotID, newSlot.Stack);
        }

        return newSlot;
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
            if (slot.GetItem().Equals(newSlot.GetItem())) {
                (slot.Stack, newSlot.Stack) = (newSlot.Stack, slot.Stack);

                PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);
                PlayerHUD.OnItemAdded(newSlot.SlotID, newSlot.Stack);
            } else
                return newSlot;
        }
        return newSlot;
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

    /// <summary>
    /// This function is used to use an item from inventory or slotbar
    /// </summary>
    /// <param name="slot"></param>
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

    public ItemSlot FindSlotbarSlotWithID(int hotBarSlotID) {
        return SlotBarSlots[hotBarSlotID] ?? null;
    }

    public ItemSlot FindInventorySlotWithID(int slotID) {
        return InventorySlots[slotID] ?? null;
    }
}
