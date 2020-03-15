using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class SingletonClass<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Singleton { get; protected set; }

        protected virtual void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this as T;
            }
            else
            {
                Destroy(this);
            }
        }

        public static bool IsExist
        {
            get
            {
                return Singleton != null;
            }
        }
    }
}