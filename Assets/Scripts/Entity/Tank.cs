using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct TankParam
{
    public int maxHP;
    public int maxMana;

    public int tauntAttackDebuff;
    public int tauntDuration;
    public int tauntCost;

    public int singleDamage;
    public int singleHeal;
    public int singleCost;

    public int basicDamage;

    public TankParam(int maxHP, int maxMana, int tauntAttackDebuff, int tauntDuration, int tauntCost,
                     int singleDamage, int singleHeal, int singleCost, int basicDamage)
    {
        this.maxHP = maxHP;
        this.maxMana = maxMana;
        this.tauntAttackDebuff = tauntAttackDebuff;
        this.tauntDuration = tauntDuration;
        this.tauntCost = tauntCost;
        this.singleDamage = singleDamage;
        this.singleHeal = singleHeal;
        this.singleCost = singleCost;
        this.basicDamage = basicDamage;
    }
}
public class Tank : Entity
{
    TankParam tankParam;

    public Tank(TankParam param): base(param.maxHP, param.maxMana)
    {
        tankParam = param;
    }

    public void BasicAttack(Entity target)
    {
        target.Damage(tankParam.basicDamage);
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

    public void Taunt(Entity target)
    {
        if(!CheckCost(tankParam.tauntCost))
        {
            return;
        }
        mana -= tankParam.tauntCost;
        target.Taunted(this, tankParam.tauntDuration);
        target.DebuffAttack(tankParam.tauntAttackDebuff, tankParam.tauntDuration);
    }

    public void SingleAttackAndHeal(Entity target)
    {
        if (!CheckCost(tankParam.singleCost))
        {
            return;
        }
        mana -= tankParam.singleCost;
        target.Damage(tankParam.singleDamage + attackBuff - attackDebuff);
        this.Heal(tankParam.singleHeal);
    }
}
