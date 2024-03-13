namespace GraphSplit
{
    public partial class MainForm : Form
    {
        private Panel toolPanel;
        private Button but_addVertex = new();
        private Button but_addEdge = new();
        private Command command;
        private ToolTip toolTip;
        private List<Button> buttons;



        private Dictionary<Keys, Command> keyCommandMap = new Dictionary<Keys, Command>()
        {
            { Keys.G, Command.AddVertex },
            { Keys.D, Command.AddEdge }
        };



        public MainForm()
        {
            InitializeComponent();

            InitializeLeftPanel();
            InitializeButtons();
            InitializeToolTip();

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


            AttachMouseEnterHandler(buttons[(int)Command.AddVertex], "Добавить вершину");
            AttachMouseEnterHandler(buttons[(int)Command.AddEdge], "Добавить ребро");
        }


        private void InitializeButtons()
        {
            buttons = new List<Button>
            {
                but_addVertex,
                but_addEdge
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

        }

        private void ButtonAddVertex_Click(object sender, EventArgs e) => SelectedCommand(Command.AddVertex);
        private void ButtonAddEdge_Click(object sender, EventArgs e) => SelectedCommand(Command.AddEdge);
        

        private void SelectedCommand(Command selected_command)
        {
            if (command != Command.None)
                buttons[(int)command].BackColor = Color.Gray;

            buttons[(int)selected_command].BackColor = Color.Red;

            command = selected_command;
        }
    }

    public enum Command 
    {
        AddVertex,
        AddEdge,
        None
    }
}