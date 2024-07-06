using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Lucky.Extensions;
using Lucky.Managers;
using Mono.Cecil;
using UnityEngine;

namespace Events
{
    public class EventManager : Singleton<EventManager>
    {
        public HashSet<Building> buildingsWithEvent = new();
        private EventItem eventItemPrefab;
        public Transform eventItemContainer;
        public Transform enemiesContainer;

        protected override void Awake()
        {
            base.Awake();
            eventItemPrefab = Resources.Load<EventItem>("Prefabs/EventItem");
        }

        private void Start()
        {
            float weakBanditEventDuration = 1;
            // float strongBanditEventDuration;
            // float droughtEventDuration;
            // float floodEventDuration;
            // float earthquakeEventDuration;
            this.CreateFuncTimer(() =>
            {
                if (!ChoiceValidBuilding(out Building building, new HashSet<Type>
                    {
                        typeof(CircleBuilding),
                        typeof(TriangleBuilding)
                    }))
                    return;
                buildingsWithEvent.Add(building);

                Event data = new WeakBanditEvent();
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.targetBuilding = building;
                eventItem.eventData = data;
                eventItem.eventStartTimer = weakBanditEventDuration;

                EventHint.instance.TryShowHint(data);

                weakBanditEventDuration = 10000;
            }, () => weakBanditEventDuration);
        }

        private bool ChoiceValidBuilding(out Building building, HashSet<Type> types)
        {
            building = null;

            List<Building> buildings = BuildingsManager.instance.buildings.Where(b => types.Contains(b.GetType())).ToList();
            if (buildings.Count == 0)
                return false;
            building = buildings.Choice();
            int tryCount = 0;
            while (buildingsWithEvent.Contains(building))
            {
                building = buildings.Choice();
                if (tryCount++ > 100)
                    return false;
            }

            return true;
        }
    }
}