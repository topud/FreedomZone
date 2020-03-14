using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class ItemTrigger : MonoBehaviour
    {
        private Role character;

        private void Update()
        {
            if (!character)
            {
                character = transform.GetComponentInParent<Role>();
            }
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
                    if (character.IsPlayer)
                    {
                        Role.onNearbyItemChange.Invoke();
                    }
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
                if (character.IsPlayer)
                {
                    Role.onNearbyItemChange.Invoke();
                }
            }
        }
    }
}