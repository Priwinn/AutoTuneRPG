using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct HealerParam
{
    public int maxHP;
    public int maxMana;

    public int singleHeal;
    public int singleHealCost;

    public int aoeHeal;
    public int aoeHealCost;

    public int basicDamage;

    public HealerParam(int maxHP, int maxMana, int singleHeal, int singleHealCost, int aoeHeal, int aoeHealCost, int basicDamage)
    {
        this.maxHP = maxHP;
        this.maxMana = maxMana;
        this.singleHeal = singleHeal;
        this.singleHealCost = singleHealCost;
        this.aoeHeal = aoeHeal;
        this.aoeHealCost = aoeHealCost;
        this.basicDamage = basicDamage;
    }
}

public class Healer : Entity
{
    public readonly HealerParam healerParam;

    public Healer(HealerParam param) : base(param.maxHP, param.maxMana)
    {
        healerParam = param;
    }

    public void BasicAttack(Entity target)
    {
        target.Damage(healerParam.basicDamage + attackBuff - attackDebuff);
    }

    private bool CheckCost(int cost)
    {
        if (mana < cost)
        {
            if (defaultTarget != null)
            {
                BasicAttack(defaultTarget);
            }
            return false;
        }
        return true;
    }

    public void SingleHeal(Entity target)
    {
        if (!CheckCost(healerParam.singleHealCost))
        {
            return;
        }
        mana -= healerParam.singleHealCost;
        target.Heal(healerParam.singleHeal);
    }

    public void AOEHeal(Party party)
    {
        if (!CheckCost(healerParam.aoeHealCost))
        {
            return;
        }
        mana -= healerParam.aoeHealCost;
        party.AOEHeal(healerParam.aoeHeal);
    }
}
