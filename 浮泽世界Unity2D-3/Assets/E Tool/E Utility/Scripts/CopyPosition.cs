using UnityEngine;

namespace E.Tool
{
    public class CopyPosition : MonoBehaviour
    {
        public bool x, y, z;
        public bool xScale, yScale, zScale;
        public Transform target;

        private void Update()
        {
            //默认跟随玩家
            if (!target)
            {
                if (Character.Player)
                {
                    target = Character.Player.transform;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!target) return;
            transform.position = new Vector3(
                (x ? target.position.x : transform.position.x),
                (y ? target.position.y : transform.position.y),
                (z ? target.position.z : transform.position.z));

            transform.localScale = new Vector3(
                (x ? target.lossyScale.x : transform.lossyScale.x),
                (y ? target.lossyScale.y : transform.lossyScale.y),
                (z ? target.lossyScale.z : transform.lossyScale.z));

        }
    }
}