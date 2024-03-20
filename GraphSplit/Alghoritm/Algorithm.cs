using System.Diagnostics;

namespace MultiagentAlgorithm
{
    public static class Algorithm
   {
      public static Vertex[] Run(BaseGraph graph, Options options, Random rnd)
      {
         graph.InitializeGraph();
         graph.InitializeAnts(options.NumberOfAnts);
         graph.ColorVerticesRandomly(options.NumberOfPartitions);
         graph.CalculateLocalCostFunction();

         var bestCost = graph.GetGlobalCostFunction();
         var bestCostIteration = 0;
         var bestDistribution = (Vertex[])graph.Vertices.Clone();
         var iteration = 0;

         while (bestCost > 0 && iteration < options.NumberOfIterations)
         {
            foreach (var ant in graph.Ants.Keys.ToArray())
            {
               Vertex vertexWithAnt;
               if (rnd.NextDouble() < options.MovingProbability)
                  vertexWithAnt = graph.MoveAntToVertexWithLowestCost(ant);
               else
                  vertexWithAnt = graph.MoveAntToAnyAdjacentVertex(ant);

               int oldColor = vertexWithAnt.Color;
               int vertexWithAntID = vertexWithAnt.ID;

               Vertex vertexWithNewColor;
               if (rnd.NextDouble() < options.ColoringProbability)
                  vertexWithNewColor = graph.ColorVertexWithBestColor(ant);
               else
                  vertexWithNewColor = graph.ColorVertexWithRandomColor(ant, options.NumberOfPartitions);

               Vertex vertexWhichKeepBalance = graph.KeepBalance(options.NumberOfVerticesForBalance, vertexWithAntID, oldColor, vertexWithNewColor.Color);

               graph.UpdateLocalCostFunction(vertexWhichKeepBalance, vertexWithNewColor);

               var globalCost = graph.GetGlobalCostFunction();
               if (globalCost < bestCost)
               {
                  bestCost = globalCost;
                  bestCostIteration = iteration;
                  bestDistribution = (Vertex[])graph.Vertices.Clone();
               }
            }

            iteration++;
         }


         return bestDistribution;
      }
   }
}
