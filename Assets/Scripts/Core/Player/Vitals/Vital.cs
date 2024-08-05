using System;

/// <summary>
/// This script handle the Vital feature of the player such as, hunger, thirst or health
/// </summary>
public abstract class Vital {

    public delegate void WhenDecrease();
    public abstract WhenDecrease OnDecrease();

    public delegate void WhenIncrease();
    public abstract WhenIncrease OnIncrease();

    public abstract void OverrideVitalBy(Action modifier);

    /// <summary>
    /// Return true if the selected vital is decreasing over time. wheareas the health should not
    /// </summary>
    /// <returns></returns>
    public virtual bool CanDecrease() {
        return true;
    }

    /// <summary>
    /// Return true if the selected vital is increasing over time. wheareas the health should not
    /// </summary>
    /// <returns></returns>
    public virtual bool CanIncrease() {
        return true;
    }
}
