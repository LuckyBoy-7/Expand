using Lucky.Extensions;
using UnityEngine;

namespace Buildings
{
    public class CircleBuilding : Building
    {
        public float produceDuration = 1f;
        public float produceElapse = 0;
        public int ProduceNumber => (int)((0.5f + CurrentSoldiers / 80f) * BuildingsManager.instance.produceSpeedMultiplier * (1 - reduceRate));
        public float reduceRate = 0;

        protected override void Awake()
        {
            base.Awake();
            soldierMoveSpeed = 2;
            MaxSoldiers = 35;
        }

        protected override void Update()
        {
            base.Update();
            // 没兵就不造
            if (CurrentSoldiers == 0 || CurrentSoldiers == MaxSoldiers)
            {
                produceElapse = 0;
                return;
            }

            produceElapse += Time.deltaTime;
            if (produceElapse > produceDuration)
            {
                produceElapse -= produceDuration;

                // todo: 矛盾，如果兵正在路上，那么粮田是否应该造兵
                CurrentSoldiers += Mathf.Min(MaxSoldiers - possibleSoldiers, ProduceNumber);
            }
        }
    }
}