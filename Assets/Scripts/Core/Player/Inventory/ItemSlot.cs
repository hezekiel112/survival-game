﻿using System;
using UnityEngine;

/// <summary>
/// This script handle the inventory slot functionnality
/// </summary>
[Serializable]
public class ItemSlot {
    public ScriptableItem Item => _item;
    public int Stack {
        get => _stack;
        set => _stack = value;
    }
    public int SlotID => _slotID;

    [SerializeField] int _slotID;
    [SerializeField] ScriptableItem _item;
    [SerializeField] int _stack;

    public bool HasItem() {
        return _item != null;
    }

    public bool AddItemToSlot(PlayerItem item) {
        PlayerHUD.OnItemPickup.Invoke(SlotID, item.Item.ItemIcon);

        _stack++;
        _item = item.Item;

        PlayerHUD.OnItemAdded(SlotID, _stack);
        return true;
    }

    public ScriptableItem GetItem() { return _item; }

    public bool RemoveItemFromSlot() {
        PlayerHUD.OnItemUsed.Invoke(SlotID);

        _stack = 0;
        _item = null;

        PlayerHUD.OnItemAdded(SlotID, _stack);
        return true;
    }
}