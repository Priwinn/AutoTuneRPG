using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public readonly GameParams gameParams;
    List<int[]> gameStatisticsHistory;
    public const int statisticsCount = 7;
    bool debugMode;

    Party playerParty;
    DPS dps;
    Tank tank;
    Healer healer;

    Party bossParty;
    Boss boss;

    public Game(GameParams gameParams, bool debugMode=false)
    {
        this.gameParams = gameParams;
        this.debugMode = debugMode;
        gameStatisticsHistory = new List<int[]>();
    }

    public List<int[]> GetStatistics()
    {
        return gameStatisticsHistory;
    }

    private void InitGame()
    {
        playerParty = new Party();
        dps = new DPS(gameParams.dpsParam);
        tank = new Tank(gameParams.tankParam);
        healer = new Healer(gameParams.healerParam);
        playerParty.Add(new DPSAI(dps));
        playerParty.Add(new TankAI(tank));
        playerParty.Add(new HealerAI(healer));

        bossParty = new Party();
        boss = new Boss(gameParams.bossParam);
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
            Debug.Log("Player turn");
            playerParty.Execute();
            playerParty.ResolveTurn();
            if (!(playerParty.IsAlive() && bossParty.IsAlive()))
            {
                break;
            }
            Debug.Log("Boss turn");
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
}
