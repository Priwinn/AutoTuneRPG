using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : EntityController
{
    // private float healThresholdHP = 0.2f;


    public TankAI(Tank tank)
    {
        entity = tank;
    }

    public override void Execute()
    {
        List<Ability> abilityList = entity.GetAbilityList();
        bool success = false;
        if (entity.GetDefaultTarget().GetTauntDuration() <= 0)
        {
            success = AbilityManager.ExecuteAction(abilityList[1], this, entity.GetDefaultTarget());    // Taunt
        }
        if (!success)
        {
            success = AbilityManager.ExecuteAction(abilityList[2], this, entity.GetDefaultTarget());    // Attack and Heal
        }
        if (!success)
        {
            AbilityManager.ExecuteAction(abilityList[0], this, entity.GetDefaultTarget());              // Basic Attack
        }
    }
}
