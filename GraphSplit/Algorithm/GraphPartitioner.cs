using GraphSplit.GraphElements;

namespace GraphSplit.Algorithm
{
    public class GraphPartitioner
    {
        int countSubGraph = 4;
        public List<List<Vertex>> PartitionGraph(List<Vertex> vertices, bool showAllSteps = true, int _countSubGraph = 4, bool shuffleVertex = false)
        {
            if (shuffleVertex == true)
                vertices = vertices.Shuffle();

            var steps = new List<List<Vertex>> { };
            countSubGraph = _countSubGraph;

            var coarsenedGraph = CoarsenGraph(vertices);
            if (coarsenedGraph == null) return null;

            steps.AddRange(coarsenedGraph);

            PartitionCoarsenedGraph(coarsenedGraph[^1]);

            var uncoarsenedGraph = UncoarsenGraph(coarsenedGraph);
            if (coarsenedGraph == null) return null;
            steps.AddRange(uncoarsenedGraph);

            steps.Add(DeleteEdgeWithDifferentColor(steps[^1]));

            foreach (var vertex in steps[^1])
            {
                vertex.SetDefaultColor(Color.Blue);
                vertex.ChangeBorderColor(Color.Blue);
            }

            if (showAllSteps == false) return new List<List<Vertex>>() { steps[^1] };

            return steps;
        }

        public Vertex GetVertexById(int id, List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
                if (vertex.Index == id) return vertex;
            return null;
        }

        public List<List<Vertex>> CoarsenGraph(List<Vertex> originalVertices)
        {
            var steps = new List<List<Vertex>>() { CloneVertices(originalVertices) };
            var currentStepVertices = CloneVertices(originalVertices);

            int maxAttempts = 1000;
            int attempts = 0;

            while (currentStepVertices.Count(vertex => vertex.testExist == true) > countSubGraph)
            {
                if (attempts >= maxAttempts) return null;

                var visited = new HashSet<Vertex>();

                foreach (var vertex in currentStepVertices)
                {
                    if (!visited.Contains(vertex) && vertex.testExist)
                    {
                        var connectedVertex = vertex.AdjacentEdgesRender
                                                    .Select(edge => edge.GetOtherVertex(vertex))
                                                    .FirstOrDefault(v => !visited.Contains(v));

                        if (connectedVertex != null)
                        {
                            connectedVertex = GetVertexById(connectedVertex.Index, currentStepVertices);

                            var mergedVertex = MergeVertices(vertex, connectedVertex);
                            vertex.Location = mergedVertex.Location;
                            vertex.AdjacentEdgesRender = mergedVertex.AdjacentEdgesRender;
                            vertex.Index = mergedVertex.Index;
                            connectedVertex.testExist = false;
                            connectedVertex.Location = new Point(0, 0);

                            foreach (var combinedVertexIndex in connectedVertex.CombinedVerticesIndex)
                            {
                                if (vertex.CombinedVerticesIndex.Contains(combinedVertexIndex) == false)
                                    vertex.CombinedVerticesIndex.Add(combinedVertexIndex);
                            }

                            vertex.CombinedVerticesIndex.Add(connectedVertex.Index);

                            visited.Add(connectedVertex);

                            steps.Add(CloneVertices(currentStepVertices));

                        }

                        visited.Add(vertex);
                    }
                    if (currentStepVertices.Count(vertex => vertex.testExist == true) <= countSubGraph) break;
                }
                currentStepVertices.RemoveAll(vertex => vertex.testExist == false);
                attempts++;
            }

            return steps;
        }


        public bool ShouldContinueCoarsening(List<Vertex> vertices)
        {
            if (vertices.Count <= countSubGraph) return false;
            return true;
        }

        public Vertex MergeVertices(Vertex v1, Vertex v2)
        {
            var midpoint = new Point((v1.Location.X + v2.Location.X) / 2, (v1.Location.Y + v2.Location.Y) / 2);
            var newIndex = v1.Index;
            var mergedVertex = new Vertex(midpoint, newIndex);

            var connectedVertices = new HashSet<Vertex>();
            List<Edge> connectedEdge = new();

            foreach (var edge in v1.AdjacentEdgesRender)
            {
                var otherVertex = edge.GetOtherVertex(v1);
                if (otherVertex.Index != v2.Index)
                {
                    connectedVertices.Add(otherVertex);
                    connectedEdge.Add(edge);
                }
            }

            foreach (var edge in v2.AdjacentEdgesRender)
            {
                var otherVertex = edge.GetOtherVertex(v2);
                if (otherVertex.Index != v1.Index)
                {
                    connectedVertices.Add(otherVertex);
                    connectedEdge.Add(edge);
                }
            }

            foreach (var edge in connectedEdge)
            {
                edge.Destroy();
            }

            foreach (var vertex in connectedVertices)
            {
                var newEdge = new Edge(mergedVertex, vertex);
            }

            while (v2.AdjacentEdgesRender.Count != 0)
            {
                v2.AdjacentEdgesRender[0].Destroy();
            }

            return mergedVertex;
        }



        public void PartitionCoarsenedGraph(List<Vertex> coarsenedGraph)
        {
            List<Color> colors = new List<Color>
            {
                Color.FromArgb(255, 255, 0, 0),       // Красный
                Color.FromArgb(255, 255, 255, 0),     // Желтый
                Color.FromArgb(255, 0, 255, 0),       // Зеленый
                Color.FromArgb(255, 0, 0, 0),         // Черный
                Color.FromArgb(255, 0, 0, 255),       // Синий
                Color.FromArgb(255, 255, 0, 255),     // Пурпурный
                Color.FromArgb(255, 128, 128, 128),   // Серый
                Color.FromArgb(255, 255, 127, 80),    // Коралловый
                Color.FromArgb(255, 255, 20, 147),    // Глубокий розовый
                Color.FromArgb(255, 0, 255, 255),     // Бирюзовый
                Color.FromArgb(255, 128, 0, 0),       // Темно-красный
                Color.FromArgb(255, 128, 128, 0),     // Темно-желтый
                Color.FromArgb(255, 0, 128, 0),       // Темно-зеленый
                Color.FromArgb(255, 128, 0, 128),     // Темно-пурпурный
                Color.FromArgb(255, 0, 128, 128),     // Темно-бирюзовый
                Color.FromArgb(255, 128, 128, 128),   // Темно-серый
                Color.FromArgb(255, 220, 20, 60),     // Карминный
                Color.FromArgb(255, 255, 105, 180),   // Розовый
                Color.FromArgb(255, 0, 255, 127),     // Зеленовато-голубой
                Color.FromArgb(255, 255, 0, 255)      // Фуксия
            };


            for (int i = 0; i < countSubGraph; i++)
            {
                coarsenedGraph[i].ChangeBorderColor(colors[i % colors.Count]);
                coarsenedGraph[i].SetDefaultColor(colors[i % colors.Count]);
            }

        }

        public Color GetColorByIndex(int index, List<Vertex> vertices)
        {
            foreach (var vertex in vertices)
                if (vertex.Index == index) return vertex.GetBorderColor();

            return Color.Blue;
        }

        public List<List<Vertex>> UncoarsenGraph(List<List<Vertex>> coarsenedGraphSteps)
        {
            try
            {
                var steps = new List<List<Vertex>>();

                var previousVertices = CloneVertices(coarsenedGraphSteps[^2]);

                foreach (var prevVertex in previousVertices)
                {
                    Color newColor = GetColorByIndex(prevVertex.Index, coarsenedGraphSteps[^1]);
                    prevVertex.SetDefaultColor(newColor);
                    prevVertex.ChangeBorderColor(newColor);

                    if (prevVertex.GetBorderColor() == Color.Blue)
                    {
                        foreach (var prevVertex1 in coarsenedGraphSteps[^1])
                        {
                            if (prevVertex1.GetBorderColor() == Color.Blue) continue;

                            if (prevVertex1.CombinedVerticesIndex.Contains(prevVertex.Index))
                            {
                                prevVertex.SetDefaultColor(prevVertex1.GetBorderColor());
                                prevVertex.ChangeBorderColor(prevVertex1.GetBorderColor());
                            }
                        }
                    }
                }

                steps.Add(previousVertices);

                for (int i = 3; i < coarsenedGraphSteps.Count + 1; i++)
                {
                    previousVertices = CloneVertices(coarsenedGraphSteps[^i]);

                    foreach (var prevVertex in previousVertices)
                    {
                        Color newColor = GetColorByIndex(prevVertex.Index, steps[^1]);
                        prevVertex.SetDefaultColor(newColor);
                        prevVertex.ChangeBorderColor(newColor);

                        if (prevVertex.GetBorderColor() == Color.Blue)
                        {
                            foreach (var prevVertex1 in steps[^1])
                            {
                                if (prevVertex1.GetBorderColor() == Color.Blue) continue;

                                if (prevVertex1.CombinedVerticesIndex.Contains(prevVertex.Index))
                                {
                                    prevVertex.SetDefaultColor(prevVertex1.GetBorderColor());
                                    prevVertex.ChangeBorderColor(prevVertex1.GetBorderColor());
                                }
                            }
                        }
                    }

                    steps.Add(previousVertices);
                }

                return steps;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Vertex> DeleteEdgeWithDifferentColor(List<Vertex> vertices)
        {
            var cloneVertices = CloneVertices(vertices);

            List<Edge> BadEdges = new();
            foreach (var vertex in cloneVertices)
            {
                foreach (var edge in vertex.AdjacentEdgesRender)
                {
                    if (edge.GetOtherVertex(vertex).GetBorderColor() != vertex.GetBorderColor())
                    {
                        BadEdges.Add(edge);
                    }
                }
            }

            foreach (var edge in BadEdges)
            {
                edge.Destroy();
            }

            return cloneVertices;
        }

        private List<Vertex> CloneVertices(List<Vertex> vertices)
        {
            return Vertex.CloneVertices(vertices);
        }
    }
}
