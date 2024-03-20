namespace MultiagentAlgorithm
{
    public readonly struct Options
    {
        public int NumberOfAnts { get; }
        public int NumberOfPartitions { get; }
        public double MovingProbability { get; }
        public double ColoringProbability { get; }
        public int NumberOfVerticesForBalance { get; }
        public int NumberOfIterations { get; }
        public TypeWeight TypeWeightGraph { get; }

        public Options(int numberOfAnts, int numberOfPartitions, double coloringProbability,
            double movingProbability, int numberOfVerticesForBalance, int numberOfIterations, TypeWeight typeWeight)
        {
            NumberOfAnts = numberOfAnts;
            NumberOfPartitions = numberOfPartitions;
            ColoringProbability = coloringProbability;
            MovingProbability = movingProbability;
            NumberOfVerticesForBalance = numberOfVerticesForBalance;
            NumberOfIterations = numberOfIterations;
            TypeWeightGraph = typeWeight;
        }

    }


    public enum TypeWeight 
    {
        WithoutDistance,
        WithDistance,
        WithNormalizeDistance
    }
}
