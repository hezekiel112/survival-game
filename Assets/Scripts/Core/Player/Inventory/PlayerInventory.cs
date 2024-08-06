using System;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IPlayerInventory
{
    [SerializeField] PlayerVitals _playerVitals;

    [Header("Hotbar :")]
    public ItemSlot[] SlotBarSlots;

    [Header("Inventory :")]
    public ItemSlot[] InventorySlots;

    /*readonly KeyCode[] _hotBarKeys = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
    };*/

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
        PlayerItem itemFromSlot = ItemManager.Instance.GetItem(slot.Item.ItemID);

        if ((slot.Stack - 1) == 0) {
            itemFromSlot.UseItem(_playerVitals);
            slot.RemoveItemFromSlot();
            return;
        }

        slot.Stack--;
        itemFromSlot.UseItem(_playerVitals);
       
        PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);
    }

    /// <summary>
    /// Return true if an ItemSlot contains the following wanted item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool GetSlotWithItem(PlayerItem item, out ItemSlot slot) {
        for (int i = 0; i < InventorySlots.Length; i++) {
            if (InventorySlots[i].Stack < item.Item.MaxStackSize) {
                if (InventorySlots[i].HasItem() && InventorySlots[i].GetItem().ItemID == item.Item.ItemID) {
                    slot = InventorySlots[i];
                    return true;
                }
            }
        }

        slot = null;
        return false;
    }


    public bool GetSlotWithItem(PlayerItem item) {
        for (int i = 0; i < InventorySlots.Length; i++) {
            if (InventorySlots[i].Stack < item.Item.MaxStackSize) {
                return InventorySlots[i].GetItem().ItemID == item.Item.ItemID;
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
            if (!InventorySlots[i].HasItem())
                return InventorySlots[i];
        }

        return null;
    }
}
