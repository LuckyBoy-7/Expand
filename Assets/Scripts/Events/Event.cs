using UnityEngine;

namespace Events
{
    public class Event
    {
        public string name;
        public string description;
    }

    public class AttackEvent : Event
    {
        public int enemyNumber;
    }

    public class DebuffEvent : Event
    {
        public float duration;

        public virtual void Do()
        {
        }

        public virtual void Undo()
        {
        }
    }


    public class WeakBanditEvent : AttackEvent
    {
        public WeakBanditEvent()
        {
            name = "WeakBandit";
            description = "Bandits appear to attack this area";
            enemyNumber = Random.Range(5, 9);
        }
    }

    public class StrongBanditEvent : AttackEvent
    {
        public StrongBanditEvent()
        {
            name = "StrongBandit";
            description = "Strong Bandits appear to attack this area";
            enemyNumber = Random.Range(5, 9) * Random.Range(2, 5);
        }
    }
    //
    // public class DroughtEvent : AttackEvent
    // {
    //     public DroughtEvent()
    //     {
    //         name = "Drought";
    //         description = "Every area Affected by the drought reduces transfer speed by half，And Castle reduce 20% produce rate";
    //         enemyNumber = Random.Range(5, 9) * Random.Range(2, 5);
    //     }
    // }
}