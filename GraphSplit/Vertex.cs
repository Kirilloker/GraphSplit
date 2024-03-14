using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSplit
{
    public class Vertex
    {
        public Point Location { get; set; }
        public List<Edge> AdjacentEdges { get; set; }
        public int Index { get; set; }
        private const int radius = 20;
        private const int borderWidth = 4;

        public Vertex(Point location, int index)
        {
            Location = location;
            AdjacentEdges = new List<Edge>();
            Index = index;
        }

        public void Draw(Graphics graphics, Graphics buffer, int index)
        {
            buffer.FillEllipse(Brushes.White, Location.X - radius, Location.Y - radius, 2 * radius, 2 * radius);
            buffer.DrawEllipse(new Pen(Color.Blue, borderWidth), Location.X - radius, Location.Y - radius, 2 * radius, 2 * radius);
            graphics.DrawString(index.ToString(), SystemFonts.DefaultFont, Brushes.Black, Location.X - radius / 2, Location.Y - radius / 2);
        }

        public bool IsInside(Point point)
        {
            double distanceSquared = Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2);
            return distanceSquared <= Math.Pow(radius + borderWidth / 2, 2);
        }

        public void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }
    }
}
