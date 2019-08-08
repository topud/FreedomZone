// This component copies a Transform's position to automatically follow it,
// which is useful for the camera.
using UnityEngine;

namespace E.Tool
{
    public class CopyPosition : MonoBehaviour
    {
        public bool x, y, z;
        public Transform target;

        private void Update()
        {
            //Ä¬ÈÏ¸úËæÍæ¼Ò
            if (!target) target = Player.Myself.transform;
        }
        private void FixedUpdate()
        {
            if (!target) return;
            transform.position = new Vector3(
                (x ? target.position.x : transform.position.x),
                (y ? target.position.y : transform.position.y),
                (z ? target.position.z : transform.position.z));
        }
    }
}