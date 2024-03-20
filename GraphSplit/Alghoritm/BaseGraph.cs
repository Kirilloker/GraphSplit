namespace MultiagentAlgorithm
{
    public abstract class BaseGraph 
   {
      protected List<GraphSplit.GraphElements.Vertex> my_vertices;

      protected Random Rnd;

      public Vertex[] Vertices { get; protected set; }
      public int NumberOfEdges { get; set; }

      public Dictionary<int, int> Ants;

      public int MaxNumberOfAdjacentVertices;

      public abstract void InitializeGraph();

      public void ColorVerticesRandomly(int numberOfColors)
      {
         var shuffleVertices = Vertices.Shuffle(Rnd).ToList();

         for (var i = 0; i < Vertices.Length; i++)
            shuffleVertices[i].Color = i % numberOfColors + 1;
      }

      public void InitializeAnts(int numberOfAnts)
      {
         Ants = new Dictionary<int, int>();
         var counter = 0;
         foreach (var vertex in Vertices.Where(v => v.ConnectedEdges.Count > 0).Shuffle(Rnd))
         {
            Ants.Add(counter, vertex.ID);
            counter++;

            if (counter >= numberOfAnts)
               return;
         }
      }
      public void CalculateLocalCostFunction()
      {
         foreach (var vertex in Vertices)
            CalculateLocalCostFunctionForVertex(vertex);
      }

      private void CalculateLocalCostFunctionForVertex(Vertex vertex)
      {
         var connectedVertices = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).ToList();
         var differentColorCount = connectedVertices.Count(x => x.Color != vertex.Color);

         if (differentColorCount == 0)
            vertex.LocalCost = 1;
         else
            vertex.LocalCost = 1 - differentColorCount / (double)MaxNumberOfAdjacentVertices;
      }

      public int GetGlobalCostFunction()
      {
         var globalCost = 0;

         foreach (var vertex in Vertices)
         {
            var differentColorCount = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).Count(x => x.Color != vertex.Color);
            globalCost += differentColorCount;
         }

         return globalCost / 2;
      }

      public Vertex MoveAntToVertexWithLowestCost(int ant)
      {
         var vertex = Vertices[Ants[ant]];
         var worstAdjacentVertex = vertex.ConnectedEdges.First().Key;
         var lowestLocalCost = Vertices[worstAdjacentVertex].LocalCost;
         foreach (var connectedVertex in vertex.ConnectedEdges.Keys.Skip(1))
         {
            var localCost = Vertices[connectedVertex].LocalCost;
            if (localCost < lowestLocalCost)
            {
               lowestLocalCost = localCost;
               worstAdjacentVertex = connectedVertex;
            }
         }

         var worstVertex = Vertices[worstAdjacentVertex];
         Ants[ant] = worstAdjacentVertex;

         return worstVertex;
      }

      public Vertex MoveAntToAnyAdjacentVertex(int ant)
      {
         var vertex = Vertices[Ants[ant]];
         var randomAdjacentVertex = vertex.ConnectedEdges.Keys.Shuffle(Rnd).First();

         Ants[ant] = randomAdjacentVertex;

         return Vertices[randomAdjacentVertex];
      }


      public Vertex ColorVertexWithBestColor(int ant)
      {
         var vertex = Vertices[Ants[ant]];
         var bestColor = vertex.ConnectedEdges
                           .Select(connectedEdge => Vertices[connectedEdge.Key])
                           .GroupBy(v => v.Color, (color, group) => new { color, count = group.Count() })
                           .ToDictionary(tuple => tuple.color, tuple => tuple.count)
                           .OrderByDescending(x => x.Value).First();

         vertex.Color = bestColor.Key;

         return vertex;
      }

      public Vertex ColorVertexWithRandomColor(int ant, int numberOfColors)
      {
         var vertex = Vertices[Ants[ant]];
         var randomColor = Enumerable.Range(1, numberOfColors).Shuffle(Rnd).First();
         vertex.Color = randomColor;

         return vertex;
      }

      public Vertex KeepBalance(int numberOfRandomVertices, int vertexWithAntID, int oldColor, int newColor)
      {
         var random = Vertices.Where(v => v.ID != vertexWithAntID).Shuffle(Rnd).Take(numberOfRandomVertices).ToList();
         var vertexChangedColor = random.Where(vertex => vertex.Color == newColor).OrderBy(vertex => vertex.LocalCost).FirstOrDefault();
         while (vertexChangedColor == null)
         {
            vertexChangedColor = Vertices
                                .Where(v => v.ID != vertexWithAntID)
                                .Shuffle(Rnd)
                                .Take(numberOfRandomVertices)
                                .Where(vertex => vertex.Color == newColor)
                                .OrderBy(vertex => vertex.LocalCost)
                                .FirstOrDefault();
         }

         vertexChangedColor.Color = oldColor;

         return vertexChangedColor;
      }

      public void UpdateLocalCostFunction(Vertex vertexWithOldColor, Vertex vertexWithNewColor)
      {
         var neighbors = new HashSet<int>(vertexWithOldColor.ConnectedEdges.Keys);
         CalculateLocalCostFunctionForVertex(vertexWithOldColor);
         neighbors.UnionWith(vertexWithNewColor.ConnectedEdges.Keys);
         CalculateLocalCostFunctionForVertex(vertexWithNewColor);
         neighbors.Remove(vertexWithNewColor.ID);
         neighbors.Remove(vertexWithOldColor.ID);

         foreach (var neighbor in neighbors)
            CalculateLocalCostFunctionForVertex(Vertices.Single(vertex => vertex.ID == neighbor));
      }
   }
}
