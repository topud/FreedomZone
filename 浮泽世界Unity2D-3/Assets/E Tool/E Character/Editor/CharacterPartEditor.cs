// ========================================================
// 作者：E Star
// 创建时间：2019-03-10 17:03:03
// 当前版本：1.0
// 作用描述：自定义Inspector面板显示StoryContent的样式
// 挂载目标：无
// ========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace E.Tool
{
    [CustomEditor(typeof(CharacterPart))]
    public class CharacterPartEditor : Editor
    {
        private CharacterPart Target;

        private void OnEnable()
        {
            Target = (CharacterPart)target;
        }

        public override void OnInspectorGUI()
        {
            Target.PartType = (PartType)EditorGUILayout.EnumPopup("部件类型", Target.PartType);
            switch (Target.PartType)
            {
                case PartType.身体:
                    Target.BodyType = (BodyType)EditorGUILayout.EnumPopup("身体部件", Target.BodyType);
                    break;
                case PartType.服装:
                    Target.ClothType = (ClothType)EditorGUILayout.EnumPopup("服装部件", Target.ClothType);
                    break;
                case PartType.饰品:
                    Target.DecorationType = (DecorationType)EditorGUILayout.EnumPopup("饰品部件", Target.DecorationType);
                    break;
                default:
                    break;
            }
        }
    }
}