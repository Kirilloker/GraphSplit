using GraphSplit.GraphElements;
using System.Drawing;

namespace GraphSplitTests
{
    [TestFixture]
    public class VertexTests
    {
        [Test]
        public void Constructor_InitializesLocationAndIndex()
        {
            var point = new Point(10, 20);
            var vertex = new Vertex(point, 1);

            Assert.AreEqual(new Point(10, 20), vertex.Location);
            Assert.AreEqual(1, vertex.Index);
        }

        [Test]
        public void IsInside_PointInside_ReturnsTrue()
        {
            var vertex = new Vertex(new Point(100, 100), 1);
            var pointInside = new Point(105, 105); 

            bool result = vertex.IsInside(pointInside);

            Assert.IsTrue(result);
        }

        [Test]
        public void IsInside_PointOutside_ReturnsFalse()
        {
            var vertex = new Vertex(new Point(100, 100), 1);
            var pointOutside = new Point(130, 130); 

            bool result = vertex.IsInside(pointOutside);

            Assert.IsFalse(result);
        }

        [Test]
        public void Move_ChangesLocation()
        {
            var initialLocation = new Point(100, 100);
            var vertex = new Vertex(initialLocation, 1);
            int deltaX = 10, deltaY = 20;

            vertex.Move(deltaX, deltaY);

            var expectedLocation = new Point(initialLocation.X + deltaX, initialLocation.Y + deltaY);
            Assert.AreEqual(expectedLocation, vertex.Location);
        }

        [Test]
        public void ChangeBorderColor_ChangesBorderColor()
        {
            var vertex = new Vertex(new Point(100, 100), 1);
            var newColor = Color.Red;

            vertex.ChangeBorderColor(newColor);

            Assert.AreEqual(newColor, vertex.GetBorderColor());
        }

        [Test]
        public void AddEdge_IncreasesAdjacentEdgesCount()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);

            Assert.AreEqual(1, vertex1.AdjacentEdgesRender.Count);
        }

        [Test]
        public void RemoveEdge_DecreasesAdjacentEdgesCount()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var edge = new Edge(vertex1, vertex2);

            vertex1.RemoveEdge(edge);

            Assert.AreEqual(0, vertex1.AdjacentEdgesRender.Count);
        }

        [Test]
        public void RemoveConnectedVertex_RemovesCorrectEdges()
        {
            var vertex1 = new Vertex(new Point(100, 100), 1);
            var vertex2 = new Vertex(new Point(200, 200), 2);
            var vertex3 = new Vertex(new Point(300, 300), 3);
            var edge1 = new Edge(vertex1, vertex2);
            var edge2 = new Edge(vertex1, vertex3);

            vertex1.RemoveConnectedVertex(vertex2);

            Assert.IsFalse(vertex1.AdjacentEdgesRender.Contains(edge1));
            Assert.IsTrue(vertex1.AdjacentEdgesRender.Contains(edge2));
        }

        [Test]
        public void CloneVertices_CreatesDeepCopyOfVertices()
        {
            var originalVertices = new List<Vertex>
            {
                new Vertex(new Point(100, 100), 1),
                new Vertex(new Point(200, 200), 2)
            };
            var edge = new Edge(originalVertices[0], originalVertices[1]);

            var clonedVertices = Vertex.CloneVertices(originalVertices);

            Assert.AreEqual(originalVertices.Count, clonedVertices.Count);
            for (int i = 0; i < originalVertices.Count; i++)
            {
                Assert.AreNotSame(originalVertices[i], clonedVertices[i]);
                Assert.AreEqual(originalVertices[i].Location, clonedVertices[i].Location);
                Assert.AreEqual(originalVertices[i].Index, clonedVertices[i].Index);
            }
        }

        [Test]
        public void CloneVertices_CreatesDeepCopyOfEdges()
        {
            var originalVertices = new List<Vertex>
            {
                new Vertex(new Point(100, 100), 1),
                new Vertex(new Point(200, 200), 2)
            };
            var edge = new Edge(originalVertices[0], originalVertices[1]);
            originalVertices[0].AddEdge(edge);
            originalVertices[1].AddEdge(edge);

            var clonedVertices = Vertex.CloneVertices(originalVertices);
            var clonedEdge = clonedVertices[0].AdjacentEdgesRender.FirstOrDefault();

            Assert.IsNotNull(clonedEdge);
            Assert.AreNotSame(edge, clonedEdge);
            Assert.AreNotSame(edge.Vertex1, clonedEdge.Vertex1);
            Assert.AreNotSame(edge.Vertex2, clonedEdge.Vertex2);
            Assert.AreEqual(edge.Vertex1.Location, clonedEdge.Vertex1.Location);
            Assert.AreEqual(edge.Vertex2.Location, clonedEdge.Vertex2.Location);
        }

    }
}