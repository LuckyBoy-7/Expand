using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buildings
{
    public class Soldier : MonoBehaviour
    {
        public float moveSpeed = 100;
        public float targetPosRandomRadius = 5;
        public Building targetBuilding;
        private Vector3 targetPos;


        private void Start()
        {
            if (targetBuilding)
                targetPos = targetBuilding.transform.position + (Vector3)Random.insideUnitCircle * targetPosRandomRadius;
        }

        public void InitPos(Vector3 pos, float radius)
        {
            transform.position = pos + (Vector3)Random.insideUnitCircle * radius;
        }

        private void Update()
        {
            transform.position += (targetPos - transform.position).normalized * (moveSpeed * Time.deltaTime);
            if (targetBuilding == null)
            {
                Destroy(gameObject);
                return;
            }

            // 对应洪水造成最大人数减员，多出来的兵直接在路上死亡
            if (targetBuilding.possibleSoldiers > targetBuilding.maxSoldiers)
            {
                Destroy(gameObject);
                targetBuilding.ComingSoldiers -= 1;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Building>() == targetBuilding)
            {
                Destroy(gameObject);
                targetBuilding.CurrentSoldiers += 1;
                targetBuilding.ComingSoldiers -= 1;
            }
        }
    }
}