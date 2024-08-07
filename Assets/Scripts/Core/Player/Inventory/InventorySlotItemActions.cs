using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItemActions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] int _slotID;
    [SerializeField] ItemSlot _itemSlot = null;
    [SerializeField] GameObject _itemTooltipGO;
    [SerializeField] TextMeshProUGUI _itemTooltip;
    [SerializeField]
    Button _useItemButton,
        _showActionPannel;

    [SerializeField] GameObject _itemActionsPannel;

    readonly StringBuilder _itemTooltipSB = new();

    private void Start() {
        _useItemButton.onClick.RemoveAllListeners();

        _itemTooltipGO.SetActive(false);

        _useItemButton.onClick.AddListener(() => {
            PlayerInventory.Instance.UseItemFromSlot(_itemSlot);
        });

        _showActionPannel.onClick.AddListener(() => {
            _itemSlot = PlayerInventory.Instance.InventorySlots.Where(x => x.SlotID == _slotID).FirstOrDefault();
            ShowOrHidePannel();
        });
    }

    public void ShowOrHidePannel() {
        if (PlayerInventory.Instance.InventorySlots[_slotID].HasItem()) {
            _itemActionsPannel.SetActive(!_itemActionsPannel.activeSelf);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!_itemSlot.HasItem())
            return;
        
        _itemTooltipGO.SetActive(true);

        _itemTooltipSB.Clear();
        _itemTooltipSB.Append($"{_itemSlot.GetItem().ItemName} +{_itemSlot.GetItem().Bonus}\n{_itemSlot.Item.ItemDescription}");
        _itemTooltip.text = _itemTooltipSB.ToString();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!_itemSlot.HasItem())
            return;

        _itemTooltipGO.SetActive(false);
    }
}