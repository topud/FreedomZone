using UnityEngine;

namespace E.Tool
{
    public class CopyPosition : MonoBehaviour
    {
        public Transform self;
        public Transform target;
        public bool x, y, z;
        public bool xScale, yScale, zScale;
        public Vector3 positionOffset;

        private void Update()
        {
            if (!target) return;
            if (!self) self = transform;

            self.position = new Vector3(
                (x ? target.position.x + positionOffset.x : transform.position.x),
                (y ? target.position.y + positionOffset.y : transform.position.y),
                (z ? target.position.z + positionOffset.z : transform.position.z));

            self.localScale = new Vector3(
                (x ? target.lossyScale.x : transform.lossyScale.x),
                (y ? target.lossyScale.y : transform.lossyScale.y),
                (z ? target.lossyScale.z : transform.lossyScale.z));
        }

        private void OnValidate()
        {
            Update();
        }
    }
}