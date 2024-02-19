using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public struct BossParam
{
    public int maxHP;
    public int singleDamage;
    public int aoeDamage;
    public float aoeProbability;

    public BossParam(int maxHP, int singleDamage, int aoeDamage, float aoeProbability)
    {
        this.maxHP = maxHP;
        this.singleDamage = singleDamage;
        this.aoeDamage = aoeDamage;
        this.aoeProbability = aoeProbability;
    }
}
public class Boss : Entity
{
    public readonly BossParam bossParam;

    public Boss(BossParam param): base(param.maxHP, 0)
    {
        bossParam = param;
    }

    public void SingleTarget(Entity target)
    {
        if (isTaunted)
        {
            target = tauntTarget;
        }
        target.Damage(bossParam.singleDamage + attackBuff - attackDebuff);
    }

    public void AoEAttack(Party party)
    {
        party.AOEDamage(bossParam.aoeDamage + attackBuff - attackDebuff);
    }
}
