using System;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class ControlButtons
    {
        Panel toolPanel;
        private List<Button> buttons;
        private Button but_addVertex = new();
        private Button but_addEdge = new();
        private Button but_deleteElement = new();


        private MainForm mainForm;

        public ControlButtons(MainForm mainForm) 
        {
            this.mainForm = mainForm;
            this.mainForm.EventSelectedCommand += MainForm_SelectedCommand;
        }


        public Panel Initialize() 
        {
            toolPanel = new Panel();
            toolPanel.BackColor = Color.Gray;
            toolPanel.Dock = DockStyle.Left;
            toolPanel.Width = 100;



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
            }

            buttons[(int)Command.AddVertex].Image = Image.FromFile($"addVertexIcon.png");
            buttons[(int)Command.AddVertex].Click += ButtonAddVertex_Click;

            buttons[(int)Command.AddEdge].Image = Image.FromFile($"addEdgeIcon.png");
            buttons[(int)Command.AddEdge].Click += ButtonAddEdge_Click;

            buttons[(int)Command.DeleteElement].Image = Image.FromFile($"DeleteElementIcon.png");
            buttons[(int)Command.DeleteElement].Click += ButtonDeleteElement_Click;


            foreach (var button in buttons)
                toolPanel.Controls.Add(button);



            return toolPanel;
        }

        private void ButtonAddVertex_Click(object sender, EventArgs e) => mainForm.SelectedCommand(Command.AddVertex);
        private void ButtonAddEdge_Click(object sender, EventArgs e) => mainForm.SelectedCommand(Command.AddEdge);
        private void ButtonDeleteElement_Click(object sender, EventArgs e) => mainForm.SelectedCommand(Command.DeleteElement);

        private void MainForm_SelectedCommand(object sender, CommandEventArgs e)
        {
            TurnLight(e.Command);
        }

        public void TurnLight(Command command) 
        {
            foreach (var button in buttons)
                button.BackColor = Color.Gray;

            buttons[(int)command].BackColor = Color.Red;
        }

        public List<Button> GetButtons() => buttons;
    }
}
