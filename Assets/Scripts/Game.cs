using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public readonly GameParams gameParams;
    List<int[]> gameStatisticsHistory;
    public const int statisticsCount = 7;
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
        gameStatisticsHistory = new List<int[]>();
    }

    public List<int[]> GetStatistics()
    {
        return gameStatisticsHistory;
    }

    private void InitGame()
    {
        playerParty = new Party(printMode);
        dps = new DPS(gameParams.dpsParam, printMode);
        tank = new Tank(gameParams.tankParam, printMode);
        healer = new Healer(gameParams.healerParam, printMode);
        playerParty.Add(new DPSAI(dps));
        playerParty.Add(new TankAI(tank));
        playerParty.Add(new HealerAI(healer));

        bossParty = new Party(printMode);
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
            StartGame();
            int[] gameStatistics = { dps.GetPercentHP(), dps.GetMana(), tank.GetPercentHP(), tank.GetMana(), healer.GetPercentHP(), healer.GetMana(), boss.GetPercentHP() };
            gameStatisticsHistory.Add(gameStatistics);
        }
    }

    private void StartGame()
    {
        InitGame();
        while (playerParty.IsAlive() && bossParty.IsAlive())
        {
            if (printMode)
            {
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
                printGameState();
            }
            if (printMode)
            {
                Debug.Log("Boss turn");
            }
            bossParty.Execute();
            bossParty.ResolveTurn();
        }
        if (!printMode)
        {
            return;
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

    private void printGameState()
    {
        Debug.Log("Player Party:");
        foreach (Entity entity in playerParty.membersEntity)
        {
            Debug.Log(entity.EntityStateString());
        }
        Debug.Log("Boss Party:");
        foreach (Entity entity in bossParty.membersEntity)
        {
            Debug.Log(entity.EntityStateString());
        }
    }
}
