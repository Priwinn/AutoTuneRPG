using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct DPSParam
{
    public int maxHP;
    public int maxMana;

    public int attackBuff;
    public int attackBuffDuration;
    public int attackBuffCost;

    public int singleDamage;
    public int singleDamageCost;

    public int basicDamage;

    public DPSParam(int maxHP, int maxMana, int attackBuff, int attackBuffDuration, int attackBuffCost, 
                    int singleDamage, int singleDamageCost, int basicDamage)
    {
        this.maxHP = maxHP;
        this.maxMana = maxMana;
        this.attackBuff = attackBuff;
        this.attackBuffDuration = attackBuffDuration;
        this.attackBuffCost = attackBuffCost;
        this.singleDamage = singleDamage;
        this.singleDamageCost = singleDamageCost;
        this.basicDamage = basicDamage;
    }
}

public class DPS : Entity
{
    public readonly DPSParam dpsParam;
    public DPS(DPSParam param) : base(param.maxHP, param.maxMana)
    {
        dpsParam = param;
    }

    public void BasicAttack(Entity target)
    {
        target.Damage(dpsParam.basicDamage);
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

    public void AOEAttackBuff(Party party)
    {
        if (!CheckCost(dpsParam.attackBuffCost))
        {
            return;
        }
        mana -= dpsParam.attackBuffCost;
        party.AOEAttackBuff(dpsParam.attackBuff, dpsParam.attackBuffDuration);
    }

    public void SingleDamage(Entity target)
    {
        if (!CheckCost(dpsParam.singleDamageCost))
        {
            return;
        }
        mana -= dpsParam.singleDamageCost;
        target.Damage(dpsParam.singleDamage + attackBuff - attackDebuff);
    }
}
