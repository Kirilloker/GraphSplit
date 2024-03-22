using GraphSplit.GraphElements;
using System.Windows.Forms;

namespace GraphSplit.UIElements.Paint
{
    public class GraphTools
    {
        GraphManager graphManager;
        public GraphTools(GraphManager graphManager) { this.graphManager = graphManager; }

        public Vertex? FindVertexAtPoint(Point location) =>
            graphManager.Vertices.FirstOrDefault(v => v.IsInside(location));

        public Edge GetEdgeAtPoint(Point point)
        {
            foreach (Vertex vertex in graphManager.Vertices)
                foreach (Edge edge in vertex.AdjacentEdgesRender)
                    if (edge.IsInside(point))
                        return edge;

            return null;
        }

        private Random rnd = new Random();
        public void GenerateRandomGraph(int verticesCount, int width, int height)
        {
            const int minDistance = 100;
            const int connectionRadius = 150;

            const int maxPlacementAttempts = 1000;

            for (int i = 0; i < verticesCount; i++)
            {
                bool placed = false;
                for (int attempts = 0; attempts < maxPlacementAttempts; attempts++)
                {
                    Point point = new Point(
                        rnd.Next(minDistance, width - minDistance),
                        rnd.Next(minDistance, height - minDistance)
                    );

                    if (!graphManager.Vertices.Any(v => Distance(v.Location, point) < minDistance))
                    {
                        graphManager.CreateVertex(point);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    throw new Exception("Невозможно создать граф при таких параметрах");
                }
            }

            foreach (var vertex in graphManager.Vertices)
            {
                var potentialNeighbors = graphManager.Vertices
                    .Where(v => v != vertex && Distance(v.Location, vertex.Location) <= connectionRadius)
                    .ToList();

                if (potentialNeighbors.Count == 0)
                {
                    var closestVertex = graphManager.Vertices
                        .Where(v => v != vertex)
                        .OrderBy(v => Distance(v.Location, vertex.Location))
                        .First();

                    potentialNeighbors.Add(closestVertex);
                }

                int connectionsCount = GetNormalDistributionConnectionsCount();

                foreach (var neighbor in potentialNeighbors.OrderBy(x => rnd.Next()).Take(connectionsCount))
                {
                    if (!vertex.IsConnectedTo(neighbor))
                    {
                        graphManager.CreateEdge(vertex, neighbor);
                        vertex.ConnectedVertices.Add(neighbor);
                        neighbor.ConnectedVertices.Add(vertex);
                    }
                }
            }
        }

        private int GetNormalDistributionConnectionsCount()
        {
            double mean = 3.5;
            double stdDev = 1.0;
            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stdDev * randStdNormal;
            return (int)Math.Max(1, Math.Min(6, Math.Round(randNormal)));
        }

        private double Distance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}
