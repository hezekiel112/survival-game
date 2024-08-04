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

    private void FixedUpdate() {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 15, _maskHit)) {
            OnRaycastEnter += (out Transform rayHit) => {
                rayHit = hit.transform;
                return true;
            };
        } else {
            OnRaycastEnter = null;
            OnRaycastEnter += (out Transform hit) => {
                hit = null;
                return false;
            };
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
            OnKeyDownPressF.Invoke(hit.transform);

        if (Input.GetKey(KeyCode.F))
            OnKeyPressF.Invoke(hit.transform);
    }

    private void Update() {
        if (!Input.GetKey(KeyCode.F)) {
            OnKeyReleaseF.Invoke();
        }
    }
}