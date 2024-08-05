using System;
using UnityEngine;

[Serializable]
public class PlayerHealth : Vital {
    public ScriptableVital Health;
    [SerializeField] float _value;

    public override void OverrideVitalBy(Action modifier) {
        modifier.Invoke();
    }
    public override WhenDecrease OnDecrease() {
        return () => Debug.Log("Health decreased.");
    }

    public override WhenIncrease OnIncrease() {
        return () => Debug.Log("Health increased.");
    }

    public float Value {
        get => _value;
        set => _value = value;
    }

    public override bool CanDecrease() {
        return false;
    }

    public override bool CanIncrease() {
        return false;
    }

    public override string ToString() {
        return _value.ToString();
    }
}
