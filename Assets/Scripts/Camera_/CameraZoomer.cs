using UnityEngine;
using UnityEngine.Serialization;

namespace Camera_
{
    public class CameraZoomer : MonoBehaviour
    {
        public float delta = 10;
        public float minSize = 100;
        public float maxSize = 1000;

        private void Update()
        {
            
            var camera = Camera.main;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - Input.mouseScrollDelta.y * delta, minSize, maxSize);
        }
    }
}