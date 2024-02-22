using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSAI : EntityController
{
    public DPSAI(DPS dps)
    {
        entity = dps;
    }

    public override void Execute()
    {
        List<Ability> abilityList = entity.GetAbilityList();
        bool success = false;

        if (entity.GetAttackBuffDuration() <= 0)
        {
            success = AbilityManager.ExecuteAction(abilityList[1], this);   // AOE Attack Buff
        }
        if (!success)
        {
            success = AbilityManager.ExecuteAction(abilityList[2], this, entity.GetDefaultTarget());    // Single Damage
        }
        if (!success)
        {
            AbilityManager.ExecuteAction(abilityList[0], this, entity.GetDefaultTarget());              // Basic Attack
        }
    }
}
