using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class CharacterNearby : MonoBehaviour
    {
        [ReadOnly, SerializeField] private Character character;

        private void Awake()
        {
            character = transform.parent.GetComponent<Character>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            Item item = col.GetComponent<Item>();
            if (item != null)
            {
                item.CharacterUI.ShowName();
                if (!character.NearbyItems.Contains(item))
                {
                    character.NearbyItems.Add(item);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            Item item = col.GetComponent<Item>();
            if (item != null)
            {
                item.CharacterUI.HideName();
                character.NearbyItems.Remove(item);
            }
        }
    }
}