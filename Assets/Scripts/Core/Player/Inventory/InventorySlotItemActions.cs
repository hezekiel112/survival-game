using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItemActions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [SerializeField] int _slotID;
    [SerializeField] bool _isSlotBarSlot;
    [Space]
    [SerializeField] ItemSlot _itemSlot = null;
    [SerializeField] GameObject _itemTooltipGO;
    [SerializeField] TextMeshProUGUI _itemTooltip;
    [SerializeField]
    Button _useItemButton,
        _showActionPannel;

    [SerializeField] GameObject _itemActionsPannel;

    readonly StringBuilder _itemTooltipSB = new();

    Sprite itemIcon = null;

    private void Start() {
        _itemTooltipGO.SetActive(false);

        if (!_isSlotBarSlot) {
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
    }

    void DisplayTooltip(string text) {
        _itemTooltipGO.SetActive(true);
        this._itemTooltip.transform.parent.gameObject.SetActive(true);

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
        _itemSlot = PlayerInventory.Instance.FindInventorySlotWithID(_slotID);


        if (!eventData.IsPointerMoving() && _itemSlot.HasItem()) {
            DisplayTooltip($"{_itemSlot.GetItem().ItemName} +{_itemSlot.GetItem().Bonus} {_itemSlot.GetItem().FormatItemType()}\n{_itemSlot.Item.ItemDescription}");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        _itemTooltipGO.SetActive(false);
        _itemActionsPannel.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!_itemSlot.HasItem())
            return;

        itemIcon = _itemSlot.Item.GetItemIcon();

        GameObject tempItemObject = new("temp item icon");

        tempItemObject.transform.SetParent(transform.root);

        Image tempItemObjectImage = tempItemObject.AddComponent<Image>().GetComponent<Image>();

        tempItemObjectImage.sprite = itemIcon;
        tempItemObjectImage.rectTransform.sizeDelta = new Vector2(60, 60);
        tempItemObjectImage.raycastTarget = false;

        // disable tooltip and stack count
        this._itemTooltip.transform.parent.gameObject.SetActive(false);
        this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!_itemSlot.HasItem())
            return;

        transform.root.Find("temp item icon").position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!eventData.pointerCurrentRaycast.isValid || eventData.pointerCurrentRaycast.gameObject.name.Equals("Use")) {
            this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(true);

            Destroy(transform.root.Find("temp item icon").gameObject);

            return;
        }

        print(eventData.pointerCurrentRaycast.gameObject.name);

        // si un slot est trouvé, swap de celui-ci vers l'autre
        if (PlayerInventory.Instance.InventorySlotsCollection.TryGetValue(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, out var inventorySlot)) {

            if (inventorySlot == this._itemSlot) {
                this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(true);

                Destroy(transform.root.Find("temp item icon").gameObject);
                return;
            }

            bool hasInventorySlotItem = inventorySlot.HasItem();

            if (hasInventorySlotItem && inventorySlot.GetItem().CanBeStacked) {
                if (PlayerInventory.Instance.CombineStackToSlot(ref this._itemSlot, inventorySlot.SlotID)) {
                    this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(true);

                    Destroy(transform.root.Find("temp item icon").gameObject);

                    return;
                } else {
                    SwapSlot(inventorySlot, false);
                    return;
                }
            } else if (hasInventorySlotItem && !inventorySlot.GetItem().CanBeStacked) {
                SwapSlot(inventorySlot, false);
                return;
            } else {
                if (!hasInventorySlotItem) {
                    SwapSlot(inventorySlot, true);
                    return;
                }
            }
        } else if (inventorySlot == null) {
            if (PlayerInventory.Instance.SlotBarsSlotsCollection.TryGetValue(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, out var slotBarSlot)) {
                if (slotBarSlot == this._itemSlot) {
                    this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(true);

                    Destroy(transform.root.Find("temp item icon").gameObject);
                    return;
                }

                PlayerInventory.Instance.TransferInventorySlotToSlotBar(ref this._itemSlot, slotBarSlot.SlotID);


                return;
            }
        }
    }

    private void SwapSlot(ItemSlot inventorySlot, bool isBlankSlot) {
        if (isBlankSlot) {
            PlayerInventory.Instance.SwapSlot(ref _itemSlot, inventorySlot.SlotID);
            this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(true);

            Destroy(transform.root.Find("temp item icon").gameObject);
            return;
        }

        PlayerInventory.Instance.SwapSlot(ref _itemSlot, inventorySlot.SlotID, false);
        this.transform.GetChild(0).Find("Inventory Slot Item Stack").gameObject.SetActive(true);

        Destroy(transform.root.Find("temp item icon").gameObject);
    }
}
