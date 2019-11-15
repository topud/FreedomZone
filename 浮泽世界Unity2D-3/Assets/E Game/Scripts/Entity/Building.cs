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


        /// <summary>
        /// 设置数据，默认用于从存档读取数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetDynamicData(BuildingDynamicData data)
        {
            base.SetDynamicData(data);
        }

        [ContextMenu("刷新数据")]
        /// <summary>
        /// 刷新数据
        /// </summary>
        public override void Refresh()
        {
            ResetStaticData();
            ResetDynamicData();
        }
        [ContextMenu("初始化数据")]
        /// <summary>
        /// 初始化数据
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }
        /// <summary>
        /// 重置静态数据
        /// </summary>
        public override void ResetStaticData()
        {
            base.ResetStaticData();

            SpriteSorter.SetSprite(StaticData.Icon);
        }
        /// <summary>
        /// 重置动态数据
        /// </summary>
        public override void ResetDynamicData(bool isAddID = true)
        {
            base.ResetDynamicData(isAddID);

            gameObject.layer = LayerMask.NameToLayer("Building");
            gameObject.tag = "Building";

            if (!StaticData) return;

            DynamicData = new BuildingDynamicData
            {
                Name = StaticData.Name,
                Health = StaticData.Health,
            };
            Rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}