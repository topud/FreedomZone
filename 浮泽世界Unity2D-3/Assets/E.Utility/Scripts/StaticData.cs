using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace E.Utility
{
    public abstract class StaticData : StaticDataDictionary<StaticData>
    {
        [SerializeField, Tooltip("名称")] private new string name = "";
        [SerializeField, Tooltip("描述"), TextArea(1, 30)] private string describe = "";
        [SerializeField, Tooltip("图标")]  private Sprite icon = null;
        [SerializeField, Tooltip("预制体")] private GameObject prefab = null;
        [SerializeField, Tooltip("是否无敌")] private bool invincible = false;

        public string Name { get => name; }
        public string Describe { get => describe; }
        public Sprite Icon { get => icon; }
        public GameObject Prefab { get => prefab; }
        public bool Invincible { get => invincible; }
    }
    public abstract class StaticDataDictionary<T> : ScriptableObject where T : ScriptableObject
    {
        private static Dictionary<int, T> dictionary;
        public static Dictionary<int, T> Dictionary
        {
            get
            {
                if (dictionary == null)
                {
                    LoadDictionary();
                }
                return dictionary;
            }
        }

        /// <summary>
        /// 重新获取资源字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, T> ReGetDictionary()
        {
            LoadDictionary();
            return Dictionary;
        }
        /// <summary>
        /// 获取资源字典的值
        /// </summary>
        /// <returns></returns>
        public static List<T> GetDictionaryValues()
        {
            List<T> values = new List<T>();
            values.AddRange(Dictionary.Values);
            return values;
        }
        /// <summary>
        /// 重新获取资源字典的值
        /// </summary>
        /// <returns></returns>
        public static List<T> ReGetDictionaryValues()
        {
            List<T> values = new List<T>();
            values.AddRange(ReGetDictionary().Values);
            return values;
        }
        /// <summary>
        /// 从资源目录载入所有该类型的资源
        /// </summary>
        private static void LoadDictionary()
        {
            T[] items = Resources.LoadAll<T>("");

            // 检查同名对象
            List<string> duplicates = items.ToList().FindDuplicates(item => item.name);
            if (duplicates.Count == 0)
            {
                dictionary = items.ToDictionary(item => item.name.GetStableHashCode(), item => item);
            }
            else
            {
                foreach (string duplicate in duplicates)
                {
                    Debug.LogError("Resources文件夹内包含多个同名的 {" + typeof(T).Name + "} {" + duplicate + "}，如果它们在不同的子文件夹，请将它们的名称前加上 “（子文件夹名）”以区分。");
                }
            }
        }
    }
}
