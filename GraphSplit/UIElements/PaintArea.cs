using System;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class PaintArea
    {
        PictureBox pictureBox;
        MainForm mainForm;
        private Vertex draggedVertex = null;
        private Edge selectedEdge = null;

        private Point lastMouseLocation;


        public PaintArea() { mainForm = MainForm.GetInstance(); }


        public PictureBox Initialize()
        {
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(600, 400);
            pictureBox.Location = new Point((mainForm.Width - pictureBox.Width) / 2, (mainForm.Height - pictureBox.Height) / 2);
            pictureBox.BackColor = Color.White;
            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            return pictureBox;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            List<Vertex> vertices = mainForm.GetVertices();

            foreach (Vertex vertex in vertices)
                vertex.Draw(e.Graphics, e.Graphics, vertex.Index + 1);

            foreach (Vertex vertex in vertices)
                foreach (Edge edge in vertex.AdjacentEdges)
                    edge.Draw(e.Graphics);
        }



        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            List<Vertex> vertices = mainForm.GetVertices();

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex vertex = vertices[i];

                if (vertex.IsInside(e.Location))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        RemoveVertex(i);
                        return;
                    }

                    if (e.Button == MouseButtons.Left)
                    {
                        if (mainForm.Command == Command.AddVertex)
                        {
                            draggedVertex = vertex;
                            lastMouseLocation = e.Location;
                            return;
                        }
                        else if (mainForm.Command == Command.DeleteElement)
                        {
                            RemoveVertex(i);
                            return;
                        }
                        else if (mainForm.Command == Command.AddEdge)
                        {
                            if (draggedVertex == null)
                            {
                                draggedVertex = vertex;
                            }
                            else
                            {
                                CreateEdge(draggedVertex, vertex);
                                draggedVertex = null;
                            }

                            return;
                        }
                    }
                }
            }


            if (e.Button == MouseButtons.Left && mainForm.Command == Command.AddVertex)
                CreateVertex(e.Location);

            if ((e.Button == MouseButtons.Left && mainForm.Command == Command.DeleteElement) || e.Button == MouseButtons.Right)
            {
                selectedEdge = GetEdgeAtPoint(e.Location);
                if (selectedEdge != null)
                {
                    RemoveEdge(selectedEdge);
                    selectedEdge = null;
                }
            }
        }





        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mainForm.Command == Command.AddVertex)
            {
                if (draggedVertex != null)
                {
                    int deltaX = e.Location.X - lastMouseLocation.X;
                    int deltaY = e.Location.Y - lastMouseLocation.Y;

                    draggedVertex.Move(deltaX, deltaY);
                    lastMouseLocation = e.Location;
                    pictureBox.Invalidate();
                }
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (mainForm.Command == Command.AddVertex)
            {
                if (draggedVertex != null)
                    draggedVertex = null;
            }
        }



        private void CreateEdge(Vertex startVertex, Vertex endVertex)
        {
            Edge newEdge = new Edge(startVertex, endVertex);
            startVertex.AdjacentEdges.Add(newEdge);
            endVertex.AdjacentEdges.Add(newEdge);

            pictureBox.Invalidate();
        }

        private void CreateVertex(Point location)
        {
            List<Vertex> vertices = mainForm.GetVertices();

            Vertex newVertex = new Vertex(location, vertices.Count);
            vertices.Add(newVertex);

            pictureBox.Invalidate();
        }

        private void RemoveVertex(int index)
        {
            List<Vertex> vertices = mainForm.GetVertices();

            Vertex removedVertex = vertices[index];
            vertices.RemoveAt(index);

            foreach (Vertex vertex in vertices)
            {
                vertex.AdjacentEdges.RemoveAll(edge => edge.ConnectedVertices.Contains(removedVertex));
                vertex.Index = vertices.IndexOf(vertex);
            }

            pictureBox.Invalidate();
        }

        private void RemoveEdge(Edge edge)
        {
            edge.ConnectedVertices[0].AdjacentEdges.Remove(edge);
            edge.ConnectedVertices[1].AdjacentEdges.Remove(edge);

            pictureBox.Invalidate();
        }

        private Edge GetEdgeAtPoint(Point point)
        {
            List<Vertex> vertices = mainForm.GetVertices();

            foreach (Vertex vertex in vertices)
                foreach (Edge edge in vertex.AdjacentEdges)
                    if (edge.IsInside(point))
                        return edge;

            return null;
        }


        public void CommandChange() 
        {
            draggedVertex = null;
        }
    }
}
