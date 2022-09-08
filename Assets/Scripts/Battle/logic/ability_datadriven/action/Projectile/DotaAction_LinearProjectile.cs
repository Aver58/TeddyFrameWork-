using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Battle.logic.ability_dataDriven {
    // 目标,效果名称,移动速度,开始范围,结束范围,固定距离,开始位置,目标队伍,目标类型,目标标签,前方锥形,提供视野,视觉范围
    // Target, EffectName, MoveSpeed, StartRadius, EndRadius, FixedDistance, StartPosition, TargetTeams, TargetTypes, TargetFlags, HasFrontalCone, ProvidesVision, VisionRadius
    public sealed class DotaAction_LinearProjectile : DotaAction {
        // Require 必须参数
        [NotNull] private readonly string effectName;
        [NotNull] private readonly string sourceAttachment;
        private readonly bool dodgeable;
        private readonly float moveSpeed;

        // todo 抽象弹道Action类
        public delegate void OnProjectileHitUnit();
        public delegate void OnProjectileFinish();
        public delegate void OnProjectileDodge();

        // Optional 可选参数
        private bool providesVision;//提供视野
        private float visionRadius;//视野范围
        private bool hasFrontalCone;//前方锥形
        private float startRadius;
        private float endRadius;

        private Vector2 startPosition;
        private float fixedDistance;//固定距离

        public DotaAction_LinearProjectile(AbilityTarget actionTarget, [NotNull] string effectName,
            [NotNull] string sourceAttachment, bool dodgeable, float moveSpeed) : base(actionTarget) {
            this.effectName = effectName ?? throw new ArgumentNullException(nameof(effectName));
            this.sourceAttachment = sourceAttachment ?? throw new ArgumentNullException(nameof(sourceAttachment));
            this.dodgeable = dodgeable;
            this.moveSpeed = moveSpeed;
        }

        public void SetVisionParam(bool providesVision, float visionRadius) {
            this.providesVision = providesVision;
            this.visionRadius = visionRadius;
        }

        protected override void ExecuteBySingle() {
            // 创建子弹，子弹生命周期管理
            var linearProjectile = new LinearProjectile();
            
        }
    }
}