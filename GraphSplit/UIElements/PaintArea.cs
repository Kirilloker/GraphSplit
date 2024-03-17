using GraphSplit.GraphElements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphSplit.UIElements
{
    public partial class PaintArea
    {
        private const int Width = 1000;
        private const int Height = 700;

        private PictureBox pictureBox;
        private readonly MainForm mainForm;
        private List<Vertex> vertices = new();
        private readonly List<List<Vertex>> undoHistory = new();
        private const int maxUndoCount = 30;

        private Point lastMouseLocation;
        private Vertex draggedVertex = null;

        private Rectangle selectionRectangle = Rectangle.Empty;
        private bool isSelecting = false;

        public PaintArea(MainForm mainForm)
        {
            this.mainForm = mainForm;
            CommandHandler.CommandSelected += MainForm_SelectedCommand;
            CommandHandler.UndoCommand += MainForm_UndoCommand;
        }

        public PictureBox Initialize()
        {
            pictureBox = new PictureBox
            {
                Size = new Size(Width, Height),
                Location = new Point((mainForm.Width - Width) / 2, (mainForm.Height - Height) / 2),
                BackColor = Color.White
            };

            GraphSettings.SettingsChange += RefreshPaint;

            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            return pictureBox;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (!selectionRectangle.IsEmpty)
            {
                using (Pen pen = new Pen(Color.Blue) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    e.Graphics.DrawRectangle(pen, selectionRectangle);
                }
            }

            foreach (Vertex vertex in vertices)
                foreach (Edge edge in vertex.AdjacentEdgesRender)
                    edge.Draw(e.Graphics);

            foreach (Vertex vertex in vertices)
                vertex.Draw(e.Graphics, e.Graphics, vertex.Index + 1);
        }

        private Vertex? FindVertexAtPoint(Point location) =>
            vertices.FirstOrDefault(v => v.IsInside(location));

        private void HandleLeftButtonClick(Vertex clickedVertex)
        {
            switch (CommandHandler.Command)
            {
                case Command.AddVertex:

                    UpdateUndoHistory();

                    draggedVertex = clickedVertex;
                    lastMouseLocation = clickedVertex.Location;
                    clickedVertex.ChangeBorderColor(Color.Red);
                    foreach (var edge in clickedVertex.AdjacentEdgesRender)
                        edge.ChangeColorLine(Color.Red);

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


            if (e.Button == MouseButtons.Left && CommandHandler.Command == Command.AddVertex)
            {
                CreateVertex(e.Location);
                return;
            }

            if (e.Button == MouseButtons.Left && CommandHandler.Command == Command.DeleteElement)
            {
                lastMouseLocation = e.Location;
                isSelecting = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                RemoveSelectedEdge(e.Location);
                return;
            }
        }


        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point currentMouseLocation = e.Location;
                    selectionRectangle = new Rectangle(
                        Math.Min(lastMouseLocation.X, currentMouseLocation.X),
                        Math.Min(lastMouseLocation.Y, currentMouseLocation.Y),
                        Math.Abs(lastMouseLocation.X - currentMouseLocation.X),
                        Math.Abs(lastMouseLocation.Y - currentMouseLocation.Y));
                    pictureBox.Invalidate();
                }
            }
            else
            {
                if (CommandHandler.Command == Command.AddVertex && draggedVertex is not null)
                {
                    var (newX, newY) = ConstrainMouseLocation(e.Location);
                    var deltaX = newX - lastMouseLocation.X;
                    var deltaY = newY - lastMouseLocation.Y;

                    draggedVertex.Move(deltaX, deltaY);
                    lastMouseLocation = new Point(newX, newY);
                    pictureBox.Invalidate();
                }
            }
        }

        private (int X, int Y) ConstrainMouseLocation(Point location)
        {
            var newX = Math.Clamp(location.X, 0, pictureBox.Width);
            var newY = Math.Clamp(location.Y, 0, pictureBox.Height);
            return (newX, newY);
        }


        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                isSelecting = false;
                foreach (var vertex in vertices.ToArray())
                {
                    if (selectionRectangle.Contains(vertex.Location))
                    {
                        RemoveVertex(vertex);
                    }
                }
                selectionRectangle = Rectangle.Empty;
                pictureBox.Invalidate();
            }
            else
            {
                if (CommandHandler.Command == Command.AddVertex && draggedVertex != null)
                {
                    draggedVertex.ChangeBorderColor(Color.Blue);
                    foreach (var edge in draggedVertex.AdjacentEdgesRender)
                        edge.ChangeColorLine(Color.Black);

                    draggedVertex = null;
                    pictureBox.Invalidate();
                    UpdateUndoHistory();
                }
            }
        }

        public void CreateEdge(Vertex startVertex, Vertex endVertex)
        {
            UpdateUndoHistory();
            Edge newEdge = new Edge(startVertex, endVertex);

            pictureBox.Invalidate();
        }

        public void CreateVertex(Point location)
        {
            UpdateUndoHistory();
            Vertex newVertex = new Vertex(location, vertices.Count);
            vertices.Add(newVertex);

            pictureBox.Invalidate();
        }

        public void RemoveVertex(Vertex removedVertex)
        {
            UpdateUndoHistory();
            vertices.Remove(removedVertex);

            foreach (Vertex vertex in vertices)
            {
                vertex.RemoveConnectedVertex(removedVertex);
                vertex.Index = vertices.IndexOf(vertex);
            }

            pictureBox.Invalidate();
        }

        public void RemoveEdge(Edge edge)
        {
            UpdateUndoHistory();
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

        public void GenerateRandomGraph(int verticesCount)
        {
            Random rnd = new Random();
            Clear(); 
            int vertexRadius = GraphSettings.VertexRadius; 

            for (int i = 0; i < verticesCount;)
            {
                Point randomPoint = new Point(rnd.Next(vertexRadius, Width - vertexRadius), rnd.Next(vertexRadius, Height - vertexRadius));
                bool isOverlapping = vertices.Any(v => Math.Sqrt(Math.Pow(v.Location.X - randomPoint.X, 2) + Math.Pow(v.Location.Y - randomPoint.Y, 2)) < vertexRadius * 2);
                if (!isOverlapping)
                {
                    CreateVertex(randomPoint);
                    i++;
                }
            }

            int edgesCount = rnd.Next(verticesCount - 1, Math.Min(3 * verticesCount, (verticesCount * (verticesCount - 1)) / 2)); 
            for (int i = 0; i < edgesCount; i++)
            {
                Vertex startVertex = vertices[rnd.Next(verticesCount)];
                Vertex endVertex;
                do
                {
                    endVertex = vertices[rnd.Next(verticesCount)];
                } while (startVertex == endVertex || startVertex.IsConnectedTo(endVertex)); 

                CreateEdge(startVertex, endVertex);
            }
        }

        private void MainForm_SelectedCommand(object sender, CommandEventArgs e)
        {
            CommandChange(e.Command);
        }


        public void MainForm_UndoCommand(object sender, EventArgs e)
        {
            if (undoHistory.Count <= 0) return;

            List<Vertex> lastState = undoHistory[undoHistory.Count - 1];
            undoHistory.RemoveAt(undoHistory.Count - 1);
            Load(lastState);
        }


        public void UpdateUndoHistory()
        {
            undoHistory.Add(Vertex.CloneVertices(vertices));
            if (undoHistory.Count > maxUndoCount)
                undoHistory.RemoveAt(0);
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

        public void Clear()
        {
            draggedVertex = null;
            lastMouseLocation = Point.Empty;
            undoHistory.Clear();
            vertices.Clear();

            pictureBox.Invalidate();
            
        }

        private void RefreshPaint(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }
    }
}
