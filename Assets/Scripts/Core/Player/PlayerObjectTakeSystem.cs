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
        PlayerHUD.OnDisplayEnter(string.Empty);
    }

    void Update() {
        if (PlayerRaycastHandler.OnRaycastEnter(out Transform hit)) {
            PlayerHUD.OnDisplayEnter($"Press F to pickup <color=green><b>{hit.name}</b></color>");
        } else
            DisableHUDText();

        if (_objectToTakeRb != null) {
            DisableHUDText();
            return;
        }
    }

    void PickupItem(Transform t) {
        if (t.CompareTag("Item")) {
            if (ItemManager.Instance.TryGetItemByGameObject(t.gameObject, out PlayerItem item)) {
                if (!item.Item.CanBeStacked) {
                    _playerInventory.FindFirstFreeSlot().AddItemToSlot(item);
                    return;
                }

                if (item.Item.CanBeStacked) {
                    bool hasFindSlot = _playerInventory.GetSlotWithItem(item, out var slot);
                    
                    if (hasFindSlot) {
                        Debug.Log("hasFind");
                        slot.AddItemToSlot(item);
                    }

                    if (!hasFindSlot) {

                        Debug.Log("hasNotFind");
                        _playerInventory.FindFirstFreeSlot().AddItemToSlot(item);
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