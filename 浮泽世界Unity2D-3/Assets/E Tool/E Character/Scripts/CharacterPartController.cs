using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class CharacterPartController : MonoBehaviour
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
        /// 服装部件
        /// </summary>
        [SerializeField] private List<SpriteRenderer> ClothParts;
        /// <summary>
        /// 饰品部件
        /// </summary>
        [SerializeField] private List<SpriteRenderer> DecorationParts;

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
            ClothParts = new List<SpriteRenderer>();
            DecorationParts = new List<SpriteRenderer>();
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
                    case PartType.服装:
                        ClothParts.Add(item);
                        break;
                    case PartType.饰品:
                        DecorationParts.Add(item);
                        break;
                    default:
                        break;
                }
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