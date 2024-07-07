using System;
using Lucky.Interactive;
using UnityEngine;
using UnityEngine.UI;

namespace Props
{
    public class PropUI : InteractableScreenUI
    {
        private Image image;
        private Image imagePrefab;
        public Image ghostImage;

        public enum PropType
        {
            Square
        }

        public PropType propType;

        protected override void Awake()
        {
            base.Awake();
            imagePrefab = Resources.Load<Image>("Prefabs/PropImage");
            image = GetComponent<Image>();
            if (propType == PropType.Square)
                image.sprite = Resources.Load<Sprite>("Prefabs/SquareBuilding");
        }

        protected override void OnCursorPress()
        {
            base.OnCursorPress();
            ghostImage = Instantiate(imagePrefab, transform);
            if (propType == PropType.Square)
                ghostImage.sprite = Resources.Load<Sprite>("Prefabs/SquareBuilding");
        }

        private void Update()
        {
            if (ghostImage)
                ghostImage.transform.position = GameCursor.MouseWorldPos;
        }

        protected override void OnCursorRelease()
        {
            base.OnCursorRelease();
            if (PositionInBounds(GameCursor.MouseScreenPos))
            {
                Destroy(ghostImage);
            }
            else
            {
                Destroy(gameObject);
                if (propType == PropType.Square)
                    BuildingsManager.instance.SpawnBuilding(Resources.Load<Building>("Prefabs/SquareBuilding"), GameCursor.MouseWorldPos);
            }
        }
    }
}