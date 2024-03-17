using GraphSplit.GraphElements;
using System.Drawing;

namespace GrpahSplitTest
{
    [TestFixture]
    public class EdgeTests
    {
        [Test]
        // Тест конструктора при нормальных параметрах
        public void Constructor()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);

            Assert.AreEqual(vertex1, edge.Vertex1);
            Assert.AreEqual(vertex2, edge.Vertex2);
        }

        [Test]
        // Тест конструктора с null параметром
        public void Constructor_ThrowsArgumentNullException()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);

            var vertex2 = new Vertex(new Point(200, 200), 2);

            Assert.Throws<ArgumentNullException>(() => new Edge(null, vertex2));
            Assert.Throws<ArgumentNullException>(() => new Edge(vertex1, null));

        }


        [Test]
        // Тест на отрисовку с null параметром
        public void Draw_ThrowsArgumentNullException()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);

            Assert.Throws<ArgumentNullException>(() => edge.Draw(null));
        }

        [Test]
        // Тест проходит ли ребро через точку (Точка ровно на линии)
        public void IsInside_PointOnLine()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointOnLine = new Point(150, 150);

            bool result = edge.IsInside(pointOnLine);

            Assert.IsTrue(result);
        }

        [Test]
        // Тест проходит ли ребро через точку (Точка близко с линией)
        public void IsInside_PointNear()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointNearLine = new Point(150, 152); 

            bool result = edge.IsInside(pointNearLine);

            Assert.IsTrue(result);
        }

        [Test]
        // Тест проходит ли ребро через точку (Точка далеко от линии)
        public void IsInside_PointFar()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);
            var pointFarFromLine = new Point(100, 200);

            bool result = edge.IsInside(pointFarFromLine);

            Assert.IsFalse(result);
        }

        [Test]
        // Тест проходит ли ребро через точку (Точка в одной из вершин)
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
