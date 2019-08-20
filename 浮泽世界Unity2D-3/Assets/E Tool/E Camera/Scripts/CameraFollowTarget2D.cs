using UnityEngine;

namespace E.Tool
{
    public class CameraFollowTarget2D : MonoBehaviour
    {
        [Header("捕捉到像素网格")]
        public CameraPixelDensity pixelDensity;
        public bool snapToGrid = true;

        [Header("跟随目标")]
        public Transform target;
        public Vector2 offset = Vector2.zero;

        [Header("阻尼")]
        public float damp = 5;

        [ExecuteInEditMode]
        private void Update()
        {
            //默认跟随玩家
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

            // 计算目标地点
            Vector2 goal = (Vector2)target.position + offset;
            Vector2 position = Vector2.Lerp(transform.position, goal, Time.deltaTime * damp);

            // 吸附网格
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