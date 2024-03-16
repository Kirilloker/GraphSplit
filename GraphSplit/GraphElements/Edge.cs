namespace GraphSplit.GraphElements
{
    public class Edge
    {
        public Vertex Vertex1 { get; private set; }
        public Vertex Vertex2 { get; private set; }
        private const int lineWidth = 3;

        public Edge(Vertex vertex1, Vertex vertex2)
        {
            Vertex1 = vertex1 ?? throw new ArgumentNullException(nameof(vertex1));
            Vertex2 = vertex2 ?? throw new ArgumentNullException(nameof(vertex2));
        }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            graphics.DrawLine(new Pen(Color.Black, lineWidth), Vertex1.Location, Vertex2.Location);
        }

        public bool IsInside(Point point)
        {
            double distance = PointToLineDistance(point, Vertex1.Location, Vertex2.Location);
            return distance <= lineWidth;
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
    }
}
