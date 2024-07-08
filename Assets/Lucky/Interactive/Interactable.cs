using System;
using UnityEngine;

namespace Lucky.Interactive
{
    public class Interactable : InteractableBase
    {
        private Collider2D collider;

        public Collider2D Collider
        {
            get
            {
                if (collider == null)
                    collider = GetComponent<Collider2D>();
                return collider;
            }
        }

        private SpriteRenderer renderer;

        public SpriteRenderer Renderer
        {
            get
            {
                if (renderer == null)
                    renderer = GetComponent<SpriteRenderer>();
                return renderer;
            }
        }

        protected override SortingLayerType sortingLayerType => SortingLayerType.Default;
        // SortingLayer.GetLayerValueFromID，通过id返回对应的层序号，越高的层序号越大从0开始
        protected override long SortingOrder => (SortingLayer.GetLayerValueFromID(renderer.sortingLayerID) + OffsetSortingLayer) * 10000 + renderer.sortingOrder;
        public override bool IsPositionInBounds(Vector2 pos, RectTransform trans = null) => collider.OverlapPoint(pos);
        public override Vector2 BoundsCheckPos => GameCursor.MouseWorldPos;
    }
}