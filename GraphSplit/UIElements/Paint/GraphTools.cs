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


        public void GenerateRandomGraph(int verticesCount, int width, int height)
        {
            Random rnd = new Random();
            int vertexRadius = GraphSettings.VertexRadius;

            for (int i = 0; i < verticesCount;)
            {
                Point randomPoint = new Point(rnd.Next(vertexRadius, width - vertexRadius), rnd.Next(vertexRadius, height - vertexRadius));
                bool isOverlapping = graphManager.Vertices.Any(v => Math.Sqrt(Math.Pow(v.Location.X - randomPoint.X, 2) + Math.Pow(v.Location.Y - randomPoint.Y, 2)) < vertexRadius * 2);
                if (!isOverlapping)
                {
                    graphManager.CreateVertex(randomPoint);
                    i++;
                }
            }

            int edgesCount = rnd.Next(verticesCount - 1, Math.Min(3 * verticesCount, verticesCount * (verticesCount - 1) / 2));
            for (int i = 0; i < edgesCount; i++)
            {
                Vertex startVertex = graphManager.Vertices[rnd.Next(verticesCount)];
                Vertex endVertex;
                do
                {
                    endVertex = graphManager.Vertices[rnd.Next(verticesCount)];
                } while (startVertex == endVertex || startVertex.IsConnectedTo(endVertex));

                graphManager.CreateEdge(startVertex, endVertex);
            }
        }

    }
}
