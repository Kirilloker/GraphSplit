namespace MultiagentAlgorithm
{
    public class GraphWithoutWeight : BaseGraph
    {
    private const int VertexWeight = 1;
    private const int EdgeWeight = 1;

        public GraphWithoutWeight(List<GraphSplit.GraphElements.Vertex> vertices, Random rnd)
        {
            this.my_vertices = vertices;
            Rnd = rnd;
        }

        public override void InitializeGraph()
        {
            int counter = 0;
            int number_edges = 0;

            Vertices = new Vertex[my_vertices.Count];

            foreach (var my_vert in my_vertices)
            {
                Vertices[my_vert.Index] = new Vertex(my_vert.Index, VertexWeight);

                var connectedEdges = new Dictionary<int, int>();

                foreach (var edges in my_vert.AdjacentEdgesRender)
                {
                    connectedEdges.Add(edges.GetOtherVertex(my_vert).Index, EdgeWeight);
                    number_edges += 1;
                }
                
                Vertices[my_vert.Index].ConnectedEdges = connectedEdges;

                counter++;
            }

            NumberOfEdges = number_edges;
            MaxNumberOfAdjacentVertices = Vertices.Max(verex => verex.ConnectedEdges.Count);
          }
   }
}
