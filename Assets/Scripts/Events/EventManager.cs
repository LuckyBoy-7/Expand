using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Lucky.Extensions;
using Lucky.Managers;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Pool;

namespace Events
{
    public class EventManager : Singleton<EventManager>
    {
        public HashSet<Building> buildingsWithEvent = new();
        private EventItem eventItemPrefab;
        public Transform eventItemContainer;
        public Transform debuffAreaContainer;

        protected override void Awake()
        {
            base.Awake();
            eventItemPrefab = Resources.Load<EventItem>("Prefabs/EventItem");
        }

        private void Start()
        {
            // float earthquakeEventDuration;
            float floodEventDuration = 1;


            // float weakBanditEventDuration = 90;
            // this.CreateFuncTimer(() =>
            // {
            //     if (!ChoiceValidBuilding(out Building building, new HashSet<Type>
            //         {
            //             typeof(CircleBuilding),
            //             typeof(TriangleBuilding)
            //         }))
            //         return;
            //     buildingsWithEvent.Add(building);
            //
            //     Event data = new WeakBanditEvent();
            //     var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
            //     eventItem.targetBuilding = building;
            //     eventItem.eve = data;
            //     eventItem.eventStartTimer = 90;
            //
            //     EventHint.instance.TryShowHint(data);
            //
            // }, () => weakBanditEventDuration);
            //
            // float strongBanditEventDuration = 1;
            // this.CreateFuncTimer(() =>
            // {
            //     if (!ChoiceValidBuilding(out Building building, new HashSet<Type>
            //         {
            //             typeof(CircleBuilding),
            //             typeof(TriangleBuilding)
            //         }))
            //         return;
            //     buildingsWithEvent.Add(building);
            //
            //     Event data = new StrongBanditEvent();
            //     var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
            //     eventItem.targetBuilding = building;
            //     eventItem.eve = data;
            //     eventItem.eventStartTimer = 1;
            //
            //     EventHint.instance.TryShowHint(data);
            //
            // }, () => strongBanditEventDuration);

            // float droughtEventDuration = 1;
            // this.CreateFuncTimer(() =>
            // {
            //     Event data = new DroughtEvent();
            //     var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
            //     eventItem.eve = data;
            //     eventItem.eventStartTimer = 1;
            //
            //     EventHint.instance.TryShowHint(data);
            // }, () => droughtEventDuration);
            
            float stormDuration = 1;
            this.CreateFuncTimer(() =>
            {
                Event data = new StormFlood();
                var eventItem = Instantiate(eventItemPrefab, eventItemContainer);
                eventItem.eve = data;
                eventItem.eventStartTimer = 1;

                EventHint.instance.TryShowHint(data);
            }, () => stormDuration);
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