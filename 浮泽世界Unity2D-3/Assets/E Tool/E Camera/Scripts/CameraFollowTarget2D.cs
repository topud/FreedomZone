using UnityEngine;

namespace E.Tool
{
    public class CameraFollowTarget2D : MonoBehaviour
    {
        [Header("²¶×½µ½ÏñËØÍø¸ñ")]
        public CameraPixelDensity pixelDensity;
        public bool snapToGrid = true;

        [Header("¸úËæÄ¿±ê")]
        public Transform target;
        public Vector2 offset = Vector2.zero;

        [Header("×èÄá")]
        public float damp = 5;

        private void FixedUpdate()
        {
            if (!target) return;

            // calculate goal position
            Vector2 goal = (Vector2)target.position + offset;
            Vector2 position = Vector2.Lerp(transform.position, goal, Time.deltaTime * damp);

            // snap to grid, so it's always in multiples of 1/16 for pixel perfect looks
            // and to prevent shaking effects of moving objects etc.
            if (snapToGrid)
            {
                float gridSize = pixelDensity.pixelsToUnits * pixelDensity.zoom;
                position.x = Mathf.Round(position.x * gridSize) / gridSize;
                position.y = Mathf.Round(position.y * gridSize) / gridSize;
            }

            // convert to 3D but keep Z to stay in front of 2D plane
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}