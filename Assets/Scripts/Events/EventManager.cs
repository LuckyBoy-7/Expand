using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Lucky.Extensions;
using Lucky.Managers;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Events
{
    public class EventManager : Singleton<EventManager>
    {
        public HashSet<Building> buildingsWithEvent = new();
        private EventItem eventItemPrefab;
        public Transform eventItemContainer;
        public Transform debuffAreaContainer;
        public float weakBanditRate = 1.2f;
        public float strongBanditRate = 1.4f;

        protected override void Awake()
        {
            base.Awake();
            eventItemPrefab = Resources.Load<EventItem>("Prefabs/EventItem");
        }

        private void Start()
        {
            float weakBanditEventDuration = 60;
            this.CreateFuncTimer(() =>
            {
                int number = (int)((1 + BuildingsManager.instance.buildings.Count / 4) * weakBanditRate);
                for (int i = 0; i < number; i++)
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
                    eventItem.eve = data;
                    eventItem.eventStartTimer = 60;

                    EventHintController.instance.TryShowHint(data);
                }
            }, () => weakBanditEventDuration, isStartImmediate: true);


            float droughtEventDuration = 180;
            this.CreateFuncTimer(() =>
            {
                Event data = new DroughtEvent(30);
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.eve = data;
                eventItem.eventStartTimer = Random.Range(60, 121);

                EventHintController.instance.TryShowHint(data);
            }, () => droughtEventDuration);

            float stormDuration = 300;
            this.CreateFuncTimer(() =>
            {
                Event data = new StormEvent(40);
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.eve = data;
                eventItem.eventStartTimer = Random.Range(60, 121);

                EventHintController.instance.TryShowHint(data);
            }, () => stormDuration);


            float earthquakeStartTimer = 40;
            float earthquakeEventDuration = 600 - earthquakeStartTimer;
            this.CreateFuncTimer(() =>
            {
                Event data = new EarthquakeEvent();
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.eve = data;
                eventItem.eventStartTimer = earthquakeStartTimer;

                EventHintController.instance.TryShowHint(data);
                earthquakeEventDuration = 240 - earthquakeStartTimer;
            }, () => earthquakeEventDuration);
        }

        public void CallStrongBanditEvent()
        {
            int number = (int)((1 + BuildingsManager.instance.buildings.Count / 4) * strongBanditRate);
            for (int i = 0; i < number; i++)
            {
                if (!ChoiceValidBuilding(out Building building, new HashSet<Type>
                    {
                        typeof(CircleBuilding),
                        typeof(TriangleBuilding)
                    }))
                    return;
                buildingsWithEvent.Add(building);

                Event data = new StrongBanditEvent();
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.targetBuilding = building;
                eventItem.eve = data;
                eventItem.eventStartTimer = 120;

                EventHintController.instance.TryShowHint(data);
            }
        }

        public void CallFloodEvent(List<Building> buildings)
        {
            foreach (var b in buildings)
            {
                Event data = new FloodEvent();
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.targetBuilding = b;
                eventItem.eve = data;
                eventItem.eventStartTimer = 10;

                EventHintController.instance.TryShowHint(data);
            }
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