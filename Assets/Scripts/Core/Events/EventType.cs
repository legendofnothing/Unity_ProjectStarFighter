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
        OnPause,
        
        SendCommand,
        OnPlayerOutOfBound,
        OnGameStateChange,
        OnEnemySpawned,
        OnEnemyKilled,

        //Level1 Events
        OnTowerDestroyed,
        
        //Boss Events
        OnFacadeDestroyed,
        
        //Minimap
        ChangeMinimapIconSize,
    }
}