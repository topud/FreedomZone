using UnityEngine;

namespace E.Tool
{
    [ExecuteInEditMode]
    public class CameraPixelDensity : MonoBehaviour
    {
        [Tooltip("��λ������������")] public float pixelsToUnits = 16;
        [Tooltip("����ϵ��")] public float zoom = 1;

        private void Update()
        {
            GetComponent<Camera>().orthographicSize = Screen.height / pixelsToUnits / zoom / 2;
        }
    }
}