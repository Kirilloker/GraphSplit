using GraphSplit.UIElements;
using System.Windows.Forms;

namespace GraphSplit
{
    public partial class MainForm : Form
    {
        ControlButtons controlButtons;
        Tip tip;
        Navbar navbar;
        LeftPanel leftPanel;
        PaintArea paintArea;
        private Command command;
        private ToolTip toolTip;
        private List<Vertex> vertices = new List<Vertex>();


        public void Init() 
        {
            InitializePictureBox();

            InitializeButtons();
            InitializeToolTip();
            InitializeMenu();
            InitializeLeftPanel();

            command = Command.None;
            this.MinimumSize = new Size(800, 600);
            this.MaximumSize = new Size(1300, 800);
        }









        private void InitializeLeftPanel()
        {
            leftPanel = new LeftPanel();
            this.Controls.Add(leftPanel.Initialize());
        }

        private void InitializeToolTip()
        {
            tip = new Tip();
            toolTip = tip.Initialize();
        }

        private void InitializeButtons()
        {
            controlButtons = new ControlButtons();

            foreach (var button in controlButtons.Initialize()) 
                this.Controls.Add(button); 
        }

        public void SelectedCommand(Command selectedCommand) 
        {
            command = selectedCommand;
            controlButtons.TurnLight(command);
            paintArea.CommandChange();
        }



        private void InitializeMenu()
        {
            navbar = new Navbar();
            MenuStrip menuStrip = navbar.Initialize();

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void InitializePictureBox()
        {
            paintArea = new PaintArea();
            this.Controls.Add(paintArea.Initialize());
        }




        public List<Button> GetButtons() => controlButtons.getButtons();
        public List<Vertex> GetVertices() => vertices;

        public int Width { get { return this.ClientSize.Width; } }
        public int Height { get { return this.ClientSize.Height; } }

        public Command Command { get { return command; } }



        private Dictionary<Keys, Command> keyCommandMap = new Dictionary<Keys, Command>()
        {
            { Keys.V, Command.AddVertex },
            { Keys.D, Command.DeleteElement },
            { Keys.E, Command.AddEdge },
        };


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyCommandMap.TryGetValue(keyData, out Command command))
            {
                SelectedCommand(command);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }




        private static MainForm instance;
        public static MainForm GetInstance()
        {
            if (instance == null)
                instance = new MainForm();
            return instance;
        }

        private MainForm()
        {
            InitializeComponent();
        }

    }
}