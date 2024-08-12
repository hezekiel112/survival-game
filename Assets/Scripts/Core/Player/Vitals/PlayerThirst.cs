using System;
using UnityEngine;

[Serializable]
public class PlayerThirst : Vital {
    public ScriptableVital Thirst;
    [SerializeField] float _value;

    public float Value {
        get => _value;
        set => _value = value;
    }

    public override void OverrideVitalBy(Action modifier) {
        modifier.Invoke();
    }

    public override WhenDecrease OnDecrease() {
        return () => { };
    }

    public override WhenIncrease OnIncrease() {
        return () => { };
    }

    public override bool CanDecrease() {
        return _value > 0;
    }

    public override bool CanIncrease() {
        return _value < Thirst.BaseValue;
    }
    public override string ToString() {
        return _value.ToString();
    }
}