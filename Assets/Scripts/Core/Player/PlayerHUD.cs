using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _itemDisplay;
    [SerializeField] TextMeshProUGUI _healthValue, _hungerValue, _thirstValue;

    [Header("Inventory HUD :")]
    [SerializeField] GameObject _inventoryObject;

    [SerializeField] Image[] _inventoryCellsIcons;
    [SerializeField] TextMeshProUGUI[] _inventoryStackSizeTexts;

    [Header("Slotbar HUD :")]
    [SerializeField] Image[] _slotBarItemIcons;
    [SerializeField] TextMeshProUGUI[] _slotBarStackSizeTexts;

    public delegate void WhenDisplayEnter(string text);
    public static WhenDisplayEnter OnDisplayEnter;

    public delegate void WhenHungerVitalValueChange(Vital vital);
    public static WhenHungerVitalValueChange OnHungerVitalValueHasChanged;

    public delegate void WhenThirstVitalValueChange(Vital vital);
    public static WhenThirstVitalValueChange OnThirstVitalValueHasChanged;

    public delegate void WhenHealthVitalValueChange(Vital vital);
    public static WhenHealthVitalValueChange OnHealthVitalValueHasChanged;

    public delegate void WhenItemPickup(int slotID, Sprite itemIcon);
    public static WhenItemPickup OnItemPickup;

    public delegate void OnItemStackAdded(int slotID, int stackSize);
    public static OnItemStackAdded OnItemAdded;

    public delegate void WhenItemUsed(int slotID);
    public static WhenItemUsed OnItemUsed;

    readonly StringBuilder 
        _hungerValueTextSB = new(),
        _thirstValueTextSB = new(),
        _healthValueTextSB = new();

    readonly StringBuilder[] _inventoryCells_SBTextStack = new StringBuilder[20] {
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
    };

    private void OnDisable() {
        OnHungerVitalValueHasChanged -= UpdateHungerVitalValueText;
        OnThirstVitalValueHasChanged -= UpdateThirstVitalValueText;
        OnHealthVitalValueHasChanged -= UpdateHealthVitalValueText;

        OnItemPickup -= UpdateInventoryItemIcon;
        OnItemUsed   -= UpdateInventoryItemIcon;

        OnDisplayEnter -= DisplayText;

        OnItemAdded -= UpdateInventoryItemStackSize;
    }

    private void OnEnable() {
        OnHungerVitalValueHasChanged += UpdateHungerVitalValueText;
        OnThirstVitalValueHasChanged += UpdateThirstVitalValueText;
        OnHealthVitalValueHasChanged += UpdateHealthVitalValueText;

        OnItemPickup += UpdateInventoryItemIcon;
        OnItemUsed += UpdateInventoryItemIcon;

        OnDisplayEnter += DisplayText;

        OnItemAdded += UpdateInventoryItemStackSize;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I))
            InventoryOpenClose();
    }

    public void InventoryOpenClose() {
        _inventoryObject.SetActive(!_inventoryObject.activeSelf);
    }

    public void UpdateInventoryItemStackSize(int slotID, int stackSize) {
        for (int i = 0; i < _inventoryCellsIcons.Length; i++) {
           if (i == slotID) {
                _inventoryCells_SBTextStack[i].Clear();
                _inventoryCells_SBTextStack[i].Append(stackSize == 0 ? string.Empty : stackSize.ToString());
                _inventoryStackSizeTexts[i].text = _inventoryCells_SBTextStack[i].ToString();
                break;
           }
        }
    }

    /// <summary>
    /// replace the generic ItemSlot's icon
    /// </summary>
    /// <param name="slotID"></param>
    /// <param name="itemIcon"></param>
    public void UpdateInventoryItemIcon(int slotID, Sprite itemIcon) {
        _inventoryCellsIcons[slotID].sprite = itemIcon;
    }

    /// <summary>
    /// override the current ItemIcon's icon by null
    /// </summary>
    /// <param name="slotID"></param>
    public void UpdateInventoryItemIcon(int slotID) {
        _inventoryCellsIcons[slotID].sprite = null;
    }

    #region Thirst & Vitals
    private void UpdateHungerVitalValueText(Vital vital) {
        _hungerValueTextSB.Clear();
        _hungerValueTextSB.Append(vital.ToString());
       _hungerValue.text = _hungerValueTextSB.ToString();
    }
    
    private void UpdateThirstVitalValueText(Vital vital) {
        _thirstValueTextSB.Clear();
        _thirstValueTextSB.Append(vital.ToString());
        _thirstValue.text = _thirstValueTextSB.ToString();
    }

    private void UpdateHealthVitalValueText(Vital vital) {
        _healthValueTextSB.Clear();
        _healthValueTextSB.Append(vital.ToString());
        _healthValue.text = _healthValueTextSB.ToString();
    }
    #endregion

    /// <summary>
    /// display text on the hud
    /// </summary>
    /// <param name="text"></param>
    void DisplayText(string text) {
        _itemDisplay.text = text;
    }
}