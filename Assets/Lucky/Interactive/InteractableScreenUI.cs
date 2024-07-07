using System;
using UnityEngine;

namespace Lucky.Interactive
{
    public class InteractableScreenUI : InteractableBase
    {
        public RectTransform rectTransform;

        public long sortingLayer = 0;

        // warning: 这个不要忘了调，尤其是涉及到很多interactable的时候
        public override long SortingOrder => sortingLayer * 10000 + rectTransform.GetSiblingIndex();
        protected override Vector2 BoundsCheckPos => GameCursor.MouseScreenPos;

        public override bool PositionInBounds(Vector2 pos, RectTransform trans = null)
        {
            if (trans == null)
                trans = rectTransform;
            float width = trans.sizeDelta.x * trans.localScale.x;
            float height = trans.sizeDelta.y * trans.localScale.y;
            Vector2 pivot = trans.pivot;
            float x = trans.position.x - pivot.x * width;
            float y = trans.position.y - pivot.y * height;
            if (pos.x <= x + width
                && pos.x >= x
                && pos.y <= y + height
                && pos.y >= y)
                return true;
            return false;
        }


        protected override void Awake()
        {
            base.Awake();
            // ResetCollider();
            // 省事，不然每个都要手动调，由于很多时候ui的更新晚一点，所以这个最后改
            // 或者也可以就写个方法，然后子类调用就行
            // this.DoWaitUntilEndOfFrame(ResetCollider);

            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
                rectTransform = gameObject.AddComponent<RectTransform>();
        }

        private void OnEnable()
        {
            GameCursor.Instance.InteractableScreenUIs.Add(this);
        }

        private void OnDisable()
        {
            if (GameCursor.instance)
                GameCursor.Instance.InteractableScreenUIs.Remove(this);
        }
    }
}