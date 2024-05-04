using System;
using Core.Events;
using Core.Patterns;
using SO;
using UnityEngine;
using EventType = Core.Events.EventType;

namespace Level {
    public enum GameState {
        Playing,
        GameOver,
        Win
    }
    
    public class LevelManager : Singleton<LevelManager> {
        public LevelSetting levelSetting;
        private GameState _currentState;

        public GameState CurrentState => _currentState;

        private void Start() {
            _currentState = GameState.Playing;
            this.AddListener(EventType.OnGameStateChange, param => ChangeState((GameState) param));
        }

        private void ChangeState(GameState gameState) {
            if (_currentState == gameState) return;
            switch (gameState) {
                case GameState.Playing:
                    
                    break;
                case GameState.GameOver:
                    Debug.Log("Game Over");
                    break;
                case GameState.Win:
                    Debug.Log("Win");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentState = gameState;
        }
    }
}
