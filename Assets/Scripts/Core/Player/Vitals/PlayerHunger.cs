﻿using System;
using UnityEngine;

[Serializable]
public class PlayerHunger : Vital {
    public ScriptableVital Hunger;
    [SerializeField] float _value;
    public float Value {
        get => _value;
        set => _value = value;
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