namespace EnemyScript.TowerScript {
    public class TowerObjective : Tower {

        protected override void OnUpdate() {
            DetectPlayer();
        }
    }
}