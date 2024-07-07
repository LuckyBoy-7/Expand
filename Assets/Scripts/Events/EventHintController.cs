using System;
using System.Collections.Generic;
using DG.Tweening;
using Lucky.Managers;
using Lucky.Utilities;
using UnityEngine;

namespace Events
{
    public class EventHintController : Singleton<EventHintController>
    {
        private EventHint[] hints = new EventHint[4];
        private Queue<Event> waitingEvents = new Queue<Event>();
        private HashSet<Type> calledEvents = new ();
        private EventHint hintPrefab;
        public float space;
        public CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            hintPrefab = Resources.Load<EventHint>("Prefabs/EventHint");
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            Lucky.Managers.EventManager.instance.Register("Gameover", OnGameover);
        }

        private void OnDisable()
        {
            Lucky.Managers.EventManager.instance.Unregister("Gameover", OnGameover);
        }

        private void OnGameover(object[] param)
        {
            float duration = (float)param[0];
            canvasGroup.DOFade(0, duration);
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Return))
            // {
            //     TryShowHint(new WeakBanditEvent());
            // }

            if (waitingEvents.Count > 0)
                for (var i = 0; i < hints.Length; i++)
                {
                    if (hints[i] == null)
                    {
                        ShowHint(waitingEvents.Dequeue(), i);
                        return;
                    }
                }
        }

        public void TryShowHint(Event eventData)
        {
            if (calledEvents.Contains(eventData.GetType()))
                return;
            calledEvents.Add(eventData.GetType());
            for (var i = 0; i < hints.Length; i++)
            {
                if (hints[i] == null)
                {
                    ShowHint(eventData, i);
                    return;
                }
            }

            waitingEvents.Enqueue(eventData);
        }

        private void ShowHint(Event eventData, int i)
        {
            var hint = Instantiate(hintPrefab, transform);
            var trans = hint.GetComponent<RectTransform>();
            trans.anchoredPosition = Vector2.down * (space * i) + Vector2.right * 1000;
            hint.nameText.text = ColorUtils.Wrap(eventData.name, eventData.color);
            hint.descriptionText.text = eventData.description;
            hint.PopUp(0.5f);
            hints[i] = hint;
        }

        public void OnHintOver(EventHint eventHint)
        {
            for (int i = 0; i < hints.Length; i++)
            {
                if (hints[i] == eventHint)
                    hints[i] = null;
            }
        }
    }
}