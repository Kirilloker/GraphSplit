using GraphSplit.UIElements;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GraphSplit
{
    public partial class MainForm : Form
    {
        private ControlButtons controlButtons;
        private Tip tip = new Tip();
        private Navbar navbar;
        private PaintArea paintArea;

        private Command command = Command.None;

        public event EventHandler<CommandEventArgs> EventSelectedCommand;

        private readonly Dictionary<Keys, Command> keyCommandMap = new()
        {
            [Keys.V] = Command.AddVertex,
            [Keys.D] = Command.DeleteElement,
            [Keys.E] = Command.AddEdge
        };

        private MainForm()
        {
            InitializeComponent();
            InitializeForm();

            this.FormClosing += (sender, e) => Application.Exit();
        }

        public void InitializeForm()
        {
            controlButtons = new ControlButtons(this);
            paintArea = new PaintArea(this);
            navbar = new Navbar(this);

            MinimumSize = new Size(800, 600);
            MaximumSize = new Size(1300, 800);

            Controls.AddRange(new Control[] { controlButtons.Initialize(), paintArea.Initialize(), navbar.Initialize()});

            navbar.paintArea = paintArea;

            tip.Initialize(GetButtons());

            SelectedCommand(Command.AddVertex);
        }

        public static MainForm GetInstance() => instance ??= new MainForm();
        private static MainForm instance;


        public event EventHandler UndoCommand;
        public event EventHandler SaveCommand;
        public event EventHandler SaveAsCommand;

        protected virtual void OnUndoCommand()
        {
            UndoCommand?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSaveCommand()
        {
            SaveCommand?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSaveAsCommand()
        {
            SaveAsCommand?.Invoke(this, EventArgs.Empty);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                OnSaveAsCommand();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Z))
            {
                OnUndoCommand();
                return true;
            }

            if (keyData == (Keys.Control | Keys.S)) 
            {
                OnSaveCommand();
                return true;
            }

            if (keyCommandMap.TryGetValue(keyData, out Command cmd))
            {
                SelectedCommand(cmd);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SelectedCommand(Command selectedCommand)
        {
            command = selectedCommand;
            EventSelectedCommand?.Invoke(this, new CommandEventArgs(selectedCommand));
        }

        private string lastUseName = string.Empty;

        public string LastUseName
        {
            get => lastUseName;
            set
            {
                lastUseName = value;
                string nameForm = "Редактор графов ";
                nameForm += "(";
                nameForm += string.IsNullOrEmpty(lastUseName) ? "*" : lastUseName;
                nameForm += ")";
                this.Text = nameForm;
            }
        }

        public List<Button> GetButtons() => controlButtons.GetButtons();
        public int Width => ClientSize.Width;
        public int Height => ClientSize.Height;
        public Command Command => command;
    }
}
