using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityManager
{
    public static bool printMode = false;

    public static bool ExecuteAction(Ability ability, EntityController caster, Entity target = null)
    {
        Entity casterEntity = caster.GetEntity();
        if (printMode)
        {
            Debug.Log(casterEntity + " cast " + ability.abilityName);
        }
        // Handle cost
        if (!casterEntity.ResolveCost(ability.manaCost, ability.hpCost))
        {
            if (printMode)
            {
                Debug.Log("BUT " + casterEntity + " HAVE NO MANA!!");
            }
            return false;
        }

        if (casterEntity.IsTaunted())
        {
            Entity tauntTarget = casterEntity.GetTauntTarget();
            if (tauntTarget != null && tauntTarget.GetHP() > 0)
            {
                target = tauntTarget;
            }
        }

        // Handle damage
        HandleDamage(ability, caster, target);

        // Handle Heal
        HandleHeal(ability, caster, target);

        //Handle Mana recover
        HandleRecoverMana(ability, caster, target);

        if (ability.duration <= 0)
        {
            return true;
        }
        // Handle Attack buff
        HandleAttackBuff(ability, caster, target);

        // Handle Defense buff
        HandleDefenseBuff(ability, caster, target);

        // Handle Attack debuff
        HandleAttackDebuff(ability, caster, target);

        // Handle Defense debuff
        HandleDefenseDebuff(ability, caster, target);

        // Handle taunt
        HandleTaunt(ability, caster, target);

        // Handle stun
        HandleStun(ability, caster, target);

        return true;
    }

    private static void HandleDamage(Ability ability, EntityController caster, Entity target)
    {
        Entity casterEntity = caster.GetEntity();
        int hitCount = ability.hitCount;
        if (hitCount > 0)
        {
            string targetName;
            int damage = ability.damage + casterEntity.GetAttackBuff() - casterEntity.GetAttackDebuff();
                        //+ Bool2Int(target.IsStunned()) * ability.stunExtraDamage + Bool2Int(target.IsTaunted()) * ability.tauntExtraDamage;
            damage = Mathf.Max(damage, 0);
            if (ability.damageAll)
            {
                targetName = "ALL";
                if (printMode)
                {
                    Debug.Log(casterEntity + " deals " + damage + "x" + hitCount + " to " + targetName + "!!");
                }
                Damage(damage, hitCount, caster.GetParty());
                Damage(damage, hitCount, caster.GetTargetParty());
            }
            else if (ability.damageAoE)
            {
                targetName = caster.GetTargetParty().partyName;
                if (printMode)
                {
                    Debug.Log(casterEntity + " deals " + damage + "x" + hitCount + " to " + targetName + "!!");
                }
                Damage(damage, hitCount, caster.GetTargetParty());
            }
            // Single target
            else
            {
                CheckTargetNull(target);
                targetName = target.ToString();
                if (printMode)
                {
                    Debug.Log(casterEntity + " deals " + damage + "x" + hitCount + " to " + targetName + "!!");
                }
                Damage(damage, hitCount, target);
            }    
        }
    }

    private static void HandleHeal(Ability action, EntityController caster, Entity target)
    {
        Entity casterEntity = caster.GetEntity();
        if (action.heal > 0)
        {
            string targetName;
            int heal = action.heal;
            if (action.healAoE)
            {
                targetName = caster.GetParty().partyName;
                if (printMode)
                {
                    Debug.Log(casterEntity + " heals " + targetName + " for " + heal + " heal");
                }
                caster.GetParty().AOEHeal(heal);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    targetName = target.ToString();
                    if (printMode)
                    {
                        Debug.Log(casterEntity + " heals " + targetName + " for " + heal + " heal");
                    }
                    target.Heal(heal);
                }
                else
                {
                    targetName = "Self";
                    if (printMode)
                    {
                        Debug.Log(casterEntity + " heals " + targetName + " for " + heal + " heal");
                    }
                    casterEntity.Heal(heal);
                }
            }
        }
    }

    private static void HandleRecoverMana(Ability action, EntityController caster, Entity target)
    {
        Entity casterEntity = caster.GetEntity();
        if (action.manaRecover > 0)
        {
            string targetName;
            if (action.manaRecoverAoE)
            {
                targetName = caster.GetParty().partyName;
                caster.GetParty().AoERecoverMana(action.manaRecover);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    targetName = target.ToString();
                    target.RecoverMana(action.manaRecover);
                }
                else
                {
                    targetName = "Self";
                    casterEntity.RecoverMana(action.manaRecover);
                }
            }
            if (printMode)
            {
                Debug.Log(casterEntity + " recover " + action.manaRecover + " mana to " + targetName);
            }
        }
    }

    private static void HandleAttackBuff(Ability action, EntityController caster, Entity target)
    {
        Entity casterEntity = caster.GetEntity();
        if (action.attackBuff > 0)
        {
            string targetName;
            if (action.buffAll)
            {
                targetName = "ALL";
                caster.GetParty().AOEAttackBuff(action.attackBuff, action.duration);
                caster.GetTargetParty().AOEAttackBuff(action.attackBuff, action.duration);
            }
            else if (action.buffAoE)
            {
                targetName = caster.GetParty().partyName;
                caster.GetParty().AOEAttackBuff(action.attackBuff, action.duration);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    targetName = target.ToString();
                    target.BuffAttack(action.attackBuff, action.duration);
                }
                else
                {
                    targetName = "Self";
                    casterEntity.BuffAttack(action.attackBuff, action.duration);
                }
            }
            if (printMode)
            {
                Debug.Log(casterEntity + " buff " + action.attackBuff + " Attack to " + targetName + " for " + action.duration + " turns.");
            }
        }
    }

    private static void HandleDefenseBuff(Ability action, EntityController caster, Entity target)
    {
        Entity casterEntity = caster.GetEntity();
        if (action.defenseBuff > 0)
        {
            string targetName;
            if (action.buffAll)
            {
                targetName = "ALL";
                caster.GetParty().AOEDefenseBuff(action.defenseBuff, action.duration);
                caster.GetTargetParty().AOEDefenseBuff(action.defenseBuff, action.duration);
            }
            else if (action.buffAoE)
            {
                targetName = caster.GetParty().partyName;
                caster.GetParty().AOEDefenseBuff(action.defenseBuff, action.duration);
            }
            else
            {
                if (AllyTargetCheck(caster, target))
                {
                    targetName = target.ToString();
                    target.BuffDefense(action.defenseBuff, action.duration);
                }
                else
                {
                    targetName = "Self";
                    casterEntity.BuffDefense(action.defenseBuff, action.duration);
                }
            }
            if (printMode)
            {
                Debug.Log(casterEntity + " buff " + action.defenseBuff + " Defense to " + targetName + " for " + action.duration + " turns.");
            }
        }
    }

    private static void HandleAttackDebuff(Ability action, EntityController caster, Entity target)
    {
        if (action.attackDebuff > 0)
        {
            string targetName;
            if (action.debuffAll)
            {
                targetName = "ALL";
                caster.GetParty().AOEAttackDebuff(action.attackDebuff, action.duration);
                caster.GetTargetParty().AOEAttackDebuff(action.attackDebuff, action.duration);
            }
            else if (action.debuffAoE)
            {
                targetName = caster.GetTargetParty().partyName;
                caster.GetTargetParty().AOEAttackDebuff(action.attackDebuff, action.duration);
            }
            else
            {
                CheckTargetNull(target);
                targetName = target.ToString();
                target.DebuffAttack(action.attackDebuff, action.duration);
            }
            if (printMode)
            {
                Debug.Log(caster.GetEntity() + " debuff " + action.attackDebuff + " Attack to " + targetName + " for " + action.duration + " turns.");
            }
        }
    }

    private static void HandleDefenseDebuff(Ability action, EntityController caster, Entity target)
    {
        if (action.defenseDebuff > 0)
        {
            string targetName;
            if (action.debuffAll)
            {
                targetName = "ALL";
                caster.GetParty().AOEDefenseDebuff(action.defenseDebuff, action.duration);
                caster.GetTargetParty().AOEDefenseDebuff(action.defenseDebuff, action.duration);
            }
            else if (action.debuffAoE)
            {
                targetName = caster.GetTargetParty().partyName;
                caster.GetTargetParty().AOEDefenseDebuff(action.defenseDebuff, action.duration);
            }
            else
            {
                CheckTargetNull(target);
                targetName = target.ToString();
                target.DebuffDefense(action.defenseDebuff, action.duration);
            }
            if (printMode)
            {
                Debug.Log(caster.GetEntity() + " debuff " + action.defenseDebuff + " Defense to " + targetName + " for " + action.duration + " turns.");
            }
        }
    }

    private static void HandleTaunt(Ability action, EntityController caster, Entity target)
    {
        Entity casterEntity = caster.GetEntity();
        if (action.taunt == true)
        {
            string targetName;
            if (action.tauntAoE)
            {
                targetName= caster.GetTargetParty().partyName;
                caster.GetTargetParty().AOETaunted(casterEntity, action.duration);
            }
            else
            {
                CheckTargetNull(target);
                targetName = target.ToString();
                target.Taunted(casterEntity, action.duration);
            }
            if (printMode)
            {
                Debug.Log(casterEntity + " taunt " + targetName + " for " + action.duration + " turns.");
            }
            
        }
    }

    private static void HandleStun(Ability action, EntityController caster, Entity target)
    {
        if (action.stun == true)
        {
            string targetName;
            if (action.stunAoE)
            {
                targetName = caster.GetTargetParty().partyName;
                caster.GetTargetParty().AOEStunned(action.duration);
            }
            else
            {
                CheckTargetNull(target);
                targetName = target.ToString();
                target.Stunned(action.duration);
            }
            if (printMode)
            {
                Debug.Log(caster.GetEntity() + " stun " + targetName + " for " + action.duration + " turns.");
            }
        }
    }

    private static void Damage(int damage, int hitCount, Entity target)
    {
        for (int i = 0; i < hitCount; i++)
        {
            target.Damage(damage);
        }
    }

    private static void Damage(int damage, int hitCount, Party target)
    {
        for (int i = 0; i < hitCount; i++)
        {
            target.AOEDamage(damage);
        }
    }

    private static void CheckTargetNull(Entity target)
    {
        if (target == null)
        {
            throw new System.Exception("TARGETED ABILITY BUT NO TARGET GIVEN REEEEEEEEEEE");
        }
    }

    private static bool AllyTargetCheck(EntityController caster, Entity target)
    {
        return target != null && (caster.GetParty().membersEntity.Contains(target));
    }

    private static bool EnemyTargetCheck(EntityController caster, Entity target)
    {
        return target != null && (caster.GetTargetParty().membersEntity.Contains(target));
    }

    private static int Bool2Int(bool b)
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
