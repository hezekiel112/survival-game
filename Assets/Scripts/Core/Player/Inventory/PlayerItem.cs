using UnityEngine;

/// <summary>
/// This script handle the item feature
/// </summary>
public abstract class PlayerItem : MonoBehaviour, IPlayerItem {
    public abstract ScriptableItem Item {
        get;
    }

    public virtual void UseItem(PlayerVitals vital) {

    }
}