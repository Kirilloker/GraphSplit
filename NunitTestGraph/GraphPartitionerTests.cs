using GraphSplit.Algorithm;
using GraphSplit.GraphElements;
using System.Drawing;

namespace GrpahSplitTest
{
    [TestFixture]
    public class GraphPartitionerTests
    {
        [Test]
        public void MergeVertices()
        {
            var partitioner = new GraphPartitioner();
            var vertex1 = new Vertex(new Point(0, 0), 1);
            var vertex2 = new Vertex(new Point(10, 10), 2);

            var mergedVertex = partitioner.MergeVertices(vertex1, vertex2);

            Assert.AreEqual(new Point(5, 5), mergedVertex.Location);
        }

        [Test]
        public void PartitionCoarsenedGraph_ColorsAssigned()
        {
            var partitioner = new GraphPartitioner();
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(0, 0), 1),
                new Vertex(new Point(10, 10), 2),
                new Vertex(new Point(20, 20), 3),
                new Vertex(new Point(30, 30), 4)
            };

            partitioner.PartitionCoarsenedGraph(vertices);

            foreach (var vertex in vertices)
            {
                Assert.AreNotEqual(Color.Blue, vertex.GetBorderColor());
            }
        }


        [Test]
        public void DeleteEdgeWithDifferentColor_EdgesDeleted()
        {
            var vertex0 = new Vertex(new Point(0, 0), 0);
            vertex0.ChangeBorderColor(Color.Red);
            vertex0.SetDefaultColor(Color.Red);
            var vertex1 = new Vertex(new Point(10, 10), 1);
            vertex1.ChangeBorderColor(Color.Red);
            vertex1.SetDefaultColor(Color.Red);
            var vertex2 = new Vertex(new Point(20, 20), 2);
            vertex2.ChangeBorderColor(Color.Blue);
            vertex2.SetDefaultColor(Color.Blue);

            var edge1 = new Edge(vertex0, vertex1); 
            var edge2 = new Edge(vertex0, vertex2); 
            var edge3 = new Edge(vertex1, vertex2); 

            var vertices = new List<Vertex> { vertex0, vertex1, vertex2 };
            var graphPartitioner = new GraphPartitioner();

            var resultVertices = graphPartitioner.DeleteEdgeWithDifferentColor(vertices);

            Assert.IsTrue(resultVertices[0].connectedTo(resultVertices[1]));
            Assert.IsFalse(resultVertices[0].connectedTo(resultVertices[2]));

            Assert.AreEqual(0, resultVertices[2].AdjacentEdgesRender.Count);
        }



        [Test]
        public void PartitionGraph_NoConnectedVertex()
        {
            var partitioner = new GraphPartitioner();
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(0, 0), 1),
                new Vertex(new Point(10, 10), 2),
                new Vertex(new Point(20, 20), 3),
                new Vertex(new Point(30, 30), 4)
            };

            var result = partitioner.PartitionGraph(vertices, false, 2);

            Assert.IsNull(result);
        }


        [Test]
        public void PartitionGraph_ConnectedVertex()
        {
            var partitioner = new GraphPartitioner();
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(0, 0), 1),
                new Vertex(new Point(10, 10), 2),
                new Vertex(new Point(20, 20), 3),
                new Vertex(new Point(30, 30), 4)
            };

            new Edge(vertices[0], vertices[1]);
            new Edge(vertices[0], vertices[2]);
            new Edge(vertices[0], vertices[3]);
            new Edge(vertices[1], vertices[2]);
            new Edge(vertices[1], vertices[3]);
            new Edge(vertices[2], vertices[3]);


            var result = partitioner.PartitionGraph(vertices, false, 2);

            Assert.IsNotNull(result);
        }


        [Test]
        public void GetVertexById_ReturnsNull()
        {
            var partitioner = new GraphPartitioner();
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(0, 0), 1),
                new Vertex(new Point(10, 10), 2)
            };

            var result = partitioner.GetVertexById(999, vertices);

            Assert.IsNull(result);
        }
    }
}
