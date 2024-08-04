using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handle the inventory feature
/// </summary>
[Serializable]
public class PlayerItemCollection {
    [SerializeField] List<ScriptableItem> _playerItems;

    public void Add(PlayerItem item) {
        _playerItems.Add(item.Item);
    } 
}
