namespace GraphSplit.GraphElements
{
    public class Vertex
    {
        public Point Location { get; private set; }
        public int Index;
        private Color BorderColor = Color.Blue;

        public List<Edge> AdjacentEdgesRender { get; } = new List<Edge>();

        public Vertex(Point location, int index)
        {
            Location = location;
            Index = index;
        }

        public Vertex(Vertex original)
        {
            Location = original.Location; 
            Index = original.Index; 
        }

        public void Draw(Graphics graphics, Graphics buffer, int index)
        {
            int diameter = Radius * 2;
            int halfRadius = Radius / 2;
            int textOffset = -halfRadius / 2;

            buffer.FillEllipse(Brushes.White, Location.X - Radius, Location.Y - Radius, diameter, diameter);
            buffer.DrawEllipse(new Pen(BorderColor, BorderWidth), Location.X - Radius, Location.Y - Radius, diameter, diameter);
            graphics.DrawString(index.ToString(), SystemFonts.DefaultFont, Brushes.Black, Location.X + textOffset, Location.Y + textOffset);
        }

        public bool IsInside(Point point)
        {
            double distanceSquared = Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2);
            return distanceSquared <= Math.Pow(Radius + BorderWidth / 2, 2);
        }

        public void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }

        public void ChangeBorderColor(Color color)
        {
            BorderColor = color;
        }

        public void AddEdge(Edge edgeRender)
        {
            if (AdjacentEdgesRender.Contains(edgeRender) == false)
                AdjacentEdgesRender.Add(edgeRender);
        }

        public void RemoveEdge(Edge edgeRender)
        {
            AdjacentEdgesRender.Remove(edgeRender);
        }

        public void RemoveConnectedVertex(Vertex removedVertex)
        {
            AdjacentEdgesRender.RemoveAll(edge => edge.Vertex1 == removedVertex || edge.Vertex2 == removedVertex);
        }

        public Color GetBorderColor() => BorderColor;

        public static List<Vertex> CloneVertices(List<Vertex> vertices)
        {
            var vertexMap = new Dictionary<Vertex, Vertex>();

            var clonedVertices = new List<Vertex>();

            foreach (var vertex in vertices)
            {
                var clonedVertex = new Vertex(vertex.Location, vertex.Index);
                clonedVertices.Add(clonedVertex);
                vertexMap[vertex] = clonedVertex;
            }

            foreach (var clonedVertex in clonedVertices)
            {
                var originalVertex = vertexMap.FirstOrDefault(kvp => kvp.Value == clonedVertex).Key;

                if (originalVertex != null)
                {
                    foreach (var edge in originalVertex.AdjacentEdgesRender)
                    {
                        var clonedVertex1 = vertexMap[edge.Vertex1];
                        var clonedVertex2 = vertexMap[edge.Vertex2];

                        var clonedEdge = new Edge(clonedVertex1, clonedVertex2);
                    }
                }
            }

            return clonedVertices;
        }

        private int Radius { get { return GraphSettings.VertexRadius; } }
        private int BorderWidth { get { return GraphSettings.VertexBorder; } }
    }
}
