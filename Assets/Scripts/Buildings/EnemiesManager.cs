using Lucky.Loader;
using Lucky.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Buildings
{
    public class EnemiesManager : Singleton<EnemiesManager>
    {
        public Transform enemiesContainer;
        public ObjectPool<Enemy> enemyPool;

        protected override void Awake()
        {
            base.Awake();
            enemyPool = new ObjectPool<Enemy>(() => Instantiate(Res.enemyPrefab, enemiesContainer));
        }
    }
}