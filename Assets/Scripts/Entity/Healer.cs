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

    public Healer(HealerParam param, bool printMode=false) : base(param.maxHP, param.maxMana, printMode)
    {
        healerParam = param;
        abilities.Add(InitBasicAttack());
        abilities.Add(InitSingleHeal());
        abilities.Add(InitAoEHeal());
    }

    private Ability InitBasicAttack()
    {
        Ability basicAttack = new Ability();
        basicAttack.abilityName = "HealerBasicAttack";
        basicAttack.damage = healerParam.basicDamage;
        basicAttack.hitCount = 1;
        basicAttack.manaRecover = 10;
        return basicAttack;
    }

    private Ability InitSingleHeal()
    {
        Ability singleHeal = new Ability();
        singleHeal.abilityName = "HealerSingleHeal";
        singleHeal.manaCost = healerParam.singleHealCost;
        singleHeal.heal = healerParam.singleHeal;
        return singleHeal;
    }

    private Ability InitAoEHeal()
    {
        Ability aoeHeal = new Ability();
        aoeHeal.abilityName = "HealerAoEHeal";
        aoeHeal.manaCost = healerParam.aoeHealCost;
        aoeHeal.heal = healerParam.aoeHeal;
        aoeHeal.healAoE = true;
        return aoeHeal;
    }

    /*
    public void BasicAttack(Entity target)
    {
        int damage = target.Damage(healerParam.basicDamage + attackBuff - attackDebuff);
        if (printMode)
        {
            Debug.Log("Healer BasicAttack " + target + " for " + damage + " damage!");
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

    public void SingleHeal(Entity target)
    {
        if (!CheckCost(healerParam.singleHealCost))
        {
            if (printMode)
            {
                Debug.Log("Healer atmpted to heal " + target + " but YOU HAVE NO MANA.");
            }
            return;
        }
        mana -= healerParam.singleHealCost;
        int heal = target.Heal(healerParam.singleHeal);
        if (printMode)
        {
            Debug.Log("Healer heal " + target + " for " + heal + " HP using " + healerParam.singleHealCost + " mana.");
        }
    }

    public void AOEHeal(Party party)
    {
        if (!CheckCost(healerParam.aoeHealCost))
        {
            if (printMode)
            {
                Debug.Log("Healer atmpted to AoE Heal but YOU HAVE NO MANA.");
            }
            return;
        }
        mana -= healerParam.aoeHealCost;
        party.AOEHeal(healerParam.aoeHeal);
        if (printMode)
        {
            Debug.Log("Healer AoE heal party " + healerParam.aoeHeal + " HP using " + healerParam.aoeHealCost + " mana.");
        }
    }
    */


}
