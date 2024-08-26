using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// This script handle the items registry I/O by sorting the PlayerItem and their respective ID
/// </summary>
public sealed class ItemManager : MonoBehaviour {
    public ItemDatabase ItemDatabase;

    readonly Dictionary<int, PlayerItem> _items = new();

    public ReadOnlyDictionary<int, PlayerItem> Items;

    public static ItemManager Instance { get; private set; }

    private void OnEnable() {
        if (Instance)
            Destroy(Instance);

        Instance = this;

        foreach (var item in ItemDatabase.Items) {
            if (item.TryGetComponent<PlayerItem>(out PlayerItem playerItem)) {
                _items.Add(playerItem.Item.ItemID, playerItem);
            }
        }

        Items = new ReadOnlyDictionary<int, PlayerItem>(_items);
        Debug.Log(Items[0].Item.ItemName);
    }

    public PlayerItem GetItem(int itemID) {
        return _items[itemID];
    }

    public bool TryGetItemByGameObject(GameObject go, out PlayerItem item) {
        return go.TryGetComponent<PlayerItem>(out item);
    }

    public bool TryGetItemByID(int id, out PlayerItem item) {

        bool hasGotItem = Items.ContainsKey(id);

        if (hasGotItem)
            item = Items[id] as PlayerItem;

        else
            item = null;

        return hasGotItem;
    }
}