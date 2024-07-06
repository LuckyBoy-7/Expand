using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buildings
{
    public class Enemy : MonoBehaviour
    {
        private float fadeOutDuration = 0.4f;
        public Building targetBuilding;
        private float speed = 100;
        public CanvasGroup canvasGroup;
        private bool isKilling;

        private void OnEnable()
        {
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, fadeOutDuration);
        }

        private void Update()
        {
            if (targetBuilding == null)
            {
                Kill();
                return;
            }

            Vector3 dir = (targetBuilding.transform.position - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
        }

        public void InitPos(Vector3 position)
        {
            float outer = 200;
            float inner = 100;
            Vector3 dir = Random.insideUnitCircle;
            transform.position = position + dir * (outer - inner) + dir.normalized * inner;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Building building = other.GetComponent<Building>();
            if (building != null && building == targetBuilding)
            {
                Kill();
                targetBuilding.CurrentSoldiers -= 1;
                if (targetBuilding.CurrentSoldiers == -1)
                    targetBuilding.Destroyed();
            }
        }

        private void Kill()
        {
            if (isKilling)
                return;
            isKilling = true;
            canvasGroup.DOFade(0, fadeOutDuration).onComplete += () =>
            {
                EnemiesManager.instance.enemyPool.Release(this);
                isKilling = false;
                canvasGroup.alpha = 1;
                gameObject.SetActive(false);
            };
        }
    }
}