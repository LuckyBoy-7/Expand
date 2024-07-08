using Lucky.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buildings
{
    public class Soldier : MonoBehaviour
    {
        public float moveSpeed = 100;
        public float targetPosRandomRadius = 5;
        public Building targetBuilding;
        public Building fromBuilding;
        private Vector3 targetPos;

        public void Init(Vector3 pos, float radius, float speed, Building fromBuilding, Building toBuilding)
        {
            transform.position = pos + (Vector3)Random.insideUnitCircle * radius;
            this.fromBuilding = fromBuilding;
            targetBuilding = toBuilding;
            moveSpeed = speed;
            targetPos = targetBuilding.transform.position + (Vector3)Random.insideUnitCircle * targetPosRandomRadius;
        }

        private void Update()
        {
            if (fromBuilding)
                moveSpeed = fromBuilding.SoldierMoveSpeed;
            transform.position += this.Dir(targetPos) * (moveSpeed * Time.deltaTime);
            if (targetBuilding == null)
            {
                Destroyed();
                return;
            }

            // 对应洪水造成最大人数减员，多出来的兵直接在路上死亡
            if (targetBuilding.possibleSoldiers > targetBuilding.MaxSoldiers)
            {
                Destroyed();
                targetBuilding.ComingSoldiers -= 1;
            }

            if (this.Dist(targetBuilding) < 40)
            {
                Destroyed();
                targetBuilding.CurrentSoldiers += 1;
                targetBuilding.ComingSoldiers -= 1;
            }
        }

        private void Destroyed()
        {
            SoldiersManager.instance.soldierPool.Release(this);
            gameObject.SetActive(false);
        }
    }
}