using GraphSplit.Forms;
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
            GraphSettings.LoadSettings();
            AutoLogIn();

            controlButtons = new ControlButtons();
            paintArea = new PaintArea(this);
            navbar = new Navbar(this);

            InitializeForm();

            this.FormClosing += (sender, e) => Application.Exit();
        }

        public void InitializeForm()
        {
            while (Controls.Count > 0) 
            {
                Controls.RemoveAt(0);
            }

            MinimumSize = new Size(800, 600);
            MaximumSize = new Size(1300, 800);

            if (GraphSplit.Authorization.Authorization.IsAuthorization == false)
            {
                InitAuthorizationElements();
                return;
            }

            Controls.Add(controlButtons.Initialize());
            Controls.Add(navbar.Initialize());
            Controls.Add(paintArea.Initialize());

            navbar.paintArea = paintArea;

            tip.Initialize(GetButtons());

            CommandHandler.SelectedCommand(Command.AddVertex);
        }

        private void InitAuthorizationElements() 
        {
            Label label = new Label();
            label.Text = "Редактор графов";
            label.Font = new Font(label.Font.FontFamily, 25, FontStyle.Regular);
            label.Size = new Size(600, 100);

            label.Location = new Point(470, 200);

            Button button = new Button();
            button.Text = "Авторизоваться";
            button.Size = new Size(300, 50);
            button.Font = new Font(button.Font.FontFamily, 16, FontStyle.Regular);
            button.Location = new Point(490, 400);

            button.Click += Authorization;

            Controls.Add(button);
            Controls.Add(label);
        }

        private void AutoLogIn()
        {
            GraphSplit.Authorization.Authorization.Login(GraphSettings.Login, GraphSettings.Password, true);
        }

        private void Authorization(object sender, EventArgs e)
        {
            AuthorizationForm form = new(this);
            form.ShowDialog();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return CommandHandler.HandleKeyCommand(keyData) || base.ProcessCmdKey(ref msg, keyData);
        }

        public string LastUseName
        {
            set
            {
                string nameForm = "Graph Editor";
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
