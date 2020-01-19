using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.AddressableAssets;

public class ItemManager : MonoBehaviour
{
    [Header("组件")]
    public GameObject ItemPrefab;

    [Header("数据")]
    [SerializeField, ReadOnly] private List<Item> items = new List<Item>();

    public List<Item> Items
    {
        get
        {
            items.Clear();
            Item[] its = transform.GetComponentsInChildren<Item>();
            items.AddRange(its);
            return items;
        }
    }
    public int AvailableID 
    {
        get
        {
            int max = short.MaxValue;
            int i = 0;
            do
            {
                bool isExsit = false;
                foreach (Item item in Items)
                {
                    if (i == item.DynamicData.nameID.id)
                    {
                        isExsit = true;
                        break;
                    }
                }
                if (isExsit)
                {
                    i++;
                }
                else
                {
                    break;
                }
            } while (i < max);
            return i;
        }
    }


    private void OnEnable()
    {
        items = Items;
    }

    /// <summary>
    /// 生成物品
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Item SpawnItem(string name, Vector2 position)
    {
        GameObject go;
        Item item;
        ItemStaticData sData = Addressables.LoadAsset<ItemStaticData>(name).Result;
        if (sData)
        {
            go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), transform);
            item = go.GetComponent<Item>();
            item.Refresh();
            Debug.Log("物品生成成功：" + name);
            return item;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + name);
            return null;
        }
    }
    /// <summary>
    /// 生成物品
    /// </summary>
    public Item SpawnItem(ItemDynamicData dData)
    {
        GameObject go;
        Item item;
        ItemStaticData sData = Addressables.LoadAsset<ItemStaticData>(dData.nameID).Result;
        if (sData)
        {
            go = Instantiate(sData.Prefab, dData.position, new Quaternion(0, 0, 0, 0), transform);
            item = go.GetComponent<Item>();
            item.SetDynamicData(dData);
            Debug.Log("物品生成成功：" + dData.nameID.NameID);
            return item;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + dData.nameID.name);
            return null;
        }
    }
    /// <summary>
    /// 获取物品
    /// </summary>
    /// <param name="name">物品名</param>
    /// <returns></returns>
    public Item GetItem(NameAndID nameID)
    {
        foreach (Item item in Items)
        {
            if (item.DynamicData.nameID.Equals(nameID))
            {
                return item;
            }
        }
        return null;
    }
}
