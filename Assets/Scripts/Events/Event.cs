using System.Collections.Generic;
using System.Linq;
using Buildings;
using UnityEngine;

namespace Events
{
    public class Event
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

        protected List<Building> GetBuildingsInRadius(Vector3 pos, float radius)
        {
            return BuildingsManager.instance.buildings.Where(b => (b.transform.position - pos).magnitude < radius).ToList();
        }
    }

    public class AttackEvent : Event
    {
        public int enemyNumber;
    }

    public abstract class DebuffEvent : Event
    {
        protected List<Building> affectedBuildings = new();
        public float duration;

        public abstract void Do(EventItem item);

        public abstract void Undo(EventItem item);
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
    }

    public class StrongBanditEvent : AttackEvent
    {
        public StrongBanditEvent()
        {
            name = "StrongBandit";
            description = "Strong Bandits appear to attack this area";
            eventType = EventType.Dot;
            color = Color.green;
            enemyNumber = Random.Range(5, 9) * Random.Range(2, 5);
        }
    }

    public class DroughtEvent : DebuffEvent
    {
        public DroughtEvent()
        {
            name = "Drought";
            description = "Every area Affected by the drought reduces transfer speed by half，And Castle reduces 20% produce rate";
            eventType = EventType.Area;
            color = Color.yellow;
            duration = 10;
            radius = 200;
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
                if (b)
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
        public StormEvent()
        {
            name = "Storm";
            description = "Every area Affected by the storm reduces transfer speed by half";
            eventType = EventType.Area;
            color = Color.cyan;
            duration = 10;
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
                if (b)
                    continue;
                b.soldierMoveReduceRate -= 1 - b.soldierMoveReduceRate;
            }
        }
    }

    public class FloodEvent : DebuffEvent
    {
        public FloodEvent()
        {
            name = "Flood";
            description = "Every area Affected by the flood reduces half max storage";
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
                if (b)
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