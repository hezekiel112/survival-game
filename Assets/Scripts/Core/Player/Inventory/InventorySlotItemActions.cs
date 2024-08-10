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
        _itemTooltipGO.SetActive(false);

        _useItemButton.onClick.AddListener(() => {
            if ((_itemSlot.Stack - 1) <= 0) {
                PlayerInventory.Instance.UseItemFromSlot(_itemSlot);
                _itemTooltipGO.SetActive(false);
                _itemActionsPannel.SetActive(false);
                return;
            }

            PlayerInventory.Instance.UseItemFromSlot(_itemSlot);
        });
    }

    void DisplayTooltip(string text) {
        _itemTooltipGO.SetActive(true);

        _itemTooltipSB.Clear();
        _itemTooltipSB.Append(text);
        _itemTooltip.text = _itemTooltipSB.ToString();
    }

    public void ShowOrHidePannel() {
        if (_itemSlot.HasItem()) {
            _itemActionsPannel.SetActive(!_itemActionsPannel.activeSelf);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _itemSlot = PlayerInventory.Instance.InventorySlots.Where(x => x.SlotID == _slotID).FirstOrDefault();

        if (!_itemSlot.HasItem()) {
            return;
        }
        else {
            DisplayTooltip($"{_itemSlot.GetItem().ItemName} +{_itemSlot.GetItem().Bonus} {_itemSlot.GetItem().GetItemType()}\n{_itemSlot.Item.ItemDescription}");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        _itemTooltipGO.SetActive(false);
        _itemActionsPannel.SetActive(false);
    }
}