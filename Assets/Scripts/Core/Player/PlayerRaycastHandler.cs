using System;
using UnityEngine;

/// <summary>
/// This script handle the raycasting helper features
/// </summary>
public class PlayerRaycastHandler : MonoBehaviour
{
    /// <summary>
    /// Invoked when the player iis pressing F
    /// </summary>
    public event Action<Transform> OnKeyPressF;
    /// <summary>
    /// Invoked when the player has pressed F
    /// </summary>
    public event Action<Transform> OnKeyDownPressF;
    /// <summary>
    /// Invoked when the player has released F
    /// </summary>
    public event Action OnKeyReleaseF;

    public delegate bool WhenRaycastEnter(out Transform rayHit);
    public static WhenRaycastEnter OnRaycastEnter;

    [SerializeField] LayerMask _maskHit;
    readonly RaycastHit[] hits = new RaycastHit[1];

    private void FixedUpdate() {
        if (Physics.RaycastNonAlloc(transform.position, transform.TransformDirection(Vector3.forward), hits, 15, _maskHit) > 0) {
            RaycastHit hit = hits[0];

            OnRaycastEnter += (out Transform rayHit) => {
                rayHit = hit.transform;
                return true;
            };
        } 
        else {
            OnRaycastEnter = null;
            OnRaycastEnter += (out Transform hit) => {
                hit = null;
                return false;
            };

            return;
        }
    }

    private void Update() {
        if (!Cursor.visible && Input.GetKey(KeyCode.F)) {
            OnKeyPressF?.Invoke(hits[0].transform);
        }

        if (!Cursor.visible && Input.GetKey(KeyCode.F)) {
            OnKeyReleaseF?.Invoke();
        }

        if (!Cursor.visible && Input.GetKeyDown(KeyCode.F)) {
            OnKeyDownPressF?.Invoke(hits[0].transform);
        }
    }
}