using GraphSplit.Forms;
using GraphSplit.GraphElements;
using MultiagentAlgorithm;
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

            AlgorithmForm.AlgoritmApply += AlgorithmApply;
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
                vertex.ReturnDefaultColor();

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


        private void AlgorithmApply(object sender, EventArgs e)
        {
            var options = AlgorithmForm.getOption();
            var rnd = new Random(Environment.TickCount);
            BaseGraph graph; 
            graph = new GraphWithoutWeight(Vertex.CloneVertices(vertices), rnd);

            switch (options.TypeWeightGraph)
            {
                case TypeWeight.WithoutDistance:
                    graph = new GraphWithoutWeight(Vertex.CloneVertices(vertices), rnd);
                    break;
                case TypeWeight.WithDistance:
                    foreach (var vertex in vertices)
                        foreach (var edge in vertex.AdjacentEdgesRender)
                            edge.weight = edge.getLength();
                    break;
                case TypeWeight.WithNormalizeDistance:
                    
                    double maxLenght = 0;
                    foreach (var vertex in vertices)
                        foreach (var edge in vertex.AdjacentEdgesRender)
                            maxLenght = Math.Max(maxLenght, edge.getLength());

                    foreach (var vertex in vertices)
                        foreach (var edge in vertex.AdjacentEdgesRender)
                            edge.weight = ((maxLenght - edge.getLength()) / maxLenght) * 100;

                    break;
                default:
                    break;
            }

            var resultData = Algorithm.Run(graph, options, rnd);

            Color[] color = { Color.Red, Color.Yellow, Color.Green, Color.Blue,
                              Color.Brown, Color.DarkGray, Color.DarkGreen,
                              Color.Gold, Color.DarkOliveGreen, Color.DarkSalmon,
                              Color.DarkOrange, Color.DarkSlateGray, Color.DarkViolet,};

            foreach (var item in resultData)
            {
                var vertex = vertices.FirstOrDefault(v => v.Index == item.ID);
                if (vertex != null) 
                {
                    vertex.SetDefaultColor(color[item.Color]);
                    vertex.ReturnDefaultColor();
                }
            }

            paintArea.RefreshPaint();

        }
    }
}
