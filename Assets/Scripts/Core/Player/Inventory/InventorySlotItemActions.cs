using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotItemActions : MonoBehaviour
{
    [SerializeField] int _slotID;
    
    [SerializeField]
    Button _useItemButton,
        _showActionPannel;

    [SerializeField] GameObject _itemActionsPannel;

    private void Start() {
        _useItemButton.onClick.RemoveAllListeners();

        _useItemButton.onClick.AddListener(() => {
            ItemSlot thisSlot = PlayerInventory.Instance.InventorySlots.Where(x => x.SlotID == _slotID).FirstOrDefault();
            PlayerInventory.Instance.UseItemFromSlot(thisSlot);
        });

        _showActionPannel.onClick.AddListener(() => {
            ShowOrHidePannel();
        });
    }

    public void ShowOrHidePannel() {
        if (PlayerInventory.Instance.InventorySlots[_slotID].HasItem()) {
            _itemActionsPannel.SetActive(!_itemActionsPannel.activeSelf);
        }
    }
}