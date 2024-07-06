using System;
using Buildings;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Events
{
    public class EventItem : MonoBehaviour
    {
        public Image warningFillingImage;
        public Image icon;
        public TMP_Text text;

        public Event eventData;
        public float eventStartTimer;
        public float eventStartTimerElapse;

        public Building targetBuilding;
        public CanvasGroup canvasGroup;
        private Enemy enemyPrefab;
        private bool hasStart;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            enemyPrefab = Resources.Load<Enemy>("Prefabs/Enemy");
        }

        private void Start()
        {
            Vector3 offset = new Vector3(-60, 40);
            transform.position = targetBuilding.transform.position + offset;
        }

        private void Update()
        {
            if (hasStart)
                return;
            eventStartTimerElapse += Time.deltaTime;
            warningFillingImage.fillAmount = eventStartTimerElapse / eventStartTimer;
            if (eventStartTimerElapse > eventStartTimer)
            {
                hasStart = true;
                StartEvent();
                canvasGroup.DOFade(0, 0.2f);
            }
        }

        private void StartEvent()
        {
            if (eventData is AttackEvent)
            {
                AttackEvent data = eventData as AttackEvent;
                for (int i = 0; i < data.enemyNumber; i++)
                {
                    var enemy = Instantiate(enemyPrefab, EventManager.instance.enemiesContainer);
                    enemy.InitPos(targetBuilding.transform.position);
                    enemy.targetBuilding = targetBuilding;
                }
            }
            else if (eventData is DebuffEvent)
            {
            }
        }
    }
}