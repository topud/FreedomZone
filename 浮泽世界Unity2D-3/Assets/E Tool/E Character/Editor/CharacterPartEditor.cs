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
                case PartType.物品:
                    Target.ItemType = (ItemType)EditorGUILayout.EnumPopup("物品部件", Target.ItemType);
                    break;
                default:
                    break;
            }
        }
    }
}