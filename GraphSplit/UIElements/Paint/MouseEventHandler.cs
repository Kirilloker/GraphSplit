using GraphSplit.GraphElements;

namespace GraphSplit.UIElements.Paint
{
    public class MouseEventHandler
    {
        private PaintArea paintArea;
        private GraphManager graphManager;
        private Point lastMouseLocation;
        private bool isSelecting = false;
        private Rectangle selectionRectangle = Rectangle.Empty;
        private Vertex draggedVertex = null;
        private List<Vertex> selectedVertices = new List<Vertex>();
        private bool test = false; 

        public MouseEventHandler(PaintArea paintArea, GraphManager graphManager)
        {
            this.paintArea = paintArea;
            this.graphManager = graphManager;
            CommandHandler.CommandSelected += MainForm_SelectedCommand;

        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!graphManager.TryRemoveSelectedVertex(e.Location))
                {
                    graphManager.TryRemoveSelectedEdge(e.Location);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                Vertex clickedVertex = graphManager.FindVertexAtPoint(e.Location);
                if (clickedVertex != null)
                {
                    LeftClickOnVertex(clickedVertex);
                }
                else
                {
                    if (CommandHandler.Command == Command.DeleteElement || CommandHandler.Command == Command.Moving)
                    {
                        lastMouseLocation = e.Location;
                        isSelecting = true;
                        selectionRectangle = new Rectangle(e.Location, new Size(0, 0));
                    }

                    if (CommandHandler.Command == Command.AddVertex)
                    {
                        graphManager.CreateVertex(e.Location);
                    }
                }
            }
            paintArea.RefreshPaint();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
                HandleSelectionMouseMove(e.Location);

            if (CommandHandler.Command == Command.Moving && selectedVertices.Count > 0)
                HandleMovingMouseMove(e.Location);

            if (CommandHandler.Command == Command.AddVertex && draggedVertex != null)
                HandleAddVertexMouseMove(e.Location);

            paintArea.RefreshPaint();
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            test = false;

            if (isSelecting)
                HandleSelectionMouseUp();

            if (CommandHandler.Command == Command.Moving && selectedVertices.Count == 0)
                HandleMovingMouseUp();

            if (CommandHandler.Command == Command.AddVertex && draggedVertex != null)
                HandleAddVertexMouseUp();

            paintArea.RefreshPaint();
        }

        private void LeftClickOnVertex(Vertex clickedVertex)
        {
            switch (CommandHandler.Command)
            {
                case Command.AddVertex:
                    HandleAddVertex(clickedVertex);
                    break;
                case Command.DeleteElement:
                    graphManager.RemoveVertex(clickedVertex);
                    break;
                case Command.AddEdge:
                    HandleAddEdge(clickedVertex);
                    break;
            }
        }

        private void HandleSelectionMouseMove(Point location)
        {
            selectionRectangle = new Rectangle(
                Math.Min(lastMouseLocation.X, location.X),
                Math.Min(lastMouseLocation.Y, location.Y),
                Math.Abs(location.X - lastMouseLocation.X),
                Math.Abs(location.Y - lastMouseLocation.Y));
        }

        private void HandleMovingMouseMove(Point location)
        {
            if (!test)
            {
                lastMouseLocation = location;
                test = true;
            }

            var deltaX = location.X - lastMouseLocation.X;
            var deltaY = location.Y - lastMouseLocation.Y;

            foreach (var vertex in selectedVertices)
            {
                vertex.Move(deltaX, deltaY);
            }

            lastMouseLocation = location;
        }

        private void HandleAddVertexMouseMove(Point location)
        {
            var newX = Math.Clamp(location.X, 0, paintArea.Width);
            var newY = Math.Clamp(location.Y, 0, paintArea.Height);
            var deltaX = newX - lastMouseLocation.X;
            var deltaY = newY - lastMouseLocation.Y;

            draggedVertex.Move(deltaX, deltaY);
            lastMouseLocation = new Point(newX, newY);
        }

        private void HandleSelectionMouseUp()
        {
            isSelecting = false;
            if (CommandHandler.Command == Command.DeleteElement)
            {
                graphManager.RemoveVerticesWithin(selectionRectangle);
            }
            else if (CommandHandler.Command == Command.Moving)
            {
                selectedVertices = graphManager.SelectVerticesWithin(selectionRectangle);
            }
            selectionRectangle = Rectangle.Empty;
        }

        private void HandleMovingMouseUp()
        {
            draggedVertex = null;
            selectedVertices.Clear();
            graphManager.SetDefaultColorAllElements();
            graphManager.UpdateUndoHistory();
        }

        private void HandleAddVertexMouseUp()
        {
            draggedVertex.ChangeBorderColor(Color.Blue);

            foreach (var edge in draggedVertex.AdjacentEdgesRender)
                edge.ChangeColorLine(Color.Black);
            
            draggedVertex = null;

            graphManager.UpdateUndoHistory();
        }

        private void HandleAddVertex(Vertex clickedVertex)
        {
            draggedVertex = clickedVertex;
            lastMouseLocation = clickedVertex.Location;
            clickedVertex.ChangeBorderColor(Color.Red);

            foreach (var edge in clickedVertex.AdjacentEdgesRender)
                edge.ChangeColorLine(Color.Red);

            graphManager.UpdateUndoHistory();
        }

        private void HandleAddEdge(Vertex clickedVertex)
        {
            if (draggedVertex == null)
            {
                draggedVertex = clickedVertex;
                clickedVertex.ChangeBorderColor(Color.Yellow);
            }
            else
            {
                draggedVertex.ChangeBorderColor(Color.Blue);
                graphManager.CreateEdge(draggedVertex, clickedVertex);
                draggedVertex = null;
            }
        }
        public void CommandChange(Command command)
        {
            if (draggedVertex != null)
                draggedVertex.ChangeBorderColor(Color.Blue);

            draggedVertex = null;
        }
        private void MainForm_SelectedCommand(object sender, CommandEventArgs e) => CommandChange(e.Command);


        public Rectangle SelectionRectangle { get { return selectionRectangle; } }
        public bool IsSelecting { get { return isSelecting; } }
    }
}
