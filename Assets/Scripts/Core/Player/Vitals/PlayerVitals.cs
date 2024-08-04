using System.Collections;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    public PlayerHealth Health;
    public PlayerThirst Thirst;
    public PlayerHunger Hunger;

    public bool IsDead => Health.Value <= 0;

    WaitForSeconds[] _waitTimers = {
        new(3f), // 35,50
        new(5f),
    };


    private void Start() {
        IEnumerator[] routines = {
            DecreaseHungerOverTime(),
            DecreaseThirstOverTime(),
        };

        foreach (var routine in routines) {
            StartCoroutine(routine);
        }
    }
        
    public IEnumerator DecreaseThirstOverTime() {
        do {
            if (Thirst.CanDecrease())
                Thirst.Value -= Thirst.Thirst.OverrideValue;

            PlayerHUD.OnThirstVitalValueHasChanged.Invoke(Thirst);

            yield return _waitTimers[0];
        }
        while (!IsDead);
    }

    public IEnumerator DecreaseHungerOverTime() {
        do {
            if (Hunger.CanDecrease())
                Hunger.Value -= Hunger.Hunger.OverrideValue;

            PlayerHUD.OnHungerVitalValueHasChanged.Invoke(Hunger);

            yield return _waitTimers[1];
        }
        while (!IsDead);
    }
}