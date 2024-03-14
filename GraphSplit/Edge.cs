using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSplit
{
    public class Edge
    {
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }
        public List<Vertex> ConnectedVertices { get; set; }
        private const int lineWidth = 3;

        public Edge(Vertex vertex1, Vertex vertex2)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            ConnectedVertices = new List<Vertex> { vertex1, vertex2 };
        }

        public void Draw(Graphics graphics)
        {
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

            double u = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / (length * length);

            Point closestPoint;
            if (u < 0)
            {
                closestPoint = lineStart;
            }
            else if (u > 1)
            {
                closestPoint = lineEnd;
            }
            else
            {
                closestPoint = new Point((int)(lineStart.X + u * dx), (int)(lineStart.Y + u * dy));
            }

            return Math.Sqrt(Math.Pow(point.X - closestPoint.X, 2) + Math.Pow(point.Y - closestPoint.Y, 2));
        }
    }
}
