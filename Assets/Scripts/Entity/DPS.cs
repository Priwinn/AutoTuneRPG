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
    public DPS(DPSParam param, bool printMode= false) : base(param.maxHP, param.maxMana, printMode)
    {
        dpsParam = param;
        abilities.Add(InitBasicAttack());
        abilities.Add(InitAoEAttackBuff());
        abilities.Add(InitSingleDamage());
    }

    private Ability InitBasicAttack()
    {
        Ability basicAttack = new Ability();
        basicAttack.abilityName = "DPSBasicAttack";
        basicAttack.damage = dpsParam.basicDamage;
        basicAttack.hitCount = 1;
        return basicAttack;
    }

    private Ability InitSingleDamage()
    {
        Ability singleDamage = new Ability();
        singleDamage.abilityName = "DPSSIngleDamage";
        singleDamage.manaCost = dpsParam.singleDamageCost;
        singleDamage.damage = dpsParam.singleDamage;
        singleDamage.hitCount = 1;
        return singleDamage;
    }

    private Ability InitAoEAttackBuff()
    {
        Ability aoeAttackBuff = new Ability();
        aoeAttackBuff.abilityName = "DPSAoEAttackBuff";
        aoeAttackBuff.manaCost = dpsParam.attackBuffCost;
        aoeAttackBuff.attackBuff = dpsParam.attackBuff;
        aoeAttackBuff.buffAoE = true;
        aoeAttackBuff.duration = dpsParam.attackBuffDuration;
        return aoeAttackBuff;
    }

    /*
    public void BasicAttack(Entity target)
    {
        int damage = target.Damage(dpsParam.basicDamage + attackBuff - attackDebuff);
        if (printMode)
        {
            Debug.Log("DPS BasicAttack " + target + " for " + damage + " damage!");
        }
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
            if (printMode)
            {
                Debug.Log("DPS atmpted to AOEAttackBuff but YOU HAVE NO MANA.");
            }
            return;
        }
        mana -= dpsParam.attackBuffCost;
        party.AOEAttackBuff(dpsParam.attackBuff, dpsParam.attackBuffDuration);
        if (printMode)
        {
            Debug.Log("DPS AOEAttackBuff " + dpsParam.attackBuff + " attack for " + dpsParam.attackBuffDuration + " turns using " + dpsParam.attackBuffCost + " mana.");
        }
    }

    public void SingleDamage(Entity target)
    {
        if (!CheckCost(dpsParam.singleDamageCost))
        {
            if (printMode)
            {
                Debug.Log("DPS atmpted to Single Damage but YOU HAVE NO MANA.");
            }
            return;
        }
        mana -= dpsParam.singleDamageCost;
        int damage = target.Damage(dpsParam.singleDamage + attackBuff - attackDebuff);
        if (printMode)
        {
            Debug.Log("DPS Attack " + target + " for " + damage + " damage using " + dpsParam.singleDamageCost + " mana.");
        }
    }
    */
}
