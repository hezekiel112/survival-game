using System.Collections;
using UnityEngine;

/// <summary>
/// This script handle the take or pickup system
/// </summary>
public class PlayerObjectTakeSystem : MonoBehaviour
{
    [SerializeField] Transform _takePoint;
    [SerializeField] PlayerVitals _playerVitals;
    [SerializeField] PlayerRaycastHandler _playerRaycast;
    [SerializeField] PlayerInventory _playerInventory;
    Rigidbody _objectToTakeRb;

    private void Start() {
        _playerRaycast.OnKeyPressF += TakeObject;
        _playerRaycast.OnKeyDownPressF += PickupItem;
        _playerRaycast.OnKeyReleaseF += ReleaseObject;
    }

    void DisableHUDText() {
        StartCoroutine(PlayerHUD.OnDisplayEnter(""));
    }

    void Update() {
        if (!Cursor.visible && PlayerRaycastHandler.OnRaycastEnter(out Transform hit)) {
            StartCoroutine(PlayerHUD.OnDisplayEnter($"Press F to pickup <color=green><b>{hit.name}</b></color>"));
        } else {
            DisableHUDText();
        }

        if (_objectToTakeRb != null) {
            DisableHUDText();
        }
    }

    void PickupItem(Transform t) {
        if (t.CompareTag("Item")) {
            if (ItemManager.Instance.TryGetItemByGameObject(t.gameObject, out PlayerItem item)) {
                if (!item.Item.CanBeStacked) {
                    ItemSlot slot = _playerInventory.FindFirstFreeSlot();

                    if (slot != null) {
                        slot.AddItemToSlot(item);
                        StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                    }
                    else {
                        StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));
                    }
                    
                    return;
                }

                if (item.Item.CanBeStacked) {
                    _playerInventory.GetSlotWithItem(item, out ItemSlot slot);
                    
                    if (slot  != null) {
                        slot.AddItemToSlot(item);
                        StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                    }
                    else {
                        ItemSlot newFreeSlot = _playerInventory.FindFirstFreeSlot();
                        
                        if (newFreeSlot != null) {
                            newFreeSlot.AddItemToSlot(item);
                            StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                        }
                        else
                            StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));
                    }

                    return;
                }
            }
        }
    }

    void TakeObject(Transform t) {
        if (t.CompareTag("Pickable")) {
            Rigidbody rb = t.GetComponent<Rigidbody>();

            _objectToTakeRb = rb;
            t.position = _takePoint.position;
            _objectToTakeRb.isKinematic = true;
            _objectToTakeRb.useGravity = false;
        }
    }

    void ReleaseObject() {
        if (!_objectToTakeRb)
            return;

        _objectToTakeRb.isKinematic = false;
        _objectToTakeRb.useGravity = true;
        _objectToTakeRb = null;
    }
}