namespace GraphSplit.GraphElements
{
    public class Vertex
    {
        public Point Location;
        public int Index;
        private Color CurrentColor = Color.Blue;
        private Color DefaultColor = Color.Blue;
        public bool testExist = true;

        public List<Edge> AdjacentEdgesRender = new List<Edge>();

        public List<int> CombinedVerticesIndex { get; set; } = new List<int>();

        public Vertex(Point location, int index)
        {
            Location = location;
            Index = index;
        }
 

        public void Draw(Graphics graphics, Graphics buffer, int index)
        {
            int diameter = Radius * 2;
            int halfRadius = Radius / 2;
            int textOffset = -halfRadius / 2;

            buffer.FillEllipse(Brushes.White, Location.X - Radius, Location.Y - Radius, diameter, diameter);
            buffer.DrawEllipse(new Pen(CurrentColor, BorderWidth), Location.X - Radius, Location.Y - Radius, diameter, diameter);
            //graphics.DrawString(index.ToString(), SystemFonts.DefaultFont, Brushes.Black, Location.X + textOffset, Location.Y + textOffset);
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
            CurrentColor = color;
        }

        public void SetDefaultColor(Color color) 
        {
            DefaultColor = color;
        }

        public void ReturnDefaultColor() 
        {
            CurrentColor = DefaultColor;
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

        public Color GetBorderColor() => CurrentColor;

        public static List<Vertex> CloneVertices(List<Vertex> vertices)
        {
            var vertexMap = new Dictionary<int, Vertex>();
            var clonedVertices = new List<Vertex>();

            foreach (var vertex in vertices)
            {
                if (!vertex.testExist) continue;
                var clonedVertex = new Vertex(vertex.Location, vertex.Index);
                clonedVertex.DefaultColor = vertex.DefaultColor;
                clonedVertex.CurrentColor = vertex.CurrentColor;
                clonedVertex.testExist = vertex.testExist;
                clonedVertices.Add(clonedVertex);
                vertexMap[vertex.GetHash()] = clonedVertex;
            }

            foreach (var originalVertex in vertices)
            {
                if (!originalVertex.testExist) continue;

                var clonedVertex = vertexMap[originalVertex.GetHash()];
                foreach (var edge in originalVertex.AdjacentEdgesRender)
                {
                    var clonedVertex1 = vertexMap[edge.Vertex1.GetHash()];
                    var clonedVertex2 = vertexMap[edge.Vertex2.GetHash()];
                    if (!clonedVertex1.AdjacentEdgesRender.Exists(e => e.Vertex1 == clonedVertex1 && e.Vertex2 == clonedVertex2) && 
                        !clonedVertex1.AdjacentEdgesRender.Exists(e => e.Vertex1 == clonedVertex2 && e.Vertex2 == clonedVertex1))
                    {
                        var clonedEdge = new Edge(clonedVertex1, clonedVertex2) { };
                    }
                }

                foreach (var combinedVertexIndex in originalVertex.CombinedVerticesIndex)
                {
                    if (clonedVertex.CombinedVerticesIndex.Contains(combinedVertexIndex) == false)
                        clonedVertex.CombinedVerticesIndex.Add(combinedVertexIndex);
                }
            }

            return clonedVertices;
        }
        public bool IsConnectedTo(Vertex other)
        {
            return AdjacentEdgesRender.Any(edge => edge.Vertex1 == other || edge.Vertex2 == other);
        }

        private int Radius { get { return GraphSettings.VertexRadius; } }
        private int BorderWidth { get { return GraphSettings.VertexBorder; } }
    
        public int GetHash() 
        {
            return Index * 100000 + AdjacentEdgesRender.Count + Location.X * 3 + Location.Y * 2;
        }
    }
}
