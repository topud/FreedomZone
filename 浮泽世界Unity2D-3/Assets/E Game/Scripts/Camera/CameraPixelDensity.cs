using UnityEngine;

namespace E.Tool
{
    [ExecuteInEditMode]
    public class CameraPixelDensity : MonoBehaviour
    {
        [Tooltip("单位距离像素数量")] public float pixelsToUnits = 16;
        [Tooltip("缩放系数")] public float zoom = 1;

        private void Update()
        {
            GetComponent<Camera>().orthographicSize = Screen.height / pixelsToUnits / zoom / 2;
        }
    }
}