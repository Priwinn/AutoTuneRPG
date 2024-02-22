using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Mutator
{
    private SearchSpace _searchSpace;
    private bool _swapMutate;
    private bool _randomChaosMutate;
    private double _mutationPointProbability;
    private bool _flipAtLeastOne;

    public Mutator(SearchSpace searchSpace, bool swapMutate = false, bool randomChaosMutate = false, double mutationPointProbability = 1.0, bool flipAtLeastOne = true)
    {
        _searchSpace = searchSpace;
        _swapMutate = swapMutate;
        _randomChaosMutate = randomChaosMutate;
        _mutationPointProbability = mutationPointProbability;
        _flipAtLeastOne = flipAtLeastOne;
    }

    private int[] SwapMutation(int[] point)
    {
        int length = point.Length;

        System.Random random = new System.Random();
        int[] idx = new int[2];
        idx[0] = random.Next(length);
        idx[1] = random.Next(length);

        int a = point[idx[0]];
        int b = point[idx[1]];

        point[idx[0]] = b;
        point[idx[1]] = a;

        return point;
    }

    private int[] MutateValue(int[] point, int dim)
    {
        point[dim] = _searchSpace.GetValidValuesInDim(dim)[new System.Random().Next(_searchSpace.GetValidValuesInDim(dim).Length)];
        return point;
    }

    public int[] Mutate(int[] point)
    {
        int length = point.Length;
        // Perform swap mutation operation
        if (_swapMutate)
        {
            return SwapMutation(point);
        }

        // Random mutation i.e just return a random search point
        if (_randomChaosMutate)
        {
            return _searchSpace.GetRandomPoint();
        }

        // For each of the dimensions, we mutate it based on mutation_probability
        for (int dim = 0; dim < length; dim++)
        {
            if (_mutationPointProbability > new System.Random().NextDouble())
            {
                point = MutateValue(point, dim);
            }
        }

        // If we want to force flip at least one of the points then we do this here
        if (_flipAtLeastOne)
        {
            point = MutateValue(point, new System.Random().Next(length));
        }

        return point;
    }
}
