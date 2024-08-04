using System;
using UnityEngine;

[Serializable]
public class PlayerHealth : Vital {
    public ScriptableVital Health;
    [SerializeField] float _value;
    public float Value {
        get => _value;
        set => _value = value;
    }

    public override bool CanDecrease() {
        return _value > 0;
    }

    public override bool CanIncrease() {
        return _value < Health.BaseValue;
    }

    public override string ToString() {
        return _value.ToString();
    }
}
