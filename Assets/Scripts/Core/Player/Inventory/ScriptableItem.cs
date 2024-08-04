using UnityEngine;

/// <summary>
/// This script handle the PlayerItem item data
/// </summary>
[CreateAssetMenu]
public class ScriptableItem : ScriptableObject {
    [SerializeField] string _itemName;
    [SerializeField] string _itemDescription;
    [SerializeField] Sprite _itemIcon;
    [SerializeField] float _bonus;
    [SerializeField] EItemType _itemType;
    [Space]
    [SerializeField] bool _canBeStacked;
    [SerializeField] int _maxStackSize;
    [Space]
    [SerializeField] int _itemID;

    public int ItemID => _itemID;
    public string ItemName => _itemName;
    public string ItemDescription => _itemDescription;
    public Sprite ItemIcon => _itemIcon;
    public float Bonus => _bonus;
    public EItemType ItemType => _itemType;
    
    public bool CanBeStacked => _canBeStacked;
    public int MaxStackSize => _maxStackSize;
}