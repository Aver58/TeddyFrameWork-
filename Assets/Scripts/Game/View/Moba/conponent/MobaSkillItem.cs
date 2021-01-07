﻿#region Copyright © 2020 Aver. All rights reserved.
/*
=====================================================
 AverFrameWork v1.0
 Filename:    MobaSkillItem.cs
 Author:      Zeng Zhiwei
 Time:        2021\1\5 星期二 22:47:27
=====================================================
*/
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobaSkillItem : ViewBase
{
    private int m_skillID;
    private Ability m_ability;
    private System.Action<AbilityCastType> m_action;
    private AbilityCastType m_abilityCastType;
    private Timer timer;
    private TextMeshProUGUI m_txtCD;
    private Image m_imgCDMask;

    public MobaSkillItem(GameObject go,Transform parent):base(go,parent)
    {
        AddListener((Button)UI["BtnSkill"], OnBtnSkill);
    }

    public void Init(AbilityCastType castType, Ability ability,System.Action<AbilityCastType> action)
    {
        m_action = action;
        m_ability = ability;
        m_skillID = ability.ID;
        m_abilityCastType = castType;

        skillItem skillItem = skillTable.Instance.GetTableItem(m_skillID);
        string iconName = skillItem.icon;
        iconName = "skill/" + iconName;//todo 导出动态图片映射表
        ImageEx ImgIcon = (ImageEx)UI["ImgIcon"];
        ImgIcon.SetSprite(iconName);

        m_txtCD = (TextMeshProUGUI)UI["TxtCD"];
        m_imgCDMask = (Image)UI["ImgCDMask"];

        SetCDState();
    }

    public void SetCDState()
    {
        bool isInCD = m_ability.CD > 0;
        m_imgCDMask.gameObject.SetActive(isInCD);
        timer = Timer.Register(m_ability.CD, OnCDDone, OnCDUpdate, false, true);
    }

    private void OnCDDone()
    {
        Timer.Cancel(timer);
        m_imgCDMask.gameObject.SetActive(false);
    }

    private void OnCDUpdate(float realTime)
    {
        m_imgCDMask.fillAmount = m_ability.CD / m_ability.GetTotalCD();
        m_txtCD.text = m_ability.CD.ToString("f1");
    }

    private void OnBtnSkill()
    {
        if(m_action!=null)
            m_action.Invoke(m_abilityCastType);
    }
}