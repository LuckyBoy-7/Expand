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
            if (hasStart)
                return;
            if (eve.eventType == Event.EventType.Dot && targetBuilding == null)
            {
                Destroy(gameObject);
                return;
            }

            eventStartTimerElapse += Time.deltaTime;
            warningFillingImage.fillAmount = eventStartTimerElapse / eventStartTimer;
            if (eventStartTimerElapse > eventStartTimer)
            {
                hasStart = true;
                StartEvent();
            }
        }

        private void StartEvent()
        {
            canvasGroup.DOFade(0, 0.2f);
            if (eve.eventType == Event.EventType.Area)
            {
                debuffCircle.color = eve.color.WithA(0.6f);
            }

            eve.Do(this);
            this.CreateFuncTimer(() =>
            {
                eve.Undo(this);
                Destroy(gameObject);
                if (debuffCircle)
                    Destroy(debuffCircle.gameObject);
            }, () => eve.duration, isOneShot: true);
        }
    }
}