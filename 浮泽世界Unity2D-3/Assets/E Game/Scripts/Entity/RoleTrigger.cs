using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class RoleTrigger : MonoBehaviour
    {
        private Role role;

        private void Update()
        {
            if (!role)
            {
                role = transform.GetComponentInParent<Role>();
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            //加入列表
            Role target = col.GetComponent<Role>();
            if (target)
            {
                if (!role.NearbyRoles.Contains(target))
                {
                    role.NearbyRoles.Add(target);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            //移出列表
            Role target = col.GetComponent<Role>();
            if (target)
            {
                role.NearbyRoles.Remove(target);
            }

            //隐藏名称
            //if (character.IsPlayer)
            //{
            //    if (target)
            //    {
            //        target.TargetUI.HideAll();
            //        character.TargetUI.HideAll();
            //    }
            //}
        }
    }
}