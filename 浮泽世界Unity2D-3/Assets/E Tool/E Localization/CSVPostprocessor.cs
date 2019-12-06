﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
namespace E.Tool
{
    public class CSVPostprocessor : AssetPostprocessor
    {
        public static string[] localizationCSVFiles = new string[]
        {
            "Localizations.csv",
        };

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                //本地化CSV文件导入时自动序列化
                foreach (string item in localizationCSVFiles)
                {
                    if (str.IndexOf("/" + item) != -1)
                    {
                        TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                        string assetfile = str.Replace(".csv", ".asset");
                        Localizations gm = AssetDatabase.LoadAssetAtPath<Localizations>(assetfile);
                        if (gm == null)
                        {
                            gm = ScriptableObject.CreateInstance<Localizations>();
                            AssetDatabase.CreateAsset(gm, assetfile);
                        }

                        gm.strings = CSVSerializer.Deserialize<Localization>(data.text);

                        EditorUtility.SetDirty(gm);
                        AssetDatabase.SaveAssets();
                        Debug.Log("资产已重新导入：" + str);
                    }
                }
            }
        }

        [MenuItem("Tools/E Localization/CSV to Asset")]
        public static void Deserialize()
        {
            Selection.activeObject.GetInstanceID();
            string[] strs = Selection.assetGUIDs;

            string path = AssetDatabase.GUIDToAssetPath(strs[0]);

            if (path.IndexOf("/sample.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                string assetfile = path.Replace(".csv", ".asset");
                CSVExample gm = AssetDatabase.LoadAssetAtPath<CSVExample>(assetfile);
                if (gm == null)
                {
                    gm = ScriptableObject.CreateInstance<CSVExample>();
                    AssetDatabase.CreateAsset(gm, assetfile);
                }

                gm.m_Sample = CSVSerializer.Deserialize<CSVExample.Sample>(data.text);

                EditorUtility.SetDirty(gm);
                AssetDatabase.SaveAssets();
                Debug.Log("资产已重新导入：" + path);
            }
        }
    }
}
#endif