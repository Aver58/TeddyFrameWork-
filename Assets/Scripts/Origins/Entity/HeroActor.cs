using System;
using UnityEngine;

namespace Origins.Entity {
    public class HeroActor : Actor {
        public Entity Entity;

        private Vector2 inputPos;
        private Transform mTransform;
        private Rigidbody2D rigidbody2D;

        public void Init(HeroEntity heroEntity, Rigidbody2D rigidbody) {
            inputPos = new Vector2();
            rigidbody2D = rigidbody;
            mTransform = rigidbody.transform;
            
            Entity = heroEntity;
        }

        //获取物理操作
        public override void OnUpdate() {
            OnInput();
        }

        private void OnInput() {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            inputPos.x = horizontal;
            inputPos.y = vertical;
            // rigidbody2D.velocity = inputPos;
            // rigidbody2D.MovePosition(targetPos);
      
            if (Math.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f) {
                var targetPos = rigidbody2D.position + inputPos * Entity.MoveSpeed;
                mTransform.position = targetPos;//todo 先用这种方式做，rigidbody2D 跑不起来
            }
        }
    }
}