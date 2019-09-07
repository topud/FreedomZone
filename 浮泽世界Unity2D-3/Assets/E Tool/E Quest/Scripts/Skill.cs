using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public struct Skill
    {
        [SerializeField, Tooltip("技能类型")] private SkillTpye skillTpye;
        [SerializeField, Tooltip("熟练等级")] private SkillLevel level;
        [SerializeField, Tooltip("熟练等级经验"), Range(0, 1)] private float levelEx;

        /// <summary>
        /// 技能类型
        /// </summary>
        public SkillTpye SkillTpye { get => skillTpye; set => skillTpye = value; }
        /// <summary>
        /// 熟练等级
        /// </summary>
        public SkillLevel Level { get => level; set => level = value; }
        /// <summary>
        /// 熟练等级经验
        /// </summary>
        public float LevelEx
        {
            get => levelEx;
            set
            {
                levelEx = value;
                if (levelEx >= 1 )
                {
                    if (level == SkillLevel.巅峰)
                    {
                        levelEx = 1;
                    }
                    else
                    {
                        levelEx = 0;
                        level += 1;
                    }
                }
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="type"></param>
        public Skill(SkillTpye type)
        {
            skillTpye = type;
            level = SkillLevel.生涩;
            levelEx = 0;
        }
    }

    public enum SkillLevel
    {
        生涩,
        熟练,
        大师,
        巅峰
    }

    public enum SkillTpye
    {
        语文,
        数学,
        英语,
        物理,
        化学,
        生物,
        历史,
        地理,
        政治,
        驾驶,
        厨艺,
        演技,
        声乐,
    }
}