using GraphSplit.Forms;
using GraphSplit.GraphElements;
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

        public Vertex CreateVertex(Point location)
        {
            graphUndo.UpdateUndoHistory(vertices);

            Vertex newVertex = new Vertex(location, vertices.Count);
            vertices.Add(newVertex);

            paintArea.RefreshPaint();

            return newVertex;
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
        public bool GenerateRandomGraph(int verticesCount, int width, int height, int minDistance, int connectionRadius)
        {
            return graphTools.GenerateRandomGraph(verticesCount, width, height, minDistance, connectionRadius);
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


        GraphPartitioner algTest = new GraphPartitioner();


        bool firstClick = true;
        List<List<Vertex>> test = new();
        int click = 2;


        private void AlgorithmApply(object sender, EventArgs e)
        {
            if (firstClick == true) 
            {
                UpdateUndoHistory();
                firstClick = false;
            }

            foreach (var vertex in vertices)
            {
                vertex.SetDefaultColor(Color.Blue);
                vertex.ChangeBorderColor(Color.Blue);
            }

            test = algTest.PartitionGraph(Vertex.CloneVertices(vertices), false, click);
            Load(test[0]);
            UpdateUndoHistory();

            click++;
        }

        //private void AlgorithmApply(object sender, EventArgs e)
        //{
        //    if (firstClick == true) 
        //    {
        //        var algTest = new GraphPartitioner();
        //        test = algTest.PartitionGraph(Vertex.CloneVertices(vertices), false);
        //        firstClick = false;
        //    }

        //    if (click >= test.Count) return;

        //    Load(test[click]);
        //    click++;

        //    return;
        //}
    }
}
