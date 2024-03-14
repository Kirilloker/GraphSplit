using System.Runtime.InteropServices;

namespace GraphSplit
{
    public partial class MainForm : Form
    {
        private Panel toolPanel;
        private Button but_addVertex = new();
        private Button but_addEdge = new();
        private Button but_deleteElement= new();
        private Command command;
        private ToolTip toolTip;
        private List<Button> buttons;

        private List<Vertex> vertices = new List<Vertex>(); 
        private Vertex draggedVertex = null; 
        private Point lastMouseLocation; 


        private Dictionary<Keys, Command> keyCommandMap = new Dictionary<Keys, Command>()
        {
            { Keys.V, Command.AddVertex },
            { Keys.D, Command.DeleteElement },
            { Keys.E, Command.AddEdge },
        };

        public MainForm()
        {
            InitializeComponent();

            InitializePictureBox();
            InitializeLeftPanel();
            InitializeButtons();
            InitializeToolTip();
            InitializeMenu();


            command = Command.None;
            this.MinimumSize = new Size(800, 600);
            this.MaximumSize = new Size(1300, 800);
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyCommandMap.TryGetValue(keyData, out Command command))
            {
                SelectedCommand(command);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void InitializeLeftPanel()
        {
            toolPanel = new Panel();
            toolPanel.BackColor = Color.Gray;
            toolPanel.Dock = DockStyle.Left;
            toolPanel.Width = 100; 
            this.Controls.Add(toolPanel);
        }

        private void InitializeToolTip()
        {
            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = false;

            void AttachMouseEnterHandler(Button button, string tooltipText)
            {
                button.MouseEnter += (sender, e) =>
                {
                    if (button != null)
                        toolTip.Show(tooltipText, button, button.Width, 0);
                };
            }

            foreach (var button in buttons)
                button.MouseLeave += (sender, e) =>
                    toolTip.Hide(button);


            AttachMouseEnterHandler(buttons[(int)Command.AddVertex], "Добавить вершину (V)");
            AttachMouseEnterHandler(buttons[(int)Command.AddEdge], "Добавить ребро (E)");
            AttachMouseEnterHandler(buttons[(int)Command.DeleteElement], "Удалить элемент (D)");
        }


        private void InitializeButtons()
        {
            buttons = new List<Button>
            {
                but_addVertex,
                but_addEdge,
                but_deleteElement
            };

            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];
                button.Size = new Size(80, 80);
                button.Location = new Point(10, 10 + i * 90);
                button.ImageAlign = ContentAlignment.MiddleCenter;
                button.TextImageRelation = TextImageRelation.ImageAboveText;
                toolPanel.Controls.Add(button);
            }

            buttons[(int)Command.AddVertex].Image = Image.FromFile($"addVertexIcon.png");
            buttons[(int)Command.AddVertex].Click += ButtonAddVertex_Click;
            
            buttons[(int)Command.AddEdge].Image = Image.FromFile($"addEdgeIcon.png");
            buttons[(int)Command.AddEdge].Click += ButtonAddEdge_Click;

            buttons[(int)Command.DeleteElement].Image = Image.FromFile($"DeleteElementIcon.png");
            buttons[(int)Command.DeleteElement].Click += ButtonDeleteElement_Click;

        }

        private void ButtonAddVertex_Click(object sender, EventArgs e) => SelectedCommand(Command.AddVertex);
        private void ButtonAddEdge_Click(object sender, EventArgs e) => SelectedCommand(Command.AddEdge);
        private void ButtonDeleteElement_Click(object sender, EventArgs e) => SelectedCommand(Command.DeleteElement);
        

        private void SelectedCommand(Command selected_command)
        {
            if (command != Command.None)
                buttons[(int)command].BackColor = Color.Gray;

            buttons[(int)selected_command].BackColor = Color.Red;

            command = selected_command;
        }


        private void InitializeMenu()
        {
            MenuStrip menuStrip = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            ToolStripMenuItem aboutMenu = new ToolStripMenuItem("О программе");

            ToolStripMenuItem newFileMenuItem = new ToolStripMenuItem("Новый");
            ToolStripMenuItem openFileMenuItem = new ToolStripMenuItem("Открыть");
            fileMenu.DropDownItems.Add(newFileMenuItem);
            fileMenu.DropDownItems.Add(openFileMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(aboutMenu);

            this.MainMenuStrip = menuStrip;

            this.Controls.Add(menuStrip);
        }



        private void InitializePictureBox()
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(600, 400); 
            pictureBox.Location = new Point((this.ClientSize.Width - pictureBox.Width) / 2, (this.ClientSize.Height - pictureBox.Height) / 2); // Размещаем по центру формы
            pictureBox.BackColor = Color.White;
            pictureBox.Paint += PictureBox_Paint; 
            pictureBox.MouseDown += PictureBox_MouseDown; 
            pictureBox.MouseMove += PictureBox_MouseMove; 
            pictureBox.MouseUp += PictureBox_MouseUp; 
            this.Controls.Add(pictureBox); 
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < vertices.Count; i++)
                vertices[i].Draw(e.Graphics, i + 1);

            foreach (Vertex vertex in vertices)
                foreach (Vertex adjacentVertex in vertex.AdjacentVertices)
                    e.Graphics.DrawLine(new Pen(Color.Black, 3), vertex.Location, adjacentVertex.Location);
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex vertex = vertices[i];
                if (vertex.IsInside(e.Location) && e.Button == MouseButtons.Left)
                {
                    // Если пользователь зажал ЛКМ, то начинаем перетаскивание вершины
                    if (command == Command.AddVertex)
                    { 
                        draggedVertex = vertex;
                        lastMouseLocation = e.Location;
                        return;
                    }
                    // Если пользователь зажал ПКМ, то удаляем вершину
                    else if (command == Command.DeleteElement)
                    {
                        RemoveVertex(i);
                        return;
                    }
                    else if (command == Command.AddEdge)
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

            // Если нажатие было на пустом месте, то создаем новую вершину
            if (e.Button == MouseButtons.Left && command == Command.AddVertex)
                CreateVertex(e.Location);
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {

            if (command == Command.AddVertex)
            {
                if (draggedVertex != null)
                {
                    int deltaX = e.Location.X - lastMouseLocation.X;
                    int deltaY = e.Location.Y - lastMouseLocation.Y;

                    draggedVertex.Move(deltaX, deltaY);
                    lastMouseLocation = e.Location;
                    ((PictureBox)sender).Invalidate();
                }

            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (command == Command.AddVertex) 
            {
                if (draggedVertex != null)
                    draggedVertex = null;
            }
        }

        private void CreateVertex(Point location)
        {
            Vertex newVertex = new Vertex(location);
            vertices.Add(newVertex);

            ((PictureBox)this.Controls[0]).Invalidate();
        }

        private void RemoveVertex(int index)
        {
            Vertex removedVertex = vertices[index];
            vertices.RemoveAt(index);

            foreach (Vertex vertex in vertices)
                vertex.AdjacentVertices.Remove(removedVertex);

            ((PictureBox)this.Controls[0]).Invalidate();
        }

        private void CreateEdge(Vertex startVertex, Vertex endVertex)
        {
            if (!startVertex.AdjacentVertices.Contains(endVertex) 
                && 
                !endVertex.AdjacentVertices.Contains(startVertex)
                )
                startVertex.AdjacentVertices.Add(endVertex);

            ((PictureBox)this.Controls[0]).Invalidate();
        }
    }

    // Класс для представления вершины графа
    public class Vertex
    {
        public Point Location { get; set; } 
        public List<Vertex> AdjacentVertices { get; set; } 
        private const int radius = 20; 

        public Vertex(Point location)
        {
            Location = location;
            AdjacentVertices = new List<Vertex>();
        }

        public void Draw(Graphics graphics, int index)
        {
            graphics.FillEllipse(Brushes.Red, Location.X - radius, Location.Y - radius, 2 * radius, 2 * radius);
            graphics.DrawString(index.ToString(), SystemFonts.DefaultFont, Brushes.Black, Location.X - radius / 2, Location.Y - radius / 2);
        }

        public bool IsInside(Point point)
        {
            double distanceSquared = Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2);
            return distanceSquared <= Math.Pow(radius, 2);
        }
        public void Move(int deltaX, int deltaY)
        {
            Location = new Point(Location.X + deltaX, Location.Y + deltaY);
        }
    }

    public enum Command
    {
        AddVertex,
        AddEdge,
        DeleteElement,
        None
    }
}
