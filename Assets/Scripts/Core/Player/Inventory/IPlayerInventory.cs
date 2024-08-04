public interface IPlayerInventory {
    ItemSlot FindFirstFreeSlot();
    bool GetSlotWithItem(PlayerItem item, out ItemSlot slot);
    void UseItemFromSlot(ItemSlot slot);
}