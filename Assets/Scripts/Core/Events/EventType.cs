namespace Core.Events {
    public enum EventType {
        //UI Event
        OnPlayerHpChangeBar,
        OnPlayerHpChangeText,
        OnShieldChange,
        OnMinimapSizeChange,
        
        OnDialoguesChange,
        ClearDialogues,
        OpenDeathUI,
        OpenWinUI,
        OpenActualWinUI,
        
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