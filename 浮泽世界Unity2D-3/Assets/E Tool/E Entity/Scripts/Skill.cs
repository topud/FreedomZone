using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace E.Tool
{
    [Serializable]
    public class Skill
    {
        [Tooltip("技能类型")] public SkillTpye skillTpye;
        [Tooltip("熟练等级")] public SkillLevel level;
        [Tooltip("等级经验"), Range(0, 1)] public float levelEx;

        /// <summary>
        /// 熟练等级经验
        /// </summary>
        public float LevelEx
        {
            get => levelEx;
            set
            {
                levelEx = value;
                if (levelEx >= 1)
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