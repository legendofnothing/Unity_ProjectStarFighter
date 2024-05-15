namespace Core.Events {
    public enum EventType {
        //UI Event
        OnPlayerHpChangeBar,
        OnPlayerHpChangeText,
        OnShieldChange,
        OnMinimapSizeChange,
        
        OnDialoguesChange,
        OpenDeathUI,
        OpenWinUI,
        
        SendCommand,
        OnPlayerOutOfBound,
        OnGameStateChange,
        OnEnemyKilled,

        //Level1 Events
        OnTowerDestroyed,
        
        //Boss Events
        OnFacadeDestroyed,
        
        //Minimap
        ChangeMinimapIconSize,
    }
}