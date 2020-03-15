using UnityEngine;

namespace E.Tool
{
    public class AutoDestroy : MonoBehaviour
    {
        public float time = 1;

        private void Start()
        {
            Destroy(gameObject, time);
        }
    }
}