using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    protected Entity defaultTarget;
    protected Entity tauntTarget;

    protected int HP;
    protected int maxHP;

    protected int mana;
    protected int maxMana;

    protected int attackBuff = 0;
    protected int attackBuffDuration = 0;
    protected int attackDebuff = 0;
    protected int attackDebuffDuration = 0;

    protected int defenseBuff = 0;
    protected int defenseBuffDuration = 0;
    protected int defenseDebuff = 0;
    protected int defenseDebuffDuration = 0;

    protected bool isTaunted = false;
    protected int tauntDuration = 0;

    public Entity(int initHP, int initMana)
    {
        maxHP = initHP; HP = maxHP;
        maxMana = initMana; mana = maxMana;
    }

    public Entity GetDefaultTarget()
    {
        return defaultTarget;
    }

    public void SetDefaultTarget(Entity target)
    {
        defaultTarget = target;
    }

    public Entity GetTauntTarget()
    {
        return tauntTarget;
    }

    public void SetTauntTarget(Entity target)
    {
        tauntTarget = target;
    }

    public int GetHP()
    {
        return HP;
    }

    /*
    public float GetPercentHP()
    {
        return (float)HP / (float)maxHP;
    }
    */

    public int GetPercentHP()
    {
        return (int)(((float)HP / (float)maxHP) * 100);
    }
    
    public int GetMaxHP()
    {
        return maxHP;
    }

    public int GetMana()
    {
        return mana;
    }

    public int GetMaxMana()
    {
        return maxMana;
    }

    public int GetAttackBuff()
    {
        return attackBuff;
    }

    public int GetAttackBuffDuration()
    {
        return attackBuffDuration;
    }

    public int GetAttackDebuff()
    {
        return attackBuff;
    }

    public int GetAttackDebuffDuration()
    {
        return attackDebuffDuration;
    }

    public int GetDefenseBuff()
    {
        return defenseBuff;
    }

    public int GetDefenseBuffDuration()
    {
        return defenseBuffDuration;
    }

    public int GetDefenseDebuff()
    {
        return defenseDebuff;
    }

    public int GetDefenseDebuffDuration()
    {
        return defenseDebuffDuration;
    }

    public bool IsTaunted()
    {
        return isTaunted;
    }

    public int GetTauntDuration()
    {
        return tauntDuration;
    }

    virtual public int Damage(int damage)
    {
        damage = Mathf.Max(damage - defenseBuff + defenseDebuff, 0);
        HP -= damage;
        if (HP <= 0)
        {
            OnDeath();
        }
        return damage;
    }

    virtual public int Heal(int heal)
    {
        heal = Mathf.Max(heal, 0);
        HP += heal;
        HP = Mathf.Min(maxHP, HP);
        return heal;
    }

    virtual public void OnDeath()
    {

    }

    virtual public void BuffAttack(int buff, int duration)
    {
        attackBuff += buff;
        attackBuffDuration = duration;
    }

    virtual public void DebuffAttack(int debuff, int duration)
    {
        attackDebuff += debuff;
        attackDebuffDuration = duration;
    }

    virtual public void BuffDefense(int buff, int duration)
    {
        defenseBuff += buff;
        defenseBuffDuration = duration;
    }

    virtual public void DebuffDefense(int debuff, int duration)
    {
        defenseDebuff += debuff;
        defenseDebuffDuration = duration;
    }

    virtual public void Taunted(Entity target, int duration)
    {
        isTaunted = true;
        tauntTarget = target;
        tauntDuration = duration;
    }

    virtual public void ResolveTurn()
    {
        if (attackBuffDuration > 0)
        {
            attackBuffDuration--;
            if (attackBuffDuration == 0)
            {
                attackBuff = 0;
            }
        }
        if (attackDebuffDuration > 0)
        {
            attackDebuffDuration--;
            if (attackDebuffDuration == 0)
            {
                attackDebuff = 0;
            }
        }

        if (defenseBuffDuration > 0)
        {
            defenseBuffDuration--;
            if (defenseBuffDuration == 0)
            {
                defenseBuff = 0;
            }
        }
        if (defenseDebuffDuration > 0)
        {
            defenseDebuffDuration--;
            if (defenseDebuffDuration == 0)
            {
                defenseDebuff = 0;
            }
        }

        if (tauntDuration > 0)
        {
            tauntDuration--;
            if (tauntDuration == 0)
            {
                isTaunted = false;
            }
        }
    }
}
