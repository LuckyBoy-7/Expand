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
        public float activeDuration = 3f;
        public bool over;

        private void Update()
        {
            if (over)
                return;
            activeDuration -= Time.deltaTime;
            if (activeDuration < 0)
            {
                Withdraw(0.5f);
                over = true;
            }
        }

        public void PopUp(float duration)
        {
            GetComponent<RectTransform>().DOAnchorPosX(0, duration);
        }

        public void Withdraw(float duration)
        {
            GetComponent<RectTransform>().DOAnchorPosX(1000, duration).onComplete += () =>
            {
                EventHintController.instance.OnHintOver(this);
                Destroy(gameObject);
            };
        }


        public void MoveTo(Vector3 pos, float duration)
        {
            GetComponent<RectTransform>().DOAnchorPos(pos, duration);
        }
    }
}