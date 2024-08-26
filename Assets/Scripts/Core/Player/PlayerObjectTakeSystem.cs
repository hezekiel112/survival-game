using UnityEngine;

/// <summary>
/// This script handle the take or pickup system
/// </summary>
public class PlayerObjectTakeSystem : MonoBehaviour {
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
        _playerRaycast.OnKeyPressF -= TakeObject;
        _playerRaycast.OnKeyDownPressF -= PickupItem;
        _playerRaycast.OnKeyReleaseF -= ReleaseObject;
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

        if (t != null && t.CompareTag("Item") && ItemManager.Instance.TryGetItemByGameObject(t.gameObject, out PlayerItem item)) {
            if (!item.Item.CanBeStacked) {

                if (AddFreeSlot(item, out var newFreeSlot)) {
                    newFreeSlot.AddItemToSlot(item);
                    StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                } else {
                    StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));
                }

                return;
            }

            // this code is really messy, need rework asap
            if (item.Item.CanBeStacked) {
                _playerInventory.GetSlotWithItem(item, out ItemSlot slot);

                // if there's a slot with the current targeted item
                if (slot != null) {

                    // check if the slot stack can be increased by the item default stack size
                    if ((slot.Stack + slot.GetItem().DefaultStackSize) <= slot.GetItem().MaxStackSize) {
                        slot.AddItemToSlot(item);

                        int stackToAdd = item.Item.DefaultStackSize - slot.Stack;

                        StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                    }

                    // if not - check if the stack can be increased by one, if so, add 1 to the stack and add another
                    // slot with the remaining stack - 1
                    else if ((slot.Stack + 1) == slot.GetItem().MaxStackSize) {
                        slot.Stack++;

                        StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: 1));
                        PlayerHUD.OnItemAdded(slot.SlotID, slot.Stack);

                        if (AddFreeSlot(item, out var newFreeSlot)) {
                            if (item.Item.DefaultStackSize > 1) {
                                newFreeSlot.AddItemToSlot(item, item.Item.DefaultStackSize - 1);
                            } else if (item.Item.DefaultStackSize == 1) {
                                newFreeSlot.AddItemToSlot(item, item.Item.DefaultStackSize);
                            }

                            StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                        }
                    }
                } else {

                    if (AddFreeSlot(item, out _)) {
                        StartCoroutine(PlayerHUD.OnDisplayEnter(pickedUpItem: item, count: item.Item.DefaultStackSize));
                    } else {
                        StartCoroutine(PlayerHUD.OnDisplayEnter("inventory is full !", true));
                    }
                }
            }
        }
    }


    bool AddFreeSlot(PlayerItem item, out ItemSlot slot) {
        ItemSlot newFreeSlot = _playerInventory.FindFirstFreeSlot();

        newFreeSlot?.AddItemToSlot(item);
        slot = newFreeSlot;

        return newFreeSlot != null;
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