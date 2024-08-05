using System;
using UnityEngine;

[Serializable]
public class PlayerHunger : Vital {
    public ScriptableVital Hunger;
    [SerializeField] float _value;
    public float Value {
        get => _value;
        set => _value = value;
    }

    public override void OverrideVitalBy(Action modifier) {
        modifier.Invoke();
    }

    public override WhenDecrease OnDecrease() {
        return () => Debug.Log("Hunger decreased.");
    }

    public override WhenIncrease OnIncrease() {
        return () => Debug.Log("Hunger increased.");
    }

    public override bool CanDecrease() {
        return _value > 0;
    }

    public override bool CanIncrease() {
        return _value < Hunger.BaseValue;
    }

    public override string ToString() {
        return _value.ToString();
    }
}
