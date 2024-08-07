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
    }

        private void Start() {
        for (int i = 0; i < ItemDatabase.Items.Length; i++) {
            if (ItemDatabase.Items[i].TryGetComponent<PlayerItem>(out PlayerItem item)) {
                _items.Add(i, item);
            }
        }

        Items = new ReadOnlyDictionary<int, PlayerItem>(_items);
        Debug.Log(Items.Count);
    }

    public PlayerItem GetItem(int id) {
        return Items[id];
    }

    public bool TryGetItemByGameObject(GameObject go, out PlayerItem item) {
        return go.TryGetComponent<PlayerItem>(out item);
    }

    public bool TryGetItemByID(int id, out PlayerItem item) {

        bool hasGotItem = Items.ContainsKey(id);

        if (hasGotItem) 
            item = Items[id];

        else
            item = null;

        return hasGotItem;
    }
}