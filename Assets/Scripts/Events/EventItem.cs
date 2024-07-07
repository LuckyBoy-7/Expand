using System;
using Buildings;
using DG.Tweening;
using Lucky.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Events
{
    public class EventItem : MonoBehaviour
    {
        public Image warningFillingImage;
        public Image innerImage;
        public Image icon;
        public TMP_Text text;

        public Event eve;
        public float eventStartTimer;
        public float eventStartTimerElapse;

        public Building targetBuilding;
        public CanvasGroup canvasGroup;
        private bool hasStart;
        public Image debuffCircle;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            warningFillingImage.color = eve.color;
            innerImage.color = eve.color.WithA(0.4f);
            if (eve.eventType == Event.EventType.Dot)
            {
                Vector3 offset = new Vector3(-60, 40);
                transform.position = targetBuilding.transform.position + offset;
            }
            else if (eve.eventType == Event.EventType.Area)
            {
                if (BuildingsManager.instance.buildings.Count == 0)
                {
                    EventManager.instance.buildingsWithEvent.Remove(targetBuilding);
                    Destroy(gameObject);
                    return;
                }

                var pivotPos = BuildingsManager.instance.buildings.Choice().transform.position;
                var pos = pivotPos + (Vector3)Random.insideUnitCircle * 60;
                var prefab = Resources.Load<Image>("Prefabs/Circle");
                debuffCircle = Instantiate(prefab, EventManager.instance.debuffAreaContainer);
                debuffCircle.transform.localScale = Vector3.one * eve.radius * 2;
                debuffCircle.transform.position = pos;
                debuffCircle.color = eve.color.WithA(0.2f);

                transform.position = pos;
            }
        }

        private void Update()
        {
            if (eve.eventType == Event.EventType.Dot && targetBuilding == null)
                Destroy(gameObject);
            if (hasStart)
                return;
            eventStartTimerElapse += Time.deltaTime;
            warningFillingImage.fillAmount = eventStartTimerElapse / eventStartTimer;
            if (eventStartTimerElapse > eventStartTimer)
            {
                hasStart = true;
                if (debuffCircle)
                    debuffCircle.color = eve.color.WithA(0.6f);
                StartEvent();
                canvasGroup.DOFade(0, 0.2f);
            }
        }

        private void StartEvent()
        {
            if (eve is AttackEvent)
            {
                AttackEvent e = eve as AttackEvent;
                for (int i = 0; i < e.enemyNumber; i++)
                {
                    var enemy = EnemiesManager.instance.enemyPool.Get();
                    enemy.gameObject.SetActive(true);
                    enemy.InitPos(targetBuilding.transform.position);
                    enemy.targetBuilding = targetBuilding;
                }

                if (eve is WeakBanditEvent)
                {
                    if (Random.value < 0.3f)
                        EventManager.instance.CallStrongBanditEvent();
                }

                EventManager.instance.buildingsWithEvent.Remove(targetBuilding);
            }
            else if (eve is DebuffEvent)
            {
                DebuffEvent e = eve as DebuffEvent;
                e.Do(this);
                this.CreateFuncTimer(() =>
                {
                    e.Undo(this);
                    Destroy(gameObject);
                    if (debuffCircle)
                        Destroy(debuffCircle.gameObject);
                }, () => e.duration, isOneShot: true);
            }
        }
    }
}