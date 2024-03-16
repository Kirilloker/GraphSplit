using GraphSplit.GraphElements;
using GraphSplit.JSON;

namespace GraphSplit.UIElements
{
    public partial class PaintArea
    {
        private PictureBox pictureBox;
        private readonly MainForm mainForm;
        private List<Vertex> vertices = new List<Vertex>();
        private Vertex draggedVertex = null;
        private Point lastMouseLocation;


        public PaintArea(MainForm mainForm) 
        { 
            this.mainForm = mainForm;
            this.mainForm.EventSelectedCommand += MainForm_SelectedCommand;
            this.mainForm.UndoCommand += MainForm_UndoCommand;
        }

        public PictureBox Initialize()
        {
            pictureBox = new PictureBox
            {
                Size = new Size(600, 400),
                Location = new Point((mainForm.Width - 600) / 2, (mainForm.Height - 400) / 2),
                BackColor = Color.White
            };

            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            return pictureBox;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            foreach (Vertex vertex in vertices)
                foreach (Edge edge in vertex.AdjacentEdgesRender)
                    edge.Draw(e.Graphics);

            foreach (Vertex vertex in vertices)
                vertex.Draw(e.Graphics, e.Graphics, vertex.Index + 1);
        }

        private Vertex FindVertexAtPoint(Point location)
        {
            foreach (Vertex vertex in vertices)
                if (vertex.IsInside(location))
                    return vertex;

            return null;
        }

        private void HandleLeftButtonClick(Vertex clickedVertex)
        {
            switch (mainForm.Command)
            {
                case Command.AddVertex:
                    draggedVertex = clickedVertex;
                    lastMouseLocation = clickedVertex.Location;
                    clickedVertex.ChangeBorderColor(Color.Red);
                    break;
                case Command.DeleteElement:
                    RemoveVertex(clickedVertex);
                    break;
                case Command.AddEdge:
                    HandleAddEdgeCommand(clickedVertex);
                    break;
            }

            pictureBox.Invalidate();
        }


        private void HandleAddEdgeCommand(Vertex clickedVertex)
        {
            if (draggedVertex == null)
            {
                draggedVertex = clickedVertex;
                clickedVertex.ChangeBorderColor(Color.Yellow);
            }
            else
            {
                draggedVertex.ChangeBorderColor(Color.Blue);
                CreateEdge(draggedVertex, clickedVertex);
                draggedVertex = null;
            }
        }
        private void RemoveSelectedEdge(Point location)
        {
            Edge selectedEdge = GetEdgeAtPoint(location);

            if (selectedEdge != null)
                RemoveEdge(selectedEdge);
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Vertex clickedVertex = FindVertexAtPoint(e.Location);

            if (clickedVertex != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    RemoveVertex(clickedVertex);
                    return;
                }

                HandleLeftButtonClick(clickedVertex);
                return;
            }


            if (e.Button == MouseButtons.Left && mainForm.Command == Command.AddVertex)
            {
                CreateVertex(e.Location);
                return;
            }

            if ((e.Button == MouseButtons.Left && mainForm.Command == Command.DeleteElement) || e.Button == MouseButtons.Right)
            {
                RemoveSelectedEdge(e.Location);
                return;
            }
        }


        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((mainForm.Command == Command.AddVertex) && (draggedVertex != null))
            {
                int newX = e.Location.X;
                int newY = e.Location.Y;

                if (newX < 0)
                    newX = 0;
                else if (newX > pictureBox.Width)
                    newX = pictureBox.Width;

                if (newY < 0)
                    newY = 0;
                else if (newY > pictureBox.Height)
                    newY = pictureBox.Height;

                int deltaX = newX - lastMouseLocation.X;
                int deltaY = newY - lastMouseLocation.Y;

                draggedVertex.Move(deltaX, deltaY);
                lastMouseLocation = new Point(newX, newY);
                pictureBox.Invalidate();
            }
        }


        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (mainForm.Command == Command.AddVertex && draggedVertex != null)
            {
                draggedVertex.ChangeBorderColor(Color.Blue); 
                draggedVertex = null;
                pictureBox.Invalidate();
            }
        }

        private void CreateEdge(Vertex startVertex, Vertex endVertex)
        {
            Edge newEdge = new Edge(startVertex, endVertex);
            startVertex.AddEdge(newEdge);
            endVertex.AddEdge(newEdge); 

            pictureBox.Invalidate();
        }

        private void CreateVertex(Point location)
        {
            Vertex newVertex = new Vertex(location, vertices.Count);
            vertices.Add(newVertex);

            pictureBox.Invalidate();
        }

        private void RemoveVertex(Vertex removedVertex)
        {
            vertices.Remove(removedVertex);

            foreach (Vertex vertex in vertices)
            {
                vertex.RemoveConnectedVertex(removedVertex);
                vertex.Index = vertices.IndexOf(vertex);
            }

            pictureBox.Invalidate();
        }

        private void RemoveEdge(Edge edge)
        {
            edge.Vertex1.RemoveEdge(edge);
            edge.Vertex2.RemoveEdge(edge);

            pictureBox.Invalidate();
        }

        private Edge GetEdgeAtPoint(Point point)
        {
            foreach (Vertex vertex in vertices)
                foreach (Edge edge in vertex.AdjacentEdgesRender)
                    if (edge.IsInside(point))
                        return edge;

            return null;
        }

        private void MainForm_SelectedCommand(object sender, CommandEventArgs e)
        {
            CommandChange(e.Command);
        }

        private void MainForm_UndoCommand(object sender, EventArgs e) 
        {

        }

        public void Load(List<Vertex> vertices) 
        {
            this.vertices = vertices;
            pictureBox.Invalidate();
        }

        public void CommandChange(Command command) 
        {
            if (draggedVertex != null) 
                draggedVertex.ChangeBorderColor(Color.Blue);

            draggedVertex = null;

            pictureBox.Invalidate();
        }

        public List<Vertex> GetVertices() => vertices;
    }
}
