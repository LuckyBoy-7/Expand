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

        public override bool PositionInBounds(Vector2 pos)
        {
            float width = rectTransform.sizeDelta.x * rectTransform.localScale.x;
            float height = rectTransform.sizeDelta.y * rectTransform.localScale.y;
            Vector2 pivot = rectTransform.pivot;
            float x = rectTransform.position.x - pivot.x * width;
            float y = rectTransform.position.y - pivot.y * height;
            if (Input.mousePosition.x <= x + width
                && Input.mousePosition.x >= x
                && Input.mousePosition.y <= y + height
                && Input.mousePosition.y >= y)
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
            GameCursor.Instance.InteractableScreenUIs.Remove(this);
        }
    }
}