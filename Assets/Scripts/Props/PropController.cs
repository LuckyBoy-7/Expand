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
        public RectTransform panel;
        public float popupDist = 100;
        public float popupDuraiton = 0.5f;

        private void Start()
        {
            rectTransform.sizeDelta = rectTransform.sizeDelta.WithX(popupDist);
        }


        protected override void OnCursorEnterBounds()
        {
            base.OnCursorEnterBounds();
            panel.DOAnchorPosX(0, popupDuraiton);
        }

        protected override void OnCursorExitBounds()
        {
            base.OnCursorExitBounds();
            panel.DOAnchorPosX(-popupDist, popupDuraiton);
        }
    }
}