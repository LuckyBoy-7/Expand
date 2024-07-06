using Lucky.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Buildings
{
    public class EnemiesManager : Singleton<EnemiesManager>
    {
        public Enemy enemyPrefab;

        public Transform enemiesContainer;
        public ObjectPool<Enemy> enemyPool;

        protected override void Awake()
        {
            base.Awake();
            enemyPrefab = Resources.Load<Enemy>("Prefabs/Enemy");
            enemyPool = new ObjectPool<Enemy>(() => Instantiate(instance.enemyPrefab, enemiesContainer));
        }
    }
}