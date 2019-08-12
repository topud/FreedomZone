using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class CharacterSprite : MonoBehaviour
    {
        /// <summary>
        /// 渲染器
        /// </summary>
        [SerializeField] private SpriteRenderer[] Renderers;
        /// <summary>
        /// 身体部件
        /// </summary>
        [SerializeField] private List<SpriteRenderer> BodyParts;
        /// <summary>
        /// 物品部件
        /// </summary>
        [SerializeField] private List<SpriteRenderer> ItemParts;

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

            BodyParts = new List<SpriteRenderer>();
            ItemParts = new List<SpriteRenderer>();
            foreach (SpriteRenderer item in Renderers)
            {
                CharacterPart part = item.GetComponent<CharacterPart>();
                if (!part)
                {
                    Debug.LogError("未给 " + item.name + " 添加 CharacterPart 组件");
                    continue;
                }
                switch (part.PartType)
                {
                    case PartType.身体:
                        BodyParts.Add(item);
                        break;
                    case PartType.物品:
                        ItemParts.Add(item);
                        break;
                    default:
                        break;
                }
            }
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
        /// <summary>
        /// 设置身体部件颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color, BodyType bodyType)
        {
            foreach (SpriteRenderer item in BodyParts)
            {
                if (item.GetComponent<CharacterPart>().BodyType == bodyType)
                {
                    item.color = color;
                }
            }
        }
        /// <summary>
        /// 设置物品部件颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color, ItemType bodyType)
        {
            foreach (SpriteRenderer item in ItemParts)
            {
                if (item.GetComponent<CharacterPart>().ItemType == bodyType)
                {
                    item.color = color;
                }
            }
        }
    }
}