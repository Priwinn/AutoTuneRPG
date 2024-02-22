using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator
{
    private int runsPerIndividual;
    private float HPWeight;
    private float ManaWeight;
    private float PlayerDeathWeight;
    private float PlayerLoseWeight;
    private float baseFitness;
    private int targetRounds;

    public Evaluator(int runsPerIndividual, float HPWeight, float ManaWeight, float PlayerDeathWeight, float PlayerLoseWeight, float baseFitness, int targetRounds)
    {
        this.runsPerIndividual = runsPerIndividual;
        this.HPWeight = HPWeight;
        this.ManaWeight = ManaWeight;
        this.PlayerDeathWeight = PlayerDeathWeight;
        this.PlayerLoseWeight = PlayerLoseWeight;
        this.baseFitness = baseFitness;
        this.targetRounds = targetRounds;
    }


    public float[] FitnessFunction(List<float[]> stats)
    {
        //TODO define fitness function
        //Use sum of all params as fitness
        float fitness = 0f;
        float hpFitness = 0f;
        float manaFitness = 0f;
        float deathPenalty = 0f;
        float roundPenalty = 0f;
        float losePenalty = 0f;

        for (int i = 0; i < stats.Count; i++)
        {
            for (int j = 0; j < 6; j+=2)
            {
                hpFitness += (1-stats[i][j])/3;  
                manaFitness += (1-stats[i][j+1])/3;             
            }
            //Death penalty
            if (stats[i][0] == 0)
            {
                deathPenalty -= PlayerDeathWeight;
            }
            if (stats[i][2] == 0)
            {
                deathPenalty -= PlayerDeathWeight;
            }
            if (stats[i][4] == 0)
            {
                deathPenalty -= PlayerDeathWeight;
            }
            //Lose weight penalty
            if (stats[i][6] != 0)
            {
                losePenalty =  -PlayerLoseWeight;
            }
            //Round penalty as squared difference from target rounds
            roundPenalty += -Mathf.Pow(stats[i][7]-targetRounds,2);

            
        }
        hpFitness = HPWeight*hpFitness / stats.Count;
        manaFitness = ManaWeight*manaFitness / stats.Count;
        deathPenalty = deathPenalty / stats.Count;
        roundPenalty = roundPenalty / stats.Count;
        losePenalty = losePenalty / stats.Count;
        fitness = baseFitness + hpFitness + manaFitness + deathPenalty + roundPenalty + losePenalty;
        return new float[] {Mathf.Max(fitness,0),hpFitness,manaFitness,deathPenalty,roundPenalty,losePenalty};
    }

    public float Evaluate(int[] individual){
        GameParams Params = new GameParams(individual);
        Game game = new Game(Params);
        game.Run(runsPerIndividual);
        List<float[]> stats = game.GetStatistics();
        float[] fitnessList = FitnessFunction(stats);
        float fitness = fitnessList[0];
        return fitness;
    }
}
