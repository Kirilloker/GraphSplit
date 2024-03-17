using GraphSplit.GraphElements;
using System.Drawing;

namespace GrpahSplitTest
{
    [TestFixture]
    public class EdgeTests
    {
        [Test]
        public void Constructor_WhenCalled_SetsVertices()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);

            Assert.AreEqual(vertex1, edge.Vertex1);
            Assert.AreEqual(vertex2, edge.Vertex2);
        }

        [Test]
        public void Constructor_NullVertex1_ThrowsArgumentNullException()
        {
            var vertex2 = new Vertex(new Point(200, 200), 2);

            Assert.Throws<ArgumentNullException>(() => new Edge(null, vertex2));
        }

        [Test]
        public void Constructor_NullVertex2_ThrowsArgumentNullException()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);

            Assert.Throws<ArgumentNullException>(() => new Edge(vertex1, null));
        }

        [Test]
        public void Draw_NullGraphics_ThrowsArgumentNullException()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);

            Assert.Throws<ArgumentNullException>(() => edge.Draw(null));
        }

        [Test]
        public void IsInside_PointOnLine_ReturnsTrue()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointOnLine = new Point(150, 150);

            bool result = edge.IsInside(pointOnLine);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInside_PointNearLineWithinLineWidth_ReturnsTrue()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointNearLine = new Point(150, 152); 

            bool result = edge.IsInside(pointNearLine);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInside_PointFarFromLine_ReturnsFalse()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointFarFromLine = new Point(100, 200);

            bool result = edge.IsInside(pointFarFromLine);

            Assert.IsFalse(result);
        }

        [Test]
        public void PointToLineDistance_PointExactlyOnLine_ReturnsZero()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointOnLine = new Point(150, 150);

            bool result = edge.IsInside(pointOnLine);

            Assert.IsTrue(result);
        }

        [Test]
        public void PointToLineDistance_StartPoint_ReturnsDistanceToLineStart()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var startPoint = new Point(100, 100);

            bool result = edge.IsInside(startPoint);

            Assert.IsTrue(result);
        }
    }
}
