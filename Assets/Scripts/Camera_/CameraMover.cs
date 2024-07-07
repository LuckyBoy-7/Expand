using Lucky.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera_
{
    public class CameraMover : MonoBehaviour
    {
        public float minSpeed = 100;
        public float maxSpeed = 200;

        public float minSize = 100;
        public float maxSize = 1000;

        private void Update()
        {
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var camera = Camera.main;
            float k = (camera.orthographicSize - minSize) / (maxSize - minSize);
            k = Ease.SineEaseOut(1 - Mathf.Clamp(k, 0, 1));
            Vector3 speed = dir.normalized * (minSpeed + (maxSpeed - minSpeed) * k);
            transform.position += speed * Time.unscaledDeltaTime;
        }
    }
}