using UnityEngine;

public class NutrimentItemHunger : PlayerItem {
    [SerializeField] ScriptableItem _item;
    public override ScriptableItem Item => _item;

    public override void UseItem(PlayerVitals vital) {
        if (vital.Hunger.CanIncrease()) {
            vital.Hunger.Value += _item.Bonus;
            PlayerHUD.OnHungerVitalValueHasChanged(vital.Hunger);
        }
    }
}
