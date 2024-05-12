using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyScript.Boss {
    public class Boss : MonoBehaviour {
        [ReadOnly] public EnemyBehaviors enemyBehaviors;
        [ReadOnly] public EnemyWeapon enemyWeapon;
        [ReadOnly] public Enemy enemy;
        
        private void Awake() {
            enemyBehaviors = GetComponent<EnemyBehaviors>();
            enemyWeapon = GetComponent<EnemyWeapon>();
            enemy = GetComponent<Enemy>();
            
            enemyWeapon.ChangeWeapon(0);
        }
    }
}
