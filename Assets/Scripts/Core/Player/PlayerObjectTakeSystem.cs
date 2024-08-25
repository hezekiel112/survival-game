using System;
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

    Rigidbody TakedObjectRigidbody {
        get; set;
    }

    private void Start() {
        _playerRaycast.OnKeyPressF += TakeObject;
        _playerRaycast.OnKeyDownPressF += PickupItem;
        _playerRaycast.OnKeyReleaseF += ReleaseObject;
    }

    private void OnDisable() {
        _playerRaycast.OnKeyPressF     -= TakeObject;
        _playerRaycast.OnKeyDownPressF -= PickupItem;
        _playerRaycast.OnKeyReleaseF   -= ReleaseObject;
    }

    void DisableHUDText() {
        StartCoroutine(PlayerHUD.OnDisplayEnter(""));
    }

    void Update() {
        if (TakedObjectRigidbody) {
            DisableHUDText();
            
            return;
        }

        if (!Cursor.visible) {
            if (PlayerRaycastHandler.OnRaycastEnter(out Transform hit)) {
                StartCoroutine(PlayerHUD.OnDisplayEnter($"Press F to pickup <color=green><b>{hit.name}</b></color>"));
            } else {
                DisableHUDText();
            }
        } 
    }

    void PickupItem(Transform t) {
        if (TakedObjectRigidbody)
            return;

        if (t != null && t.CompareTag("Item")) {
            if (ItemManager.Instance.TryGetItemByGameObject(t.gameObject, out PlayerItem item)) {
                if (!item.Item.CanBeStacked) {
                    ItemSlot slot = _playerInventory.FindFirstFreeSlot();

                    if (slot != null) {
                        slot.AddItemToSlot(item);
                        StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                    } else {
                        StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));
                    }

                    return;
                }

                if (item.Item.CanBeStacked) {
                    _playerInventory.GetSlotWithItem(item, out ItemSlot slot);

                    if (slot != null) {
                        if ((slot.Stack + slot.GetItem().DefaultStackSize) <= slot.GetItem().MaxStackSize) {
                            slot.AddItemToSlot(item);

                            int stackToAdd = item.Item.DefaultStackSize - slot.Stack;

                            print(stackToAdd);

                            StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                        }
                        else if ((slot.Stack + 1) == slot.GetItem().MaxStackSize) {
                            slot.Stack++;

                            ItemSlot newFreeSlot = _playerInventory.FindFirstFreeSlot();
                            if (newFreeSlot != null) {
                                newFreeSlot.AddItemToSlot(item, item.Item.DefaultStackSize - 1);
                                StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                            } else
                                StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));

                            PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);
                        }
                    } else {
                        ItemSlot newFreeSlot = _playerInventory.FindFirstFreeSlot();

                        if (newFreeSlot != null) {
                            newFreeSlot.AddItemToSlot(item);
                            StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                        } else
                            StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));
                    }
                    return;
                }
            }
        }
    }

    void TakeObject(Transform t) {
        if (t != null && t.CompareTag("Pickable")) {
            Rigidbody rb = t.GetComponent<Rigidbody>();

            if (rb) {
                DisableHUDText();

                TakedObjectRigidbody = rb;
                Debug.Log(TakedObjectRigidbody);
                t.position = _takePoint.position;
                TakedObjectRigidbody.isKinematic = true;
                TakedObjectRigidbody.useGravity = false;
            }
        }
    }

    void ReleaseObject() {
        if (!TakedObjectRigidbody)
            return;

        TakedObjectRigidbody.isKinematic = false;
        TakedObjectRigidbody.useGravity = true;
        TakedObjectRigidbody = null;
    }
}