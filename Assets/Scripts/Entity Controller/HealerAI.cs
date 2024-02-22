using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAI : EntityController
{
    private int AoEThresholdCount = 2;
    private float AoEThresholdPercentHP = 0.7f;
    private float SingleThresholdPercentHP = 0.5f;

    public HealerAI (Healer healer)
    {
        entity = healer;
    }

    public override void Execute()
    {
        List<Ability> abilityList = entity.GetAbilityList();
        Entity healTarget = null;
        int count = 0;

        foreach (Entity member in party.membersEntity)
        {
            if (member.GetHPPercentage() <= AoEThresholdPercentHP)
            {
                count++;
            }
            if (member.GetHPPercentage() <= SingleThresholdPercentHP)
            {
                healTarget = member;
            }
        }
        bool success = false;
        if (count >= AoEThresholdCount)
        {
            success = AbilityManager.ExecuteAction(abilityList[2], this); // AOE Heal
        }
        if (healTarget != null && !success)
        {
            success = AbilityManager.ExecuteAction(abilityList[1], this, healTarget);     // Single heal
        }
        if (!success)
        {
            AbilityManager.ExecuteAction(abilityList[0], this, entity.GetDefaultTarget());  // Basic Attack
        }
    }
}
