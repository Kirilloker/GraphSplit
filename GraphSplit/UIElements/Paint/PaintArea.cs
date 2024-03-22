using GraphSplit.GraphElements;
using System.Windows.Forms;

namespace GraphSplit.UIElements.Paint
{
    public partial class PaintArea
    {
        private const int width = 1000;
        private const int height = 700;

        private PictureBox pictureBox;
        private readonly MainForm mainForm;
        private GraphManager graphManager;
        private MouseEventHandler mouseEventHandler;


        public PaintArea(MainForm mainForm)
        {
            this.mainForm = mainForm;
            CommandHandler.CommandSelected += MainForm_SelectedCommand;
        }

        public PictureBox Initialize()
        {
            pictureBox = new PictureBox
            {
                Size = new Size(width, height),
                Location = new Point((mainForm.Width - width) / 2, (mainForm.Height - height) / 2),
                BackColor = Color.White
            };

            graphManager = new(this);
            mouseEventHandler = new MouseEventHandler(this, graphManager);

            GraphSettings.SettingsChange += RefreshPaint;

            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += mouseEventHandler.OnMouseDown;
            pictureBox.MouseMove += mouseEventHandler.OnMouseMove;
            pictureBox.MouseUp += mouseEventHandler.OnMouseUp;

            return pictureBox;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (mouseEventHandler.IsSelecting)
            {
                using (Pen pen = new Pen(Color.Blue) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    e.Graphics.DrawRectangle(pen, mouseEventHandler.SelectionRectangle);
                }
            }

            foreach (Vertex vertex in graphManager.Vertices)
                foreach (Edge edge in vertex.AdjacentEdgesRender)
                    edge.Draw(e.Graphics);

            foreach (Vertex vertex in graphManager.Vertices)
                vertex.Draw(e.Graphics, e.Graphics, vertex.Index);
        }


        public bool GenerateRandomGraph(int verticesCount, int minDistance, int connectionRadius)
        {
            Clear();
            graphManager.UpdateUndoHistory();
            return graphManager.GenerateRandomGraph(verticesCount, width, height, minDistance, connectionRadius);
        }

        public void Clear()
        {
            graphManager.Clear();
            RefreshPaint();
        }

        public void Load(List<Vertex> vertices)
        {
            graphManager.Load(vertices);
        }

        private void MainForm_SelectedCommand(object sender, CommandEventArgs e) => RefreshPaint();
        public void RefreshPaint() => pictureBox.Invalidate();
        public void RefreshPaint(object sender, EventArgs e) => pictureBox.Invalidate();
        public List<Vertex> GetVertices() => graphManager.Vertices;
        public int Width { get { return width; } }
        public int Height { get { return height; } }
    }
}
