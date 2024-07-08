using Lucky.Loader;
using Lucky.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Buildings
{
    public class SoldiersManager : Singleton<SoldiersManager>
    {
        public Transform soldiersContainer;
        public ObjectPool<Soldier> soldierPool;

        protected override void Awake()
        {
            base.Awake();
            soldierPool = new ObjectPool<Soldier>(() => Instantiate(Res.soldierPrefab, soldiersContainer));
        }
    }
}