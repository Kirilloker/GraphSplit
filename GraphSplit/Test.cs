using GraphSplit.GraphElements;


namespace GraphSplit.UIElements
{
    public partial class Test1
    {
        private PictureBox pictureBox;
        private readonly MainForm mainForm;
        private readonly List<Vertex> vertices = new List<Vertex>();
        private readonly Stack<Action> undoStack = new Stack<Action>();
        private Vertex draggedVertex = null;
        private Point lastMouseLocation;
        private Point initialDragLocation;
        
        public Test1(MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.mainForm.EventSelectedCommand += MainForm_SelectedCommand;
            this.mainForm.UndoCommand += MainForm_UndoCommand;
        }

        private void MainForm_SelectedCommand(object sender, CommandEventArgs e)
        {
            CommandChange(e.Command);
        }

        public void CommandChange(Command command)
        {
            if (draggedVertex != null)
                draggedVertex.ChangeBorderColor(Color.Blue);

            draggedVertex = null;

            pictureBox.Invalidate();
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

        private void MainForm_UndoCommand(object sender, EventArgs e)
        {
            if (undoStack.Any())
            {
                undoStack.Pop().Invoke();
                pictureBox.Invalidate();
            }
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


                    // индекс в массиве?
                    Action undoAction = () =>
                        AddVertex(clickedVertex.Location,
                                    clickedVertex.Index,
                                    clickedVertex.AdjacentEdgesRender.Select(edge => (edge.Vertex1 == clickedVertex ? edge.Vertex2 : edge.Vertex1).Index).ToList()
                                    );

                    RemoveVertex(clickedVertex);
                    undoStack.Push(undoAction);
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
                Vertex currentDraggedVertex = draggedVertex;
                Vertex currentClickedVertex = clickedVertex;

                currentDraggedVertex.ChangeBorderColor(Color.Blue);
                CreateEdge(currentDraggedVertex, currentClickedVertex);
                Action undoAction = () => {
                    RemoveEdge(currentDraggedVertex.AdjacentEdgesRender.LastOrDefault());
                    currentDraggedVertex.ChangeBorderColor(Color.Blue);
                    currentClickedVertex.ChangeBorderColor(Color.Blue);
                };
                undoStack.Push(undoAction);
                draggedVertex = null;
            }
        }


        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Vertex clickedVertex = FindVertexAtPoint(e.Location);
            //Vertex currentClickedVertex = clickedVertex;
            initialDragLocation = e.Location;

            if (clickedVertex != null)
            {
                // Запоминаем исходное местоположение вершины перед началом перетаскивания
                Point initialLocationBeforeDrag = clickedVertex.Location;

                if (e.Button == MouseButtons.Right)
                {
                    Action undoAction = () => AddVertex(clickedVertex.Location, clickedVertex.Index, clickedVertex.AdjacentEdgesRender.Select(edge => (edge.Vertex1 == clickedVertex ? edge.Vertex2 : edge.Vertex1).Index).ToList());
                    RemoveVertex(clickedVertex);
                    undoStack.Push(undoAction);
                    return;
                }

                HandleLeftButtonClick(clickedVertex);

                // Сохраняем исходное местоположение для возможной отмены
                if (e.Button == MouseButtons.Left)
                {
                    draggedVertex = clickedVertex;
                    Action undoAction = () => clickedVertex.Move(initialLocationBeforeDrag.X - clickedVertex.Location.X, initialLocationBeforeDrag.Y - clickedVertex.Location.Y);
                    undoStack.Push(undoAction);
                }
                return;
            }

            if (e.Button == MouseButtons.Left && mainForm.Command == Command.AddVertex)
            {
                Vertex newVertex = CreateVertex(e.Location);
                Action undoAction = () => RemoveVertex(newVertex);
                undoStack.Push(undoAction);
                return;
            }

            if ((e.Button == MouseButtons.Left && mainForm.Command == Command.DeleteElement) || e.Button == MouseButtons.Right)
            {
                Edge selectedEdge = GetEdgeAtPoint(e.Location);
                Edge currentSelectedEdge = selectedEdge;
                if (currentSelectedEdge != null)
                {
                    Action undoAction = () => CreateEdge(currentSelectedEdge.Vertex1, currentSelectedEdge.Vertex2);
                    RemoveEdge(currentSelectedEdge);
                    undoStack.Push(undoAction);
                }
                return;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((mainForm.Command == Command.AddVertex || mainForm.Command == Command.None) && draggedVertex != null)
            {
                int newX = e.Location.X;
                int newY = e.Location.Y;

                if (newX < 0) newX = 0;
                else if (newX > pictureBox.Width) newX = pictureBox.Width;

                if (newY < 0) newY = 0;
                else if (newY > pictureBox.Height) newY = pictureBox.Height;

                int deltaX = newX - lastMouseLocation.X;
                int deltaY = newY - lastMouseLocation.Y;

                Vertex currentDraggedVertex = draggedVertex;
                currentDraggedVertex.Move(deltaX, deltaY);
                lastMouseLocation = new Point(newX, newY);
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if ((mainForm.Command == Command.AddVertex || mainForm.Command == Command.None) && draggedVertex != null)
            {
                //Point finalLocation = draggedVertex.Location;
                //Vertex currentDraggedVertex = draggedVertex;
                //Action undoAction = () => currentDraggedVertex.Move(initialDragLocation.X - finalLocation.X, initialDragLocation.Y - finalLocation.Y);
                //undoStack.Push(undoAction);

                draggedVertex.ChangeBorderColor(Color.Blue);
                draggedVertex = null;
                pictureBox.Invalidate();
            }
        }

        private Vertex CreateVertex(Point location)
        {
            Vertex newVertex = new Vertex(location, vertices.Count);
            vertices.Add(newVertex);
            pictureBox.Invalidate();
            return newVertex;
        }

        private void AddVertex(Point location, int index, List<int> connectedVertices)
        {
            Vertex newVertex = new Vertex(location, index);
            foreach (int connectedVertexIndex in connectedVertices)
            {
                Vertex connectedVertex = vertices.FirstOrDefault(v => v.Index == connectedVertexIndex);
                if (connectedVertex != null)
                {
                    CreateEdge(newVertex, connectedVertex);
                }
            }
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

        private void CreateEdge(Vertex startVertex, Vertex endVertex)
        {
            Edge newEdge = new Edge(startVertex, endVertex);
            startVertex.AddEdge(newEdge);
            endVertex.AddEdge(newEdge);
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
    }
}