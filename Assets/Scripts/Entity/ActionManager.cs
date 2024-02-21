using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    private Party playerParty;
    private Party bossParty;
    public bool printMode = false;

    public bool ExecuteAction(Action action, EntityController caster, Entity target = null)
    {
        Entity casterEntity = caster.GetEntity();
        // Handle cost
        if (!casterEntity.ResolveCost(action.hpCost, action.manaCost))
        {
            return false;
        }

        if (casterEntity.IsTaunted())
        {
            target = casterEntity.GetTauntTarget();
        }

        // Handle damage
        if (action.hitCount > 0)
        {
            int damage = action.damage + casterEntity.GetAttackBuff() - casterEntity.GetAttackDebuff() 
                        + Bool2Int(target.IsStunned())*action.stunExtraDamage + Bool2Int(target.IsTaunted()) * action.tauntExtraDamage;
            if (action.damageAll)
            {
                Damage(damage, action.hitCount, caster.GetParty());
                Damage(damage, action.hitCount, caster.GetTargetParty());
            }
            else if (action.damageAoE)
            {
                Damage(damage, action.hitCount, caster.GetTargetParty());
            }
            // Single target
            else
            {
                CheckTargetNull(target);
                Damage(damage, action.hitCount, target);
            }
        }

        // Handle Heal
        if (action.heal > 0)
        {
            if (action.healAoE)
            {
                caster.GetParty().AOEHeal(action.heal);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    target.Heal(action.heal);
                }
                else
                {
                    casterEntity.Heal(action.heal);
                }
            }
        }

        //Handle Mana recover
        if (action.manaRecover > 0)
        {
            if (action.manaRecoverAoE)
            {
                caster.GetParty().AoERecoverMana(action.manaRecover);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    target.RecoverMana(action.manaRecover);
                }
                else
                {
                    casterEntity.RecoverMana(action.manaRecover);
                }
            }
        }
        
        if (action.duration <= 0)
        {
            return true;
        }
        // Handle Attack buff
        if (action.attackBuff > 0)
        {
            if (action.attackBuffAll)
            {
                caster.GetParty().AOEAttackBuff(action.attackBuff, action.duration);
                caster.GetTargetParty().AOEAttackBuff(action.attackBuff, action.duration);
            }
            else if (action.attackBuffAoE)
            {
                caster.GetParty().AOEAttackBuff(action.attackBuff, action.duration);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    target.BuffAttack(action.attackBuff, action.duration);
                }
                else
                {
                    casterEntity.BuffAttack(action.attackBuff, action.duration);
                }
            }
        }
        // Handle Defense buff
        if (action.defenseBuff > 0)
        {
            if (action.defenseBuffAll)
            {
                caster.GetParty().AOEDefenseBuff(action.defenseBuff, action.duration);
                caster.GetTargetParty().AOEDefenseBuff(action.defenseBuff, action.duration);
            }
            else if (action.defenseBuffAoE)
            {
                caster.GetParty().AOEDefenseBuff(action.defenseBuff, action.duration);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    target.BuffDefense(action.defenseBuff, action.duration);
                }
                else
                {
                    casterEntity.BuffDefense(action.defenseBuff, action.duration);
                }
            }
        }
        
        // Handle Attack debuff
        if (action.attackDebuff > 0)
        {
            if (action.attackDebuffAll)
            {
                caster.GetParty().AOEAttackDebuff(action.attackDebuff, action.duration);
                caster.GetTargetParty().AOEAttackDebuff(action.attackDebuff, action.duration);
            }
            else if (action.attackDebuffAoE)
            {
                caster.GetParty().AOEAttackDebuff(action.attackDebuff, action.duration);
            }
            else
            {
                CheckTargetNull(target);
                target.DebuffAttack(action.attackDebuff, action.duration);
            }
        }
        // Handle Defense debuff
        if (action.defenseDebuff > 0)
        {
            if (action.defenseDebuffAll)
            {
                caster.GetParty().AOEDefenseDebuff(action.defenseDebuff, action.duration);
                caster.GetTargetParty().AOEDefenseDebuff(action.defenseDebuff, action.duration);
            }
            else if (action.defenseDebuffAoE)
            {
                caster.GetParty().AOEDefenseDebuff(action.defenseDebuff, action.duration);
            }
            else
            {
                CheckTargetNull(target);
                target.DebuffDefense(action.defenseDebuff, action.duration);
            }
        }

        // Handle taunt
        if (action.taunt == true)
        {
            if (action.tauntAoE)
            {
                caster.GetTargetParty().AOETaunted(casterEntity, action.duration);
            }
            else
            {
                target.Taunted(casterEntity, action.duration);
            }
        }

        // Handle stun
        if (action.stun == true)
        {
            if (action.stunAoE)
            {
                caster.GetTargetParty().AOEStunned(action.duration);
            }
            else
            {
                target.Stunned(action.duration);
            }
        }

        return true;
    }

    private void Damage(int damage, int hitCount, Entity target)
    {
        for (int i = 0; i < hitCount; i++)
        {
            target.Damage(damage);
        }
    }

    private void Damage(int damage, int hitCount, Party target)
    {
        for (int i = 0; i < hitCount; i++)
        {
            target.AOEDamage(damage);
        }
    }

    private void CheckTargetNull(Entity target)
    {
        if (target == null)
        {
            throw new System.Exception("TARGETED ABILITY BUT NO TARGET GIVEN REEEEEEEEEEE");
        }
    }

    private bool AllyTargetCheck(EntityController caster, Entity target)
    {
        return target != null && (caster.GetParty().membersEntity.Contains(target));
    }

    private bool EnemyTargetCheck(EntityController caster, Entity target)
    {
        return target != null && (caster.GetTargetParty().membersEntity.Contains(target));
    }

    private int Bool2Int(bool b)
    {
        if (b)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
