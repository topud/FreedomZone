using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class LookAt : MonoBehaviour
    {
        public LookAtItem lookAtItem = LookAtItem.Cursor;
        public GameObject target;
        private Vector3 mpos;
        private Vector3 lookPos;

        private void Update()
        {
            switch (lookAtItem)
            {
                case LookAtItem.Cursor:
                    mpos = Input.mousePosition;
                    mpos = Camera.main.ScreenToWorldPoint(mpos);
                    lookPos = new Vector3(mpos.x, mpos.y, 0.1f);

                    transform.LookAt(lookPos);
                    transform.Rotate(new Vector3(0, 1, 0), 90);
                    transform.Rotate(new Vector3(0, 0, -1), -90);
                    //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

                    break;
                case LookAtItem.Gameobject:
                    break;
                default:
                    break;
            }
        }

        public enum LookAtItem
        {
            Cursor,
            Gameobject
        }
    }
}