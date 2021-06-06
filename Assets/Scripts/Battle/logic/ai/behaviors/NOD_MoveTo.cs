﻿#region Copyright © 2018 Aver. All rights reserved.
/*
=====================================================
 AverFrameWork v1.0
 Filename:    Nod_Move.cs
 Author:      Zeng Zhiwei
 Time:        2020/3/4 20:01:27
=====================================================
*/
#endregion

using UnityEngine;

namespace Aver3
{
    public class NOD_MoveTo : BTAction
    {
        private const float m_threshold = 0.01f;

        protected override void OnEnter(BTData wData)
        {
            GameLog.Log("NOD_MoveTo OnEnter");
            var behaviorData = wData as BattleData;
            BattleUnit owner = behaviorData.owner;
            owner.SetState(HeroState.MOVE);
        }

        protected override void OnExit(BTData bTData)
        {
            GameLog.Log("NOD_MoveTo OnExit");
            var behaviorData = bTData as BattleData;
            behaviorData.owner.SetRequestComplete();
        }
        
        protected override BTResult OnExecute(BTData wData)
        {
            var behaviorData = wData as BattleData;
            var owner = behaviorData.owner;
            var request = behaviorData.request;
            var target = request.target;

            var ownerPos = owner.Get3DPosition();
            var targetPos = target.Get3DPosition();

            float distance = BattleMath.GetDistance3DSquare(ownerPos, targetPos);

            if(distance <= m_threshold)
            {
                owner.Set3DPosition(targetPos);
                GameMsg.instance.SendMessage(GameMsgDef.Hero_MoveTo, owner.id, targetPos.x, targetPos.y, targetPos.z);
                return BTResult.Finished;
            }

            float moveSpeed = owner.GetMoveSpeed();
            float detalTime = behaviorData.deltaTime;
            var targetForward = (targetPos - ownerPos).normalized;
            float newPosX = ownerPos.x + detalTime * moveSpeed * targetForward.x;
            float newPosZ = ownerPos.z + detalTime * moveSpeed * targetForward.z;

            //distance = BattleMath.GetDistance3DSquare(ownerPos.x, ownerPos.y, ownerPos.z, newPosX,targetPos.y,newPosZ);
            GameLog.Log("【NOD_MoveTo】speed：{0} 当前位置：{1},{2} 目标位置：{3},{4} newPos：{5},{6}",moveSpeed, ownerPos.x, ownerPos.z, targetPos.x, targetPos.z, newPosX, newPosZ);
            
            owner.Set3DPosition(newPosX, targetPos.y, newPosZ);
            GameMsg.instance.SendMessage(GameMsgDef.Hero_MoveTo, owner.id, newPosX, targetPos.y, newPosZ);

            return BTResult.Running;
        }
    }
}