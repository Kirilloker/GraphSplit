namespace GraphSplit.UIElements
{
    public partial class ControlButtons
    {
        private Panel toolPanel;
        private readonly List<Button> buttons = new List<Button>();

        public ControlButtons()
        {
            CommandHandler.CommandSelected += MainForm_SelectedCommand;
        }

        public Panel Initialize()
        {
            toolPanel = new Panel
            {
                BackColor = Color.Gray,
                Dock = DockStyle.Left,
                Width = 100
            };

            InitializeButtons();

            foreach (Button button in buttons)
                toolPanel.Controls.Add(button);

            return toolPanel;
        }

        private void InitializeButtons()
        {
            InitializeButton("D:\\Charp\\GraphSplit\\GraphSplit\\bin\\Debug\\net6.0-windows\\addVertexIcon.png", ButtonAddVertex_Click, 0);
            InitializeButton("D:\\Charp\\GraphSplit\\GraphSplit\\bin\\Debug\\net6.0-windows\\addEdgeIcon.png", ButtonAddEdge_Click, 1);
            InitializeButton("D:\\Charp\\GraphSplit\\GraphSplit\\bin\\Debug\\net6.0-windows\\DeleteElementIcon.png", ButtonDeleteElement_Click, 2);
            InitializeButton("D:\\Charp\\GraphSplit\\GraphSplit\\bin\\Debug\\net6.0-windows\\MovingIcon.png", ButtonMoving_Click, 3);
        }

        private void InitializeButton(string imageName, EventHandler clickHandler, int position)
        {
            Button button = new Button
            {
                Size = new Size(80, 80),
                Image = Image.FromFile(imageName),
                TextAlign = ContentAlignment.MiddleCenter,
                TextImageRelation = TextImageRelation.ImageAboveText,
                Location = new Point(10, 10 + position * 90)
        };

            button.Click += clickHandler;
            buttons.Add(button);
        }

        private void ButtonAddVertex_Click(object sender, EventArgs e) => CommandHandler.SelectedCommand(Command.AddVertex);
        private void ButtonAddEdge_Click(object sender, EventArgs e) => CommandHandler.SelectedCommand(Command.AddEdge);
        private void ButtonDeleteElement_Click(object sender, EventArgs e) => CommandHandler.SelectedCommand(Command.DeleteElement);
        private void ButtonMoving_Click(object sender, EventArgs e) => CommandHandler.SelectedCommand(Command.Moving);

        private void MainForm_SelectedCommand(object sender, CommandEventArgs e)
        {
            TurnLight(e.Command);
        }

        public void TurnLight(Command command)
        {
            foreach (Button button in buttons)
            {
                button.BackColor = Color.Gray;
            }
            buttons[(int)command].BackColor = Color.Red;
        }

        public List<Button> GetButtons() => buttons;
    }
}
