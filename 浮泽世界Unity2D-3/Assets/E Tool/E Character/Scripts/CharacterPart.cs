using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharacterPart : MonoBehaviour
    {
        public PartType PartType = PartType.身体;
        public BodyType BodyType = BodyType.脸型;
        public ItemType ItemType = ItemType.上衣;
    }

    public enum PartType
    {
        身体,
        物品
    }
    public enum BodyType
    {
        头发,
        脸型,
        左眼,
        右眼,
        左眉,
        右眉,
        鼻子,
        嘴巴,
        耳朵,
        躯干,
        左臂,
        右臂,
        左手,
        右手,
        左腿,
        右腿,
        左脚,
        右脚,
    }

    public enum ItemType
    {
        帽子,
        面具,
        背包,
        上衣,
        左袖,
        右袖,
        左手套,
        右手套,
        左手物品,
        右手物品,
        左裤,
        右裤,
        左鞋,
        右鞋,
    }
}