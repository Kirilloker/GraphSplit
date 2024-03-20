using GraphSplit.GraphElements;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MultiagentAlgorithm;
using System.Windows.Forms;
using Vertex = GraphSplit.GraphElements.Vertex;

namespace GraphSplit.UIElements.Paint
{
    public class GraphManager
    {
        private List<Vertex> vertices = new();
        public List<Vertex> Vertices { get { return vertices; } set { vertices = value; } }

        GraphUndo graphUndo;
        GraphTools graphTools;
        PaintArea paintArea;
        public GraphManager(PaintArea paintArea) 
        {
            this.paintArea = paintArea;
            graphUndo = new(this);
            graphTools = new(this);

            CommandHandler.Right += PressRight;
        }


        public void CreateEdge(Vertex startVertex, Vertex endVertex)
        {
            graphUndo.UpdateUndoHistory(vertices);

            Edge newEdge = new Edge(startVertex, endVertex);

            paintArea.RefreshPaint();
        }

        public bool TryRemoveSelectedVertex(Point location)
        {
            Vertex selectedVertex = FindVertexAtPoint(location);

            if (selectedVertex != null)
            {
                RemoveVertex(selectedVertex);
                return true;
            }

            return false;
        }

        public bool TryRemoveSelectedEdge(Point location)
        {
            Edge selectedEdge = graphTools.GetEdgeAtPoint(location);

            if (selectedEdge != null) 
            {
                RemoveEdge(selectedEdge);
                return true;
            }

            return false; 
        }

        public void RemoveEdge(Edge edge)
        {
            graphUndo.UpdateUndoHistory(vertices);

            edge.Vertex1.RemoveEdge(edge);
            edge.Vertex2.RemoveEdge(edge);

            paintArea.RefreshPaint();
        }

        public void CreateVertex(Point location)
        {
            graphUndo.UpdateUndoHistory(vertices);

            Vertex newVertex = new Vertex(location, vertices.Count);
            vertices.Add(newVertex);

            paintArea.RefreshPaint();
        }

        public void RemoveVertex(Vertex removedVertex)
        {
            graphUndo.UpdateUndoHistory(vertices);

            vertices.Remove(removedVertex);

            foreach (Vertex vertex in vertices)
            {
                vertex.RemoveConnectedVertex(removedVertex);
                vertex.Index = vertices.IndexOf(vertex);
            }

            paintArea.RefreshPaint();
        }

        public void UpdateUndoHistory() 
        {
            graphUndo.UpdateUndoHistory(vertices);
        }

        public void SetDefaultColorAllElements() 
        {
            foreach (var vertex in vertices) 
            {
                vertex.ChangeBorderColor(Color.Blue);

                foreach (var edge in vertex.AdjacentEdgesRender)
                    edge.ChangeColorLine(Color.Black);
            }
        }

        public List<Vertex> SelectVerticesWithin(Rectangle selectionRectangle)
        {
            var selectedVertices = new List<Vertex>();

            foreach (var vertex in vertices)
            {
                if (selectionRectangle.Contains(vertex.Location))
                {
                    vertex.ChangeBorderColor(Color.Red);
                    selectedVertices.Add(vertex);

                    foreach (var edge in vertex.AdjacentEdgesRender)
                        edge.ChangeColorLine(Color.Red);
                }
            }

            return selectedVertices;
        }

        public void RemoveVerticesWithin(Rectangle selectionRectangle)
        {
            foreach (var vertex in vertices.ToArray())
                if (selectionRectangle.Contains(vertex.Location))
                    RemoveVertex(vertex);
        }

        public Vertex FindVertexAtPoint(Point location) => graphTools.FindVertexAtPoint(location);
        public void GenerateRandomGraph(int verticesCount, int width, int height)
        {
            graphTools.GenerateRandomGraph(verticesCount, width, height);
        }

        public void Load(List<Vertex> vertices)
        {
            this.vertices = vertices;
            paintArea.RefreshPaint();
        }

        public void Clear()
        {
            vertices.Clear();
            graphUndo.Clear();
        }














        int test = -1;
        bool first_test = false;
        List<Vertex> originaltest;


        private void PressRight(object sender, EventArgs e)
        {
            if (first_test == false)
            {


                double maxLenght = 0;
                foreach (var vertex in vertices)
                {
                    foreach (var edge in vertex.AdjacentEdgesRender)
                    {
                        double lenght = edge.getLength();
                        
                        if (lenght > maxLenght) 
                        {
                            maxLenght = lenght;
                        }
                    }
                }

                foreach (var vertex in vertices)
                {
                    foreach (var edge in vertex.AdjacentEdgesRender)
                    {
                        double length = edge.getLength();

                        edge.weight = ((maxLenght - length) / maxLenght) * 100;
                        //edge.weight = length;
                    }
                }

                first_test = true;
                originaltest = Vertex.CloneVertices(Vertices);
            }

            //var vrt = GetConnectedComponents(Vertex.CloneVertices(originaltest));
            //var x = new MultiLevelGraphPartitioning();
            //var vrt = x.Partition(Vertex.CloneVertices(originaltest), 3);

            //!!!!!!!!!!!!!!!!!!!!!
            var rnd = new Random(Environment.TickCount);

            BaseGraph graph;
            //graph = new MetisUnweightedGraph(Vertex.CloneVertices(originaltest), rnd);
            graph = new GraphWithWeight(Vertex.CloneVertices(vertices), rnd);
            
            var graphOptions = new Options(
                numberOfAnts: 3,
                numberOfPartitions: 2,
                coloringProbability: 0.95,
                movingProbability: 0.95,
                numberOfVerticesForBalance: 20,
                numberOfIterations: 500
            );


            var resultData = Algorithm.Run(graph, graphOptions, rnd);

            Color[] color = { Color.Red, Color.Yellow, Color.Green, Color.Blue };
            foreach (var item in resultData)
            {
                var vertex = vertices.FirstOrDefault(v => v.Index == item.ID);
                if (vertex != null)
                    vertex.ChangeBorderColor(color[item.Color]);
            }

            paintArea.RefreshPaint();
            //Load(vertices);
        }

    }
}
