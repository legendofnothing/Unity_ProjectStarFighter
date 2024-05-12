namespace Core.Events {
    public enum EventType {
        SendCommand,
        OnPlayerOutOfBound,
        OnGameStateChange,
        
        //Level1 Events
        OnTowerDestroyed,
        
        //Boss Events
        OnFacadeDestroyed,
    }
}