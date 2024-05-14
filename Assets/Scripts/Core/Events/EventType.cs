namespace Core.Events {
    public enum EventType {
        //UI Event
        OnPlayerHpChangeBar,
        OnPlayerHpChangeText,
        OnShieldChange,
        OnMinimapSizeChange,
        
        SendCommand,
        OnPlayerOutOfBound,
        OnGameStateChange,
        
        //Level1 Events
        OnTowerDestroyed,
        
        //Boss Events
        OnFacadeDestroyed,
    }
}