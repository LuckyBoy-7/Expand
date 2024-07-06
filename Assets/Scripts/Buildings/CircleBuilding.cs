using UnityEngine;

namespace Buildings
{
    public class CircleBuilding : Building
    {
        public float produceDuration = 1f;
        public float produceElapse = 0;
        public float produceRate => 1 * Mathf.Max(0, 1 - reduceRate);
        public float reduceRate = 0;

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
                CurrentSoldiers += Mathf.Min(MaxSoldiers - possibleSoldiers, (int)(CurrentSoldiers * produceRate));
            }
        }
    }
}