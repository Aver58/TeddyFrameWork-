﻿#region Copyright © 2020 Aver. All rights reserved.
/*
=====================================================
 AverFrameWork v1.0
 Filename:    D2Action_IsHit.cs
 Author:      Zeng Zhiwei
 Time:        2020\6\21 星期日 13:18:01
=====================================================
*/
#endregion

using System.Collections.Generic;

public class D2Action_IsHit : D2Action
{
    private List<D2Action> m_SuccessActions;
    public D2Action_IsHit(List<D2Action> successActions, AbilityData abilityData) : base(abilityData)
    {
        m_SuccessActions = successActions;
    }

    protected override void ExecuteByUnit(BattleEntity source, BattleEntity target)
    {
        for(int i = 0; i < m_SuccessActions.Count; i++)
        {
            D2Action action = m_SuccessActions[i];
            action.Execute(source, m_RequestTarget);
        }
    }
}