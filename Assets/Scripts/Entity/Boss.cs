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
    public int aoeProbability;

    public BossParam(int maxHP, int singleDamage, int aoeDamage, int aoeProbability)
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

    public Boss(BossParam param, bool printMode=false): base(param.maxHP, 0, printMode)
    {
        bossParam = param;
        abilities.Add(InitSingleTarget());
        abilities.Add(InitAoEAttack());
    }

    /*
    public void SingleTarget(Entity target)
    {
        if (isTaunted)
        {
            if (target.GetHP() <= 0)
            {
                isTaunted = false;
                tauntDuration = 0;
            }
            else
            {
                target = tauntTarget;
            }
        }
        int damage = target.Damage(bossParam.singleDamage + attackBuff - attackDebuff);
        if (printMode)
        {
            Debug.Log("Boss Attack " + target + " for " + damage + " damage!");
        }

    }
    */

    private Ability InitSingleTarget()
    {
        Ability singleTarget = new Ability();
        singleTarget.abilityName = "BossSingleTarget";
        singleTarget.damage = bossParam.singleDamage;
        singleTarget.hitCount = 1;
        return singleTarget;
    }

    /*
    public void AoEAttack(Party party)
    {
        int damage = bossParam.aoeDamage + attackBuff - attackDebuff;
        party.AOEDamage(damage);
        if (printMode)
        {
            Debug.Log("Boss AoE Attack for " + damage + " damage!");
        }
    }
    */

    private Ability InitAoEAttack()
    {
        Ability aoeAttack = new Ability();
        aoeAttack.abilityName = "BossAoEAttack";
        aoeAttack.damage = bossParam.aoeDamage;
        aoeAttack.hitCount = 1;
        aoeAttack.damageAoE = true;
        return aoeAttack;
    }
}
