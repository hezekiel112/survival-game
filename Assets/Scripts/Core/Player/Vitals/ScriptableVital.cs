using UnityEngine;

[CreateAssetMenu()]
public class ScriptableVital : ScriptableObject {
    [SerializeField] float _baseValue, _overrideValue;
    public float BaseValue => _baseValue;
    public float OverrideValue => _overrideValue;
}
