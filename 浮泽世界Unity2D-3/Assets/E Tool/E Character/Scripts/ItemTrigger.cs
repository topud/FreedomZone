﻿using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class ItemTrigger : MonoBehaviour
    {
        [ReadOnly, SerializeField] private Character character;

        private void Awake()
        {
            character = transform.GetComponentInParent<Character>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            //加入列表
            Item item = col.GetComponent<Item>();
            if (item)
            {
                if (!character.NearbyItems.Contains(item))
                {
                    character.NearbyItems.Add(item);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            //移出列表
            Item item = col.GetComponent<Item>();
            if (item)
            {
                character.NearbyItems.Remove(item);
            }

            //隐藏名称
            if (character.GetType() == typeof(Player))
            {
                if (item)
                {
                    item.TargetUI.HideAll();
                    character.TargetUI.HideAll();
                }
            }
        }
    }
}