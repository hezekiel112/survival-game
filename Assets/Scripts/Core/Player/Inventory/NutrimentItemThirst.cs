using UnityEngine;

public class NutrimentItemThirst : PlayerItem {
    [SerializeField] ScriptableItem _item;
    public override ScriptableItem Item => _item;

    public override void UseItem(PlayerVitals vital) {
        if (vital.Thirst.CanIncrease()) {

            if ((vital.Thirst.Value + _item.Bonus) <= vital.Thirst.Thirst.BaseValue) {
                vital.Thirst.Value += _item.Bonus;
                PlayerHUD.OnThirstVitalValueHasChanged(vital.Thirst);
            }
        }
    }
}
