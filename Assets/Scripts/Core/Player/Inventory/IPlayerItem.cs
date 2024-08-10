public interface IPlayerItem {
    ScriptableItem Item {
        get;
    }

    /// <summary>
    /// Invoked when item is used, also increase/decrease the PlayerVitals respective value
    /// </summary>
    /// <param name="player"></param>
    public void UseItem(PlayerVitals player) { }
}