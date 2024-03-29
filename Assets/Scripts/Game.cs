using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public readonly GameParams gameParams;
    List<float[]> gameStatisticsHistory;
    public const int statisticsCount = 8;
    bool printMode;

    Party playerParty;
    DPS dps;
    Tank tank;
    Healer healer;

    Party bossParty;
    Boss boss;

    public Game(GameParams gameParams, bool printMode=false)
    {
        this.gameParams = gameParams;
        this.printMode = printMode;
        AbilityManager.printMode = printMode;
        gameStatisticsHistory = new List<float[]>();
    }

    public List<float[]> GetStatistics()
    {
        return gameStatisticsHistory;
    }

    private void InitGame()
    {
        playerParty = new Party("Player Party", printMode);
        dps = new DPS(gameParams.dpsParam, printMode);
        tank = new Tank(gameParams.tankParam, printMode);
        healer = new Healer(gameParams.healerParam, printMode);
        playerParty.Add(new DPSAI(dps));
        playerParty.Add(new TankAI(tank));
        playerParty.Add(new HealerAI(healer));

        bossParty = new Party("Boss Party", printMode);
        boss = new Boss(gameParams.bossParam, printMode);
        bossParty.Add(new BossAI(boss));
        bossParty.SetTargetParty(playerParty);

        playerParty.SetTargetParty(bossParty);
        playerParty.SetDefaultTarget(boss);
    }

    // Run games count time
    public void Run(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int turn_count = StartGame();
            float[] gameStatistics = { dps.GetHPPercentage(), dps.GetManaPercentage(), tank.GetHPPercentage(), tank.GetManaPercentage(), healer.GetHPPercentage(), healer.GetManaPercentage(), boss.GetHPPercentage(), turn_count };
            gameStatisticsHistory.Add(gameStatistics);
        }
    }

    // Return turn count
    private int StartGame()
    {
        InitGame();
        int turn_count = 0;
        while (playerParty.IsAlive() && bossParty.IsAlive())
        {
            if (turn_count > 100)
            {
                Debug.Log("WHY IS IT STILL GOING??");
                break;
            }
            turn_count++;
            if (printMode)
            {
                Debug.Log("Turn " + turn_count);
                Debug.Log("Player turn");
            }
            playerParty.Execute();
            playerParty.ResolveTurn();
            if (printMode)
            {
                printGameState();
            }
            if (!(playerParty.IsAlive() && bossParty.IsAlive()))
            {
                break;
            }
            if (printMode)
            {
                Debug.Log("Boss turn");
            }
            bossParty.Execute();
            bossParty.ResolveTurn();
            if (printMode)
            {
                printGameState();
            }
        }
        if (!printMode)
        {
            //Debug.Log(turn_count);
            return turn_count;
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
        return turn_count;
    }

    private void printGameState()
    {
        //Debug.Log("Player Party:");
        //Debug.Log(playerParty.membersEntity.Count);
        foreach (Entity entity in playerParty.membersEntity)
        {
            Debug.Log(entity.EntityStateString());
        }
        //Debug.Log("Boss Party:");
        //Debug.Log(bossParty.membersEntity);
        foreach (Entity entity in bossParty.membersEntity)
        {
            Debug.Log(entity.EntityStateString());
        }
    }
}
