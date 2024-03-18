using GraphSplit.UIElements;
using GraphSplit.UIElements.Paint;

namespace GraphSplit
{
    public partial class MainForm : Form
    {
        private ControlButtons controlButtons;
        private Tip tip = new Tip();
        private Navbar navbar;
        private PaintArea paintArea;

        public MainForm()
        {
            InitializeComponent();
            InitializeForm();
            GraphSettings.LoadSettings();

            this.FormClosing += (sender, e) => Application.Exit();
        }

        public void InitializeForm()
        {
            controlButtons = new ControlButtons();
            paintArea = new PaintArea(this);
            navbar = new Navbar(this);

            MinimumSize = new Size(800, 600);
            MaximumSize = new Size(1300, 800);

            Controls.AddRange(new Control[] { controlButtons.Initialize(), paintArea.Initialize(), navbar.Initialize()});

            navbar.paintArea = paintArea;

            tip.Initialize(GetButtons());

            CommandHandler.SelectedCommand(Command.AddVertex);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return CommandHandler.HandleKeyCommand(keyData) || base.ProcessCmdKey(ref msg, keyData);
        }

        public string LastUseName
        {
            set
            {
                string nameForm = "Редактор графов ";
                nameForm += "(";
                nameForm += string.IsNullOrEmpty(value) ? "*" : value;
                nameForm += ")";
                this.Text = nameForm;
            }
        }


        public List<Button> GetButtons() => controlButtons.GetButtons();
        public int Width => ClientSize.Width;
        public int Height => ClientSize.Height;
    }
}
