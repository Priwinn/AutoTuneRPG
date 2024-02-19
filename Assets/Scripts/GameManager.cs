using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Party playerParty;
    Party bossParty;
    public TankParam tankParam;
    public BossParam bossParam;

    // Start is called before the first frame update
    void Start()
    {
        playerParty = new Party();
        bossParty = new Party();
        InitBossParty();
        InitPlayerParty();
        StartGame();
    }

    private void StartGame()
    {
        while (playerParty.IsAlive() && bossParty.IsAlive())
        {
            print("Player turn");
            playerParty.Execute();
            playerParty.ResolveTurn();

            print("Boss turn");
            bossParty.Execute();
            bossParty.ResolveTurn();
        }
        if (playerParty.IsAlive())
        {
            Debug.Log("PLAYER WINS");
        }
        else if (bossParty.IsAlive())
        {
            Debug.Log("BOSS WINS");
        }
        else
        {
            Debug.Log("LAMO SPAGHETTI");
        }
    }

    private void InitPlayerParty()
    {
        playerParty.Add(InitTankAI());
        playerParty.SetTargetParty(bossParty);
        playerParty.SetDefaultTarget(bossParty.membersEntity[0]);
    }

    private TankAI InitTankAI()
    {
        return new TankAI(new Tank(tankParam));
    }

    private void InitBossParty()
    {
        bossParty.Add(InitTBossAI());
        bossParty.SetTargetParty(playerParty);
    }

    private BossAI InitTBossAI()
    {
        return new BossAI(new Boss(bossParam));
    }
}
