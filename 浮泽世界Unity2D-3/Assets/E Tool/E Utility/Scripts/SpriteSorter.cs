using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class SpriteSorter : MonoBehaviour
    {
        /// <summary>
        /// 渲染器
        /// </summary>
        [SerializeField] private SpriteRenderer[] Renderers;

        /// <summary>
        /// 图片精确度
        /// </summary>
        [SerializeField] private int precision = 100;
        /// <summary>
        /// 偏移
        /// </summary>
        [SerializeField] private int offset = 0;

        private void Update()
        {
            //更新渲染层次
            int order = -Mathf.RoundToInt((transform.position.y + offset) * precision);
            foreach (SpriteRenderer item in Renderers)
            {
                item.sortingOrder = order;
            }
        }
        private void Reset()
        {
            Renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        /// <summary>
        /// 设置整体透明度
        /// </summary>
        /// <param name="color"></param>
        public void SetAlpha(float alpha)
        {
            foreach (SpriteRenderer item in Renderers)
            {
                alpha = Mathf.Clamp01(alpha);
                item.color = new Color(item.color.r, item.color.g, item.color.b, alpha);
            }
        }
        /// <summary>
        /// 设置整体颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            foreach (SpriteRenderer item in Renderers)
            {
                item.color = color;
            }
        }
    }
}