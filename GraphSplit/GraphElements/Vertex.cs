using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GraphSplit.GraphElements
{
    public class Vertex
    {
        public Point Location { get; private set; }
        public int Index;
        private const int Radius = 20;
        private const int BorderWidth = 4;
        private Color BorderColor = Color.Blue;

        public List<Edge> AdjacentEdgesRender { get; } = new List<Edge>();

        public Vertex(Point location, int index)
        {
            Location = location;
            Index = index;
        }

        public void Draw(Graphics graphics, Graphics buffer, int index)
        {
            const int diameter = Radius * 2;
            const int halfRadius = Radius / 2;
            const int textOffset = -halfRadius / 2;

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
    }
}
