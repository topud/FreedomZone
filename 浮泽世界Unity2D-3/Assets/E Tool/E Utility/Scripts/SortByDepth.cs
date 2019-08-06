using UnityEngine;

namespace E.Tool
{
    public class SortByDepth : MonoBehaviour
    {
        /// <summary>
        /// 渲染器
        /// </summary>
        [SerializeField] private SpriteRenderer[] Renderers;
        /// <summary>
        /// 精确度
        /// </summary>
        [SerializeField] private int precision = 100;
        /// <summary>
        /// 偏移
        /// </summary>
        [SerializeField] private int offset = 0;

        private void Update()
        {
            int order = -Mathf.RoundToInt((transform.position.y + offset) * precision);
            foreach (SpriteRenderer item in Renderers)
            {
                item.sortingOrder = order;
            }
        }

        public void SetColor(Color color)
        {
            foreach (SpriteRenderer item in Renderers)
            {
                item.color = color;
            }
        }
    }
}