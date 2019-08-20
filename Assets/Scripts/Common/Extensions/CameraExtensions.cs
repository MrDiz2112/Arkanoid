using UnityEngine;

namespace Common.Extensions
{
    public static class CameraExtensions
    {
        public static float GetHeight(this Camera camera)
        {
            return 2.0f * camera.orthographicSize;
        }
        
        public static float GetWidth(this Camera camera)
        {
            var height = camera.GetHeight();
            return height * camera.aspect;
        }
    }
}