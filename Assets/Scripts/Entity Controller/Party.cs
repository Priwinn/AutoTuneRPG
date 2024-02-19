using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party
{
    public readonly List<Entity> membersEntity;
    public readonly List<EntityController> membersController;
    public readonly List<Entity> graveyardEntity;
    public readonly List<EntityController> graveyardController;

    public Party()
    {
        membersEntity = new List<Entity>();
        graveyardEntity = new List<Entity>();

        membersController = new List<EntityController>();
        graveyardController = new List<EntityController>();
    }

    public void Add(EntityController controller)
    {
        membersController.Add(controller);
        membersEntity.Add(controller.GetEntity());
        controller.SetParty(this);
    }

    public void Execute()
    {
        foreach (EntityController controller in membersController)
        {
            controller.Execute();
        }
    }

    public void ResolveTurn()
    {
        for (int i = 0; i < membersEntity.Count; i++)
        {
            if (membersEntity[i].GetHP() <= 0)
            {
                Debug.Log(membersEntity[i]);
                graveyardEntity.Add(membersEntity[i]);
                graveyardController.Add(membersController[i]);
                membersEntity.RemoveAt(i);
                membersController.RemoveAt(i);
                i--;
                Debug.Log("SOMONE FUCKING DIE");
            }
            else
            {
                membersEntity[i].ResolveTurn();
            }
        }
        
    }

    public void AOEDamage(int damage)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.Damage(damage);
        }
    }

    public void AOEHeal(int heal)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.Heal(heal);
        }
    }

    public void AOEAttackBuff(int buff, int duration)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.BuffAttack(buff, duration);
        }
    }

    public void AOEAttackDebuff(int debuff, int duration)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.DebuffAttack(debuff, duration);
        }
    }

    public void AOEDefenseBuff(int buff, int duration)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.BuffDefense(buff, duration);
        }
    }

    public void AOEDefenseDebuff(int debuff, int duration)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.DebuffDefense(debuff, duration);
        }
    }

    public void SetDefaultTarget(Entity target)
    {
        foreach (Entity entity in membersEntity)
        {
            entity.SetDefaultTarget(target);
        }
    }

    public void SetTargetParty(Party p)
    {
        foreach (EntityController controller in membersController)
        {
            controller.SetTargetParty(p);
        }
    }

    public bool IsAlive()
    {
        return (membersEntity.Count > 0);
    }

}
