using System.Collections.Generic;
using System.Linq;
using Buildings;
using Lucky.Extensions;
using UnityEngine;

namespace Events
{
    public abstract class Event
    {
        public string name;
        public string description;

        public enum EventType
        {
            Dot,
            Area
        }

        public EventType eventType;
        public float radius;
        public Color color;
        public float duration;

        protected List<Building> GetBuildingsInRadius(Vector3 pos, float radius)
        {
            return BuildingsManager.instance.buildings.Where(b => b.Dist(pos) < radius).ToList();
        }

        public abstract void Do(EventItem item);

        public abstract void Undo(EventItem item);
    }

    public class AttackEvent : Event
    {
        public int enemyNumber;

        public override void Do(EventItem item)
        {
            for (int i = 0; i < enemyNumber; i++)
            {
                var enemy = EnemiesManager.instance.enemyPool.Get();
                enemy.gameObject.SetActive(true);
                enemy.InitPos(item.targetBuilding.transform.position);
                enemy.targetBuilding = item.targetBuilding;
            }

            EventManager.instance.buildingsWithEvent.Remove(item.targetBuilding);
        }

        public override void Undo(EventItem item)
        {
        }
    }

    public class DebuffEvent : Event
    {
        public List<Building> affectedBuildings = new();

        public override void Do(EventItem item)
        {
        }

        public override void Undo(EventItem item)
        {
        }
    }


    public class WeakBanditEvent : AttackEvent
    {
        public WeakBanditEvent()
        {
            name = "WeakBandit";
            description = "Bandits appear to attack this area";
            eventType = EventType.Dot;
            color = Color.gray;
            enemyNumber = Random.Range(5, 9);
        }

        public override void Do(EventItem item)
        {
            base.Do(item);
            if (Random.value < 0.3f)
                EventManager.instance.CallStrongBanditEvent();
        }
    }

    public class StrongBanditEvent : AttackEvent
    {
        public StrongBanditEvent()
        {
            name = "StrongBandit";
            description = "Strong Bandits appear to attack this area";
            eventType = EventType.Dot;
            color = Color.green;
            enemyNumber = Random.Range(20, 30);
        }
    }

    public class DroughtEvent : DebuffEvent
    {
        public DroughtEvent(float duration)
        {
            name = "Drought";
            description = "Every area Affected by the drought reduces transfer speed by half. And Castle reduces 20% produce rate";
            eventType = EventType.Area;
            color = Color.yellow;
            this.duration = duration;
            radius = 1000;
        }

        public override void Do(EventItem item)
        {
            affectedBuildings = GetBuildingsInRadius(item.debuffCircle.transform.position, radius);
            foreach (var b in affectedBuildings)
            {
                if (b is CircleBuilding building)
                {
                    building.reduceRate += 0.2f;
                }

                b.soldierMoveReduceRate += (1 - b.soldierMoveReduceRate) * 0.5f;
            }
        }

        public override void Undo(EventItem item)
        {
            foreach (var b in affectedBuildings)
            {
                if (!b)
                    continue;
                if (b is CircleBuilding building)
                {
                    building.reduceRate -= 0.2f;
                }

                b.soldierMoveReduceRate -= 1 - b.soldierMoveReduceRate;
            }
        }
    }

    public class StormEvent : DebuffEvent
    {
        public StormEvent(float duration)
        {
            name = "Storm";
            description = "Every area Affected by the storm reduces transfer speed by half";
            eventType = EventType.Area;
            color = Color.cyan;
            this.duration = duration;
            radius = 350;
        }

        public override void Do(EventItem item)
        {
            affectedBuildings = GetBuildingsInRadius(item.debuffCircle.transform.position, radius);
            foreach (var b in affectedBuildings)
            {
                b.soldierMoveReduceRate += (1 - b.soldierMoveReduceRate) * 0.5f;
            }
        }

        public override void Undo(EventItem item)
        {
            foreach (var b in affectedBuildings)
            {
                if (!b)
                    continue;
                b.soldierMoveReduceRate -= 1 - b.soldierMoveReduceRate;
            }

            EventManager.instance.CallFloodEvent(affectedBuildings.Where(b => b).ToList());
        }
    }

    public class FloodEvent : DebuffEvent
    {
        public FloodEvent()
        {
            name = "Flood";
            description = "Every area affected by the flood reduces half max storage and may cause irreversible effects";
            eventType = EventType.Area;
            color = Color.blue;
            duration = 10;
            radius = 250;
        }

        public override void Do(EventItem item)
        {
            affectedBuildings = GetBuildingsInRadius(item.debuffCircle.transform.position, radius);
            foreach (var b in affectedBuildings)
            {
                b.MaxSoldiers /= 2;
                b.CurrentSoldiers = Mathf.Min(b.CurrentSoldiers, b.MaxSoldiers);
            }
        }

        public override void Undo(EventItem item)
        {
            foreach (var b in affectedBuildings)
            {
                if (!b)
                    continue;
                b.MaxSoldiers *= 2;
            }
        }
    }

    public class EarthquakeEvent : DebuffEvent
    {
        public EarthquakeEvent()
        {
            name = "Earthquake";
            description = "Destroy every area";
            eventType = EventType.Area;
            color = Color.black;
            duration = 0;
            radius = 250;
        }

        public override void Do(EventItem item)
        {
            affectedBuildings = GetBuildingsInRadius(item.debuffCircle.transform.position, radius);
            foreach (var b in affectedBuildings)
            {
                b.Destroyed();
            }
        }

        public override void Undo(EventItem item)
        {
        }
    }
}