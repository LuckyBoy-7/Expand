using System;
using DG.Tweening;
using Lucky.Extensions;
using Lucky.Interactive;
using Lucky.Managers;
using UnityEngine;

namespace Props
{
    public class PropController : InteractableScreenUI
    {
        public static PropController instance;
        public Transform ghostImageContainer;

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        public RectTransform panel;
        public float popupDist = 100;
        public float popupDuraiton = 0.5f;
        public bool isDragging = false;
        private bool isOut = false;
        

        private void Start()
        {
            rectTransform.sizeDelta = rectTransform.sizeDelta.WithX(popupDist);
        }

        protected override void Update()
        {
            base.Update();
            if (isOut && !isDragging && !PositionInBounds(GameCursor.MouseScreenPos))
            {
                panel.DOAnchorPosX(-popupDist, popupDuraiton);
                isOut = false;
            }
        }

        protected override void OnCursorEnterBounds()
        {
            base.OnCursorEnterBounds();
            panel.DOAnchorPosX(0, popupDuraiton);
            isOut = true;
        }

        protected override void OnCursorExitBounds()
        {
            base.OnCursorExitBounds();
            if (isDragging)
                return;
            panel.DOAnchorPosX(-popupDist, popupDuraiton);
            isOut = false;
        }
        
    }
}