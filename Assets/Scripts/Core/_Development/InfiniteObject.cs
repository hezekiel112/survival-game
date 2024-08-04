#if UNITY_EDITOR
using UnityEngine;

public class InfiniteObject : MonoBehaviour {
    private void Update() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }
}
#endif