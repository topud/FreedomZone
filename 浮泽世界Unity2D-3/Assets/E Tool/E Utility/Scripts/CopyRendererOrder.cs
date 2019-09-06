using UnityEngine;

namespace E.Tool
{
    public class CopyRendererOrder : MonoBehaviour
    {
        public Renderer self;
        public Renderer target;
        public int offset = 0;

        private void Update()
        {
            if (!target || !self) return;
            self.sortingOrder = target.sortingOrder + offset;
        }
    }
}