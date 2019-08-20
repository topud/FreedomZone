using UnityEngine;

namespace E.Tool
{
    public class CameraFollowTarget2D : MonoBehaviour
    {
        [Header("��׽����������")]
        public CameraPixelDensity pixelDensity;
        public bool snapToGrid = true;

        [Header("����Ŀ��")]
        public Transform target;
        public Vector2 offset = Vector2.zero;

        [Header("����")]
        public float damp = 5;

        [ExecuteInEditMode]
        private void Update()
        {
            //Ĭ�ϸ������
            if (!target)
            {
                if (Player.Myself)
                {
                    target = Player.Myself.transform;
                }
            }
        }
        [ExecuteInEditMode]
        private void FixedUpdate()
        {
            if (!target) return;

            // ����Ŀ��ص�
            Vector2 goal = (Vector2)target.position + offset;
            Vector2 position = Vector2.Lerp(transform.position, goal, Time.deltaTime * damp);

            // ��������
            if (snapToGrid)
            {
                float gridSize = pixelDensity.pixelsToUnits * pixelDensity.zoom;
                position.x = Mathf.Round(position.x * gridSize) / gridSize;
                position.y = Mathf.Round(position.y * gridSize) / gridSize;
            }

            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}