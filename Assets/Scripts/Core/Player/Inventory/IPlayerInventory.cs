public interface IPlayerInventory {
    ItemSlot FindFirstFreeSlot();
    bool GetSlotWithItem(IPlayerItem item, out ItemSlot slot);
    void UseItemFromSlot(ItemSlot slot);
}