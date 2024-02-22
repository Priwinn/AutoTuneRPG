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

    public Tank(TankParam param, bool printMode = false) : base(param.maxHP, param.maxMana, printMode)
    {
        tankParam = param;
        abilities.Add(InitBasicAttack());
        abilities.Add(InitTaunt());
        abilities.Add(InitAttackAndHeal());
    }

    private Ability InitBasicAttack()
    {
        Ability basicAttack = new Ability();
        basicAttack.abilityName = "TankBasicAttack";
        basicAttack.damage = tankParam.basicDamage;
        basicAttack.hitCount = 1;
        return basicAttack;
    }

    private Ability InitTaunt()
    {
        Ability taunt = new Ability();
        taunt.abilityName = "TankTaunt";
        taunt.manaCost = tankParam.tauntCost;
        taunt.taunt = true;
        taunt.attackDebuff = tankParam.tauntAttackDebuff;
        taunt.duration = tankParam.tauntDuration;
        return taunt;
    }

    private Ability InitAttackAndHeal()
    {
        Ability attackAndHeal = new Ability();
        attackAndHeal.abilityName = "TankAttackAndHeal";
        attackAndHeal.manaCost = tankParam.singleCost;
        attackAndHeal.damage = tankParam.singleDamage;
        attackAndHeal.hitCount = 1;
        attackAndHeal.heal = tankParam.singleHeal;
        return attackAndHeal;
    }

    /*
    public void BasicAttack(Entity target)
    {
        int damage = target.Damage(tankParam.basicDamage + attackBuff - attackBuff);
        if (printMode)
        {
            Debug.Log("Tank BasicAttack " + target + " for " + damage + " damage!");
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

    public void Taunt(Entity target)
    {
        if(!CheckCost(tankParam.tauntCost))
        {
            if (printMode)
            {
                Debug.Log("Tank atmpted to Taunt" + target + " but YOU HAVE NO MANA.");
            }
            return;
        }
        mana -= tankParam.tauntCost;
        target.Taunted(this, tankParam.tauntDuration);
        target.DebuffAttack(tankParam.tauntAttackDebuff, tankParam.tauntDuration);
        if (printMode)
        {
            Debug.Log("Tank Taunted and debuff " + tankParam.tauntAttackDebuff + " attack " + target + " for " + tankParam.tauntDuration + " turns using " + tankParam.tauntCost + " mana.");
        }
    }

    public void SingleAttackAndHeal(Entity target)
    {
        if (!CheckCost(tankParam.singleCost))
        {
            if (printMode)
            {
                Debug.Log("Tank atmpted to Attack " + target + " and Heal but YOU HAVE NO MANA.");
            }
            return;
        }
        mana -= tankParam.singleCost;
        int damage = target.Damage(tankParam.singleDamage + attackBuff - attackDebuff);
        int heal = this.Heal(tankParam.singleHeal);

        if (printMode)
        {
            Debug.Log("Tank Attack " + target + " for " + damage + " damage and heal for " + heal + " HP using " + tankParam.tauntCost + " mana.");
        }
    }
    */
}
