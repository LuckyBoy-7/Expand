using System;
using System.Collections.Generic;
using Lucky.Managers;
using UnityEngine;

namespace Events
{
    public class EventHintController : Singleton<EventHintController>
    {
        private List<EventHint> hints = new();
        private EventHint hintPrefab;
        public float space;

        protected override void Awake()
        {
            base.Awake();
            hintPrefab = Resources.Load<EventHint>("Prefabs/EventHint");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                TryShowHint(new WeakBanditEvent());
            }
        }

        public void TryShowHint(Event eventData)
        {
            var hint = Instantiate(hintPrefab, transform);
            var trans = hint.GetComponent<RectTransform>();
            trans.anchoredPosition = Vector2.down * (space * hints.Count) + Vector2.right * 1000;
            hint.nameText.text = eventData.name;
            hint.descriptionText.text = eventData.description;
            hint.PopUp(0.5f);
            hints.Add(hint);
        }

        public void OnHintOver(EventHint eventHint)
        {
            int i = hints.IndexOf(eventHint);
            int overNumber = 0;
            for (int j = i + 1; j < hints.Count; j++)
            {
                if (hints[i].over)
                    overNumber += 1;
                else
                    hints[j + overNumber].MoveTo(Vector3.down * ((j - 1 + overNumber) * space), 0.5f);
            }

            hints.Remove(eventHint);
        }
    }
}