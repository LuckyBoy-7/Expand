using System;
using DG.Tweening;
using Lucky.Managers;
using TMPro;
using UnityEngine;

namespace Events
{
    public class EventHint : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public float activeDuration = 10f;
        public bool over;

        private void Update()
        {
            if (over)
                return;
            activeDuration -= Time.unscaledDeltaTime;
            if (activeDuration < 0)
            {
                Withdraw(1f);
                over = true;
            }
        }

        public void PopUp(float duration)
        {
            GetComponent<RectTransform>().DOAnchorPosX(0, duration).SetUpdate(true);
        }

        public void Withdraw(float duration)
        {
            GetComponent<RectTransform>().DOAnchorPosX(1000, duration).SetUpdate(true).onComplete += () =>
            {
                EventHintController.instance.OnHintOver(this);
                Destroy(gameObject);
            };
        }
    }
}