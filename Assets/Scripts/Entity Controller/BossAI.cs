using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EntityController
{
    public BossAI(Boss boss)
    {
        entity = boss;
    }

    // Get lowest HP target
    private Entity SelectTarget()
    {
        Entity target = null;
        foreach (Entity member in targetParty.membersEntity)
        {
            if (target == null)
            {
                target = member;
            }
            else if (member.GetHP() < target.GetHP())
            {
                target = member;
            }
        }
        return target;
    }

    public override void Execute()
    {
        List<Ability> abilityList = entity.GetAbilityList();
        if (Random.Range(0, 100) <= ((Boss)entity).bossParam.aoeProbability)
        {
            AbilityManager.ExecuteAction(abilityList[1], this); // AOE
        }
        else
        {
            AbilityManager.ExecuteAction(abilityList[0], this, SelectTarget()); // Single
        }
    }
}
