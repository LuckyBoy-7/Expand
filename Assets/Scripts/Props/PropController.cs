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

        protected void Awake()
        {
            instance = this;
        }

        public RectTransform panel;
        public float popupDist = 100;
        public float popupDuraiton = 0.5f;
        public bool isDragging = false;
        private bool isOut = false;


        protected override void Start()
        {
            base.Start();
            RectTransform.sizeDelta = RectTransform.sizeDelta.WithX(popupDist);
        }

        protected override void Update()
        {
            base.Update();
            if (isOut && !isDragging && !IsPositionInBounds(GameCursor.MouseScreenPos))
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