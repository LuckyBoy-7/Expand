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
            color = Color.blue;
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
            radius = 400;
            
        }

        public override void Do(EventItem item)
        {
            foreach (var b in GetBuildingsInRadius(item.transform.position, radius))
            {
                if (b is CircleBuilding building)
                {
                    building.reduceRate += 0.2f;
                    building.soldierMoveReduceRate += (1 - building.soldierMoveReduceRate) * 0.5f;
                }
            }
        }

        public override void Undo(EventItem item)
        {
            foreach (var b in GetBuildingsInRadius(item.transform.position, radius))
            {
                if (b is CircleBuilding building)
                {
                    building.reduceRate -= 0.2f;
                    building.soldierMoveReduceRate -= 1 - building.soldierMoveReduceRate;
                }
            }
        }
    }
    
    public class StormFlood : DebuffEvent
    {
        public StormFlood()
        {
            name = "Storm";
            description = "Every area Affected by the storm reduces transfer speed by half";
            eventType = EventType.Area;
            color = Color.cyan;
            duration = 10;
            radius = 700;
            
        }

        public override void Do(EventItem item)
        {
            foreach (var b in GetBuildingsInRadius(item.transform.position, radius))
            {
                if (b is CircleBuilding building)
                {
                    building.soldierMoveReduceRate += (1 - building.soldierMoveReduceRate) * 0.5f;
                }
            }
        }

        public override void Undo(EventItem item)
        {
            foreach (var b in GetBuildingsInRadius(item.transform.position, radius))
            {
                if (b is CircleBuilding building)
                {
                    building.soldierMoveReduceRate -= 1 - building.soldierMoveReduceRate;
                }
            }
        }
    }
}