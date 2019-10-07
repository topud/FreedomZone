using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class BuildingTrigger : MonoBehaviour
    {
        private Character character;

        private void Update()
        {
            if (!character)
            {
                character = transform.GetComponentInParent<Character>();
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            //半透明化
            Building target = col.GetComponent<Building>();
            if (target && character.transform.position.y > target.transform.position.y)
            {
                target.SpriteSorter.SetAlpha(0.5f);
            }
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            //半透明化
            Building target = col.GetComponent<Building>();
            if (target)
            {
                if (character.transform.position.y > target.transform.position.y)
                {
                    target.SpriteSorter.SetAlpha(0.5f);
                }
                else
                {
                    target.SpriteSorter.SetAlpha(1);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            //取消半透明化
            Building target = col.GetComponent<Building>();
            if (target)
            {
                target.SpriteSorter.SetAlpha(1);
            }
        }
    }
}