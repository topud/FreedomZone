using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public class Building : Entity<BuildingStaticData, BuildingDynamicData>
    {
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        protected override void Reset()
        {
            base.Reset();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();
            if (!character) return;

            if (character.IsPlayer)
            {
                SpriteSorter.SetAlpha(0.5f);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();
            if (!character) return;

            if (character.IsPlayer)
            {
                SpriteSorter.SetAlpha(1);
            }
        }

        [ContextMenu("重置动态数据")]
        /// <summary>
        /// 重置动态数据
        /// </summary>
        public override void ResetDynamicData()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据未设置，动态数据无法设置");
                return;
            }

            DynamicData = new BuildingDynamicData
            {
                nameID = new NameAndID(StaticData.Name, IsAsset ? -1 : 0),
                position = IsAsset ? new Vector2(0, 0) : new Vector2(transform.position.x, transform.position.y),
                health = StaticData.Health,
                power = StaticData.Power,
                //DynamicData.items = StaticData.Items;              **

            };

            Refresh();
        }
        [ContextMenu("刷新数据")]
        /// <summary>
        /// 刷新数据
        /// </summary>
        public override void Refresh()
        {
            if (!StaticData)
            {
                Debug.LogError("静态数据未设置，动态数据无法设置");
                return;
            }
            if (DynamicData == null)
            {
                Debug.Log("动态数据初始化 " + StaticData.Name);
                ResetDynamicData();
            }

            gameObject.layer = LayerMask.NameToLayer("Building");
            gameObject.tag = "Building";

            name = IsAsset ? DynamicData.nameID.name : DynamicData.nameID.NameID;
            transform.position = DynamicData.position;
            Rigidbody.mass = StaticData.Weight;
            Rigidbody.bodyType = RigidbodyType2D.Static;

            Items.Clear();
        }
    }
}