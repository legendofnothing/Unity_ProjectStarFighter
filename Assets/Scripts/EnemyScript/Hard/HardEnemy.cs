using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.Hard {
    public class HardEnemy : MonoBehaviour {
        private EnemyBehaviors _enemyBehaviors;
        private EnemyWeapon _enemyWeapon;
        private Enemy _enemy;
        
        private void Awake() {
            _enemyBehaviors = GetComponent<EnemyBehaviors>();
            _enemyWeapon = GetComponent<EnemyWeapon>();
            _enemy = GetComponent<Enemy>();
        }
        
        
    }
}
