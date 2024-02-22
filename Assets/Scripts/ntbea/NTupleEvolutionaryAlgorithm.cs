using System;
using System.Collections.Generic;
using UnityEngine;


public class NTupleEvolutionaryAlgorithm
{
    private NTupleLandscape _tupleLandscape;
    private Evaluator _evaluator;
    private SearchSpace _searchSpace;
    private Mutator _mutator;
    private int _kExplore;
    private int _nSamples;
    private int _evalNeighbours;
    private float _tieBreakNoise;


    public NTupleEvolutionaryAlgorithm(NTupleLandscape tupleLandscape, Evaluator evaluator, SearchSpace searchSpace, Mutator mutator, int kExplore = 100, int nSamples = 1, int evalNeighbours = 50)
    {
        _tupleLandscape = tupleLandscape;
        _evaluator = evaluator;
        _searchSpace = searchSpace;
        _mutator = mutator;
        _kExplore = kExplore;
        _nSamples = nSamples;
        _evalNeighbours = evalNeighbours;
        _tieBreakNoise = 1e-6f;


    }

    private int[] EvaluateLandscape(int[] point)
    {
        var evaluatedNeighbours = new HashSet<string>();

        int uniqueNeighbours = 0;
        float currentBestUcb = 0;
        int[] currentBestNeighbour = null;
        var successiveNeighbourFails = 0;

        while (uniqueNeighbours < _evalNeighbours)
        {

            int[] potentialNeighbour = _mutator.Mutate(point);
            string potentialNeighbourString = string.Join(", ", potentialNeighbour);
            if (evaluatedNeighbours.Contains(potentialNeighbourString))
            {
                successiveNeighbourFails++;
                if (successiveNeighbourFails > 10000)
                {
                    throw new InvalidOperationException("Maximum attempts at finding a neighbour exceeded. Check mutations are working correctly and eval_neighbours is set to a sensible value");
                }
                continue;
            }

            successiveNeighbourFails = 0;
            uniqueNeighbours++;
            float exploit = _tupleLandscape.GetMeanEstimate(potentialNeighbour);
            float explore = _tupleLandscape.GetExplorationEstimate(potentialNeighbour);
            float noise = (float)new System.Random().NextDouble();
            float ucbWithNoise = exploit + _kExplore * explore + noise * _tieBreakNoise;

            evaluatedNeighbours.Add(potentialNeighbourString);

            if (currentBestUcb < ucbWithNoise)
            {
                currentBestUcb = ucbWithNoise;
                currentBestNeighbour = (int[])potentialNeighbour.Clone();
            }
        }

        return currentBestNeighbour;
    }

    public void Run(int nEvaluations)
    {
        _tupleLandscape.Init();

        var point = _searchSpace.GetRandomPoint();

        for (var eval = 0; eval < nEvaluations; eval++)
        {
            if (eval > 0)
            {
                point = EvaluateLandscape(point);
            }
            float fitness = 0.0f;
            for (int i = 0; i < _nSamples; i++)
            {
                fitness += _evaluator.Evaluate(point);
            }
            fitness /= _nSamples;


            _tupleLandscape.AddEvaluatedPoint(point, fitness);

            if (eval % 10 == 0)
            {
                var solution = _tupleLandscape.GetBestSampled();
                var bestFitness = _evaluator.Evaluate(solution);
                Debug.Log("Iterations: " + eval + ", Best fitness: " + bestFitness + ", Solution: " + string.Join(", ", solution));
            }
        }

        var bestSolution = _tupleLandscape.GetBestSampled();
        Debug.Log("Best solution: " + string.Join(", ", bestSolution));
        Debug.Log("Best fitness: " + _evaluator.Evaluate(bestSolution));
    }
}
