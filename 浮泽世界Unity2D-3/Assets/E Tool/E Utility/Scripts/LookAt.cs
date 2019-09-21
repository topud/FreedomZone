using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class LookAt : MonoBehaviour
    {
        public Transform self;
        public GameObject target;
        public LookAtItem lookAtItem = LookAtItem.Cursor;
        private Vector3 mpos;
        private Vector3 lookPos;

        private void Update()
        {
            if (!self) self = transform;

            switch (lookAtItem)
            {
                case LookAtItem.Cursor:
                    mpos = Input.mousePosition;
                    mpos = Camera.main.ScreenToWorldPoint(mpos);
                    lookPos = new Vector3(mpos.x, mpos.y, 0);

                    self.LookAt(lookPos);
                    self.Rotate(new Vector3(0, 1, 0), 90);
                    self.Rotate(new Vector3(0, 0, -1), -90);

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