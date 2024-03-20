using MultiagentAlgorithm;

namespace GraphSplit.GraphElements
{
    public class Edge
    {
        public Vertex Vertex1 { get; private set; }
        public Vertex Vertex2 { get; private set; }
        private Color ColorLine = Color.Black;
        
        public Edge(Vertex vertex1, Vertex vertex2)
        {
            Vertex1 = vertex1 ?? throw new ArgumentNullException(nameof(vertex1));
            Vertex2 = vertex2 ?? throw new ArgumentNullException(nameof(vertex2));

            vertex1.AddEdge(this);
            vertex2.AddEdge(this);
        }

        public double weight;
        public double getLength()
        {
            double deltaX = Vertex1.Location.X - Vertex2.Location.X;
            double deltaY = Vertex1.Location.Y - Vertex2.Location.Y;

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }


        public void Draw(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            graphics.DrawLine(new Pen(ColorLine, lineWidth), Vertex1.Location, Vertex2.Location);
        }

        public bool IsInside(Point point)
        {
            double distance = PointToLineDistance(point, Vertex1.Location, Vertex2.Location);
            return distance <= lineWidth;
        }

        public void ChangeColorLine(Color color)
        {
            ColorLine = color;
        }

        private double PointToLineDistance(Point point, Point lineStart, Point lineEnd)
        {
            double dx = lineEnd.X - lineStart.X;
            double dy = lineEnd.Y - lineStart.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);

            double t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / (length * length);

            Point closestPoint;
            if (t < 0)
                closestPoint = lineStart;
            else if (t > 1)
                closestPoint = lineEnd;
            else
                closestPoint = new Point((int)(lineStart.X + t * dx), (int)(lineStart.Y + t * dy));

            return Math.Sqrt(Math.Pow(point.X - closestPoint.X, 2) + Math.Pow(point.Y - closestPoint.Y, 2));
        }



        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Edge other = (Edge)obj;
            return (Vertex1.Equals(other.Vertex1) && Vertex2.Equals(other.Vertex2)) ||
                   (Vertex1.Equals(other.Vertex2) && Vertex2.Equals(other.Vertex1));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                int hashV1 = Vertex1.GetHashCode();
                int hashV2 = Vertex2.GetHashCode();
                hash = hash * 23 + Math.Min(hashV1, hashV2);
                hash = hash * 23 + Math.Max(hashV1, hashV2);
                return hash;
            }
        }

        private int lineWidth { get { return GraphSettings.EdgeLineSize; } }

        public Vertex GetOtherVertex(Vertex vertex) 
        {
            if (Vertex1.Location == vertex.Location) return Vertex2;
            return Vertex1;
        }
    }
}
