using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;

namespace E.Tool
{
    public static class Utility
    {
        /// <summary>
        /// 单行高度
        /// </summary>
        public static int OneHeight
        {
            //18
            get => (int)EditorGUIUtility.singleLineHeight;
        }
        /// <summary>
        /// 行间隔
        /// </summary>
        public static int OneSpacing
        {
            //2
            get => (int)EditorGUIUtility.standardVerticalSpacing;
        }
        /// <summary>
        /// 标签宽度
        /// </summary>
        public static int LabelWidth
        {
            get => (int)EditorGUIUtility.labelWidth;
        }
        /// <summary>
        /// 输入宽度
        /// </summary>
        public static int FieldWidth
        {
            get => (int)EditorGUIUtility.fieldWidth;
        }
        /// <summary>
        /// 设置标签宽度
        /// </summary>
        /// <param name="width"></param>
        public static void SetLabelWidth(int width)
        {
            EditorGUIUtility.labelWidth = width;
        }
        /// <summary>
        /// 设置值最小宽度
        /// </summary>
        /// <param name="width"></param>
        public static void SetFieldWidth(int width)
        {
            EditorGUIUtility.fieldWidth = width;
        }

        /// <summary>
        /// 获取框架高度，包括上下间隔
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static int GetHeightLong(int lines)
        {
            return (OneHeight + OneSpacing) * lines + OneSpacing;
        }
        /// <summary>
        /// 获取框架高度，包括上间隔
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static int GetHeightMiddle(int lines)
        {
            return (OneHeight + OneSpacing) * lines;
        }
        /// <summary>
        /// 获取框架高度，不包括上下间隔
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static int GetHeightShort(int lines)
        {
            return (OneHeight + OneSpacing) * lines - OneSpacing;
        }

        /// <summary>
        /// 限定整型数字范围，仅包含最小值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int ClampMin(int value, int min)
        {
            if (value < min) return min;
            return value;
        }
        /// <summary>
        /// 限定浮点数字范围，仅包含最小值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static float ClampMin(float value, float min)
        {
            if (value < min) return min;
            return value;
        }
        /// <summary>
        /// 限定整型数字范围，仅包含最大值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int ClampMax(int value, int max)
        {
            if (value > max) return max;
            return value;
        }
        /// <summary>
        /// 限定浮点数字范围，仅包含最大值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampMax(float value, float max)
        {
            if (value > max) return max;
            return value;
        }
        /// <summary>
        /// 限定整型数字范围，包含最小值和最大值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        /// <summary>
        /// 限定浮点数字范围，包含最小值和最大值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// 获取数组中最小值的索引
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>最小值的索引</returns>
        public static int IndexMin(List<float> array)
        {
            float value = 0;
            int index = 0;
            bool hasValue = false;
            for (int i = 0; i < array.Count; i++)
            {
                if (hasValue)
                {
                    if (array[i] < value)
                    {
                        value = array[i];
                        index = i;
                    }
                }
                else
                {
                    value = array[i];
                    index = i;
                    hasValue = true;
                }
            }
            return index;
        }
        /// <summary>
        /// 获取时间数组中最新值的索引
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>最新值的索引</returns>
        public static int IndexLatest(List<DateTime> array)
        {
            DateTime value = DateTime.MinValue;
            int index = 0;
            bool hasValue = false;
            for (int i = 0; i < array.Count; i++)
            {
                if (hasValue)
                {
                    if (array[i] > value)
                    {
                        value = array[i];
                        index = i;
                    }
                }
                else
                {
                    value = array[i];
                    index = i;
                    hasValue = true;
                }
            }
            return index;
        }

        /// <summary>
        /// 移动对象
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void Move(Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        }
        /// <summary>
        /// 批量移动对象
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void Move(Transform[] transforms, float x, float y, float z)
        {
            foreach (Transform item in transforms)
            {
                item.position = new Vector3(item.position.x + x, item.position.y + y, item.position.z + z);
            }
        }
        [MenuItem("Tools/E Utility/X坐标+1 %#l")]
        public static void MoveX1()
        {
            Move(Selection.transforms,1,0,0);
        }
        [MenuItem("Tools/E Utility/X坐标-1 %#j")]
        public static void Move1X()
        {
            Move(Selection.transforms, -1, 0, 0);
        }
        [MenuItem("Tools/E Utility/Y坐标+1 %#i")]
        public static void MoveY1()
        {
            Move(Selection.transforms, 0, 1, 0);
        }
        [MenuItem("Tools/E Utility/Y坐标-1 %#k")]
        public static void Move1Y()
        {
            Move(Selection.transforms, 0, -1, 0);
        }


        /// <summary>
        /// 是否有键抬起
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool AnyKeyUp(KeyCode[] keys)
        {
            return keys.Any(k => Input.GetKeyUp(k));
        }
        /// <summary>
        /// 是否有键按下
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool AnyKeyDown(KeyCode[] keys)
        {
            return keys.Any(k => Input.GetKeyDown(k));
        }
        /// <summary>
        /// 是否有键按下按住
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool AnyKeyPressed(KeyCode[] keys)
        {
            return keys.Any(k => Input.GetKey(k));
        }

        // Distance between two ClosestPointOnBounds
        // this is needed in cases where entites are really big. in those cases,
        // we can't just move to entity.transform.position, because it will be
        // unreachable. instead we have to go the closest point on the boundary.
        //
        // Vector2.Distance(a.transform.position, b.transform.position):
        //    _____        _____
        //   |     |      |     |
        //   |  x==|======|==x  |
        //   |_____|      |_____|
        //
        //
        // Utils.ClosestDistance(a.collider, b.collider):
        //    _____        _____
        //   |     |      |     |
        //   |     |x====x|     |
        //   |_____|      |_____|
        //
        public static float ClosestDistance(Collider2D a, Collider2D b)
        {
            return Vector2.Distance(a.ClosestPointOnBounds(b.transform.position),
                                    b.ClosestPointOnBounds(a.transform.position));
        }

        // raycast while ignoring self (by setting layer to "Ignore Raycasts" first)
        // => setting layer to IgnoreRaycasts before casting is the easiest way to do it
        // => raycast + !=this check would still cause hit.point to be on player
        // => raycastall is not sorted and child objects might have different layers etc.
        public static RaycastHit2D Raycast2DWithout(Ray ray, GameObject ignore)
        {
            // remember layers
            Dictionary<Transform, int> backups = new Dictionary<Transform, int>();

            // set all to ignore raycast
            foreach (Transform tf in ignore.GetComponentsInChildren<Transform>(true))
            {
                backups[tf] = tf.gameObject.layer;
                tf.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }

            // raycast
            RaycastHit2D result = Physics2D.GetRayIntersection(ray);

            // restore layers
            foreach (KeyValuePair<Transform, int> kvp in backups)
                kvp.Key.gameObject.layer = kvp.Value;

            return result;
        }

        // direction for animation state machine ///////////////////////////////////
        // return Orthonormal Vector2 for a given Vector2, so that it's always like
        // (1,1) (0,1) (-1, -1) etc.
        public static Vector2 OrthonormalVector2(Vector2 vector, Vector2 defaultVector)
        {
            // zero?
            if (vector == Vector2.zero) return defaultVector;

            // normalize
            vector = vector.normalized;

            // quantize
            // -> right?
            if (vector.x > 0)
            {
                if (vector.y > 0.5f) return Vector2.up;
                if (vector.y < -0.5f) return Vector2.down;
                return Vector2.right;
                // -> left?
            }
            else
            {
                if (vector.y > 0.5f) return Vector2.up;
                if (vector.y < -0.5f) return Vector2.down;
                return Vector2.left;
            }
        }

        // pretty print seconds as hours:minutes:seconds(.milliseconds/100)s
        public static string PrettySeconds(float seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string res = "";
            if (t.Days > 0) res += t.Days + "d";
            if (t.Hours > 0) res += " " + t.Hours + "h";
            if (t.Minutes > 0) res += " " + t.Minutes + "m";
            // 0.5s, 1.5s etc. if any milliseconds. 1s, 2s etc. if any seconds
            if (t.Milliseconds > 0) res += " " + t.Seconds + "." + (t.Milliseconds / 100) + "s";
            else if (t.Seconds > 0) res += " " + t.Seconds + "s";
            // if the string is still empty because the value was '0', then at least
            // return the seconds instead of returning an empty string
            return res != "" ? res : "0s";
        }

        // hard mouse scrolling that is consistent between all platforms
        //   Input.GetAxis("Mouse ScrollWheel") and
        //   Input.GetAxisRaw("Mouse ScrollWheel")
        //   both return values like 0.01 on standalone and 0.5 on WebGL, which
        //   causes too fast zooming on WebGL etc.
        // normally GetAxisRaw should return -1,0,1, but it doesn't for scrolling
        public static float GetAxisRawScrollUniversal()
        {
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scroll < 0) return -1;
            if (scroll > 0) return 1;
            return 0;
        }

        // two finger pinch detection
        // source: https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
        public static float GetPinch()
        {
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                return touchDeltaMag - prevTouchDeltaMag;
            }
            return 0;
        }

        // parse last upper cased noun from a string, e.g.
        //   EquipmentWeaponBow => Bow
        //   EquipmentShield => Shield
        public static string ParseLastNoun(string text)
        {
            MatchCollection matches = new Regex(@"([A-Z][a-z]*)").Matches(text);
            return matches.Count > 0 ? matches[matches.Count - 1].Value : "";
        }

        // check if the cursor is over a UI or OnGUI element right now
        // note: for UI, this only works if the UI's CanvasGroup blocks Raycasts
        // note: for OnGUI: hotControl is only set while clicking, not while zooming
        public static bool IsCursorOverUserInterface()
        {
            // IsPointerOverGameObject check for left mouse (default)
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            // IsPointerOverGameObject check for touches
            for (int i = 0; i < Input.touchCount; ++i)
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    return true;

            // OnGUI check
            return GUIUtility.hotControl != 0;
        }

        // PBKDF2 hashing recommended by NIST:
        // http://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-132.pdf
        // salt should be at least 128 bits = 16 bytes
        public static string PBKDF2Hash(string text, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        // invoke multiple functions by prefix via reflection.
        // -> works for static classes too if object = null
        // -> cache it so it's fast enough for Update calls
        static Dictionary<KeyValuePair<Type, string>, MethodInfo[]> lookup = new Dictionary<KeyValuePair<Type, string>, MethodInfo[]>();
        public static MethodInfo[] GetMethodsByPrefix(Type type, string methodPrefix)
        {
            KeyValuePair<Type, string> key = new KeyValuePair<Type, string>(type, methodPrefix);
            if (!lookup.ContainsKey(key))
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                                           .Where(m => m.Name.StartsWith(methodPrefix))
                                           .ToArray();
                lookup[key] = methods;
            }
            return lookup[key];
        }

        public static void InvokeMany(Type type, object onObject, string methodPrefix, params object[] args)
        {
            foreach (MethodInfo method in GetMethodsByPrefix(type, methodPrefix))
                method.Invoke(onObject, args.ToArray());
        }
    }
}