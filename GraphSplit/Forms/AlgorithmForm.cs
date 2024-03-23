using GraphSplit.UIElements.Paint;

namespace GraphSplit.Forms
{
    public partial class AlgorithmForm : Form
    {
        private NumericUpDown numberOfSubgraphsNumericUpDown;
        private RadioButton showAllStepsRadioButton;
        private RadioButton splitImmediatelyRadioButton;
        private Label stepLabel;
        private Button firstStepButton;
        private Button previousStepButton;
        private Button nextStepButton;
        private Button finaleStepButton;
        private Button applyButton;

        public GraphManager graphManager;

        public AlgorithmForm(GraphManager graphManager)
        {
            InitializeComponent();
            InitializeUI();

            this.graphManager = graphManager;
            GraphManager.UpdateStepLabel += (currentStep, totalSteps) => stepLabel.Text = $"Шаг {currentStep}\\{totalSteps}";
        }


        private void InitializeUI()
        {
            this.Text = "Настройки разбиения";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            Label numberOfSubgraphsLabel = new Label
            {
                Text = "Количество подграфов:",
                Location = new Point(20, 20),
                Size = new Size(200, 20)
            };
            this.Controls.Add(numberOfSubgraphsLabel);

            numberOfSubgraphsNumericUpDown = new NumericUpDown
            {
                Location = new Point(220, 20),
                Size = new Size(140, 5),
                Minimum = 1,
                Maximum = 100,
                Value = 4
            };
            this.Controls.Add(numberOfSubgraphsNumericUpDown);

            showAllStepsRadioButton = new RadioButton
            {
                Text = "Показывать все шаги разбиения",
                Location = new Point(20, 60),
                Size = new Size(220, 20)
            };
            this.Controls.Add(showAllStepsRadioButton);

            splitImmediatelyRadioButton = new RadioButton
            {
                Text = "Разбить сразу",
                Location = new Point(20, 90),
                Size = new Size(140, 25),
                Checked = true             
            };
            this.Controls.Add(splitImmediatelyRadioButton);

            stepLabel = new Label
            {
                Text = "Шаг 0\\0",
                Location = new Point(25, 145),
                Size = new Size(100, 20),
                Visible = false
            };
            this.Controls.Add(stepLabel);

            firstStepButton = new Button
            {
                Text = "<<-",
                Location = new Point(130, 140),
                Size = new Size(50, 30),
                Visible = false
            };
            firstStepButton.Click += FirstStepButton_Click;
            this.Controls.Add(firstStepButton);


            previousStepButton = new Button
            {
                Text = "<-",
                Location = new Point(190, 140),
                Size = new Size(50, 30),
                Visible = false
            };
            previousStepButton.Click += PreviousStepButton_Click;
            this.Controls.Add(previousStepButton);

            nextStepButton = new Button
            {
                Text = "->",
                Location = new Point(260, 140),
                Size = new Size(50, 30),
                Visible = false
            };
            nextStepButton.Click += NextStepButton_Click;
            this.Controls.Add(nextStepButton);

            finaleStepButton = new Button
            {
                Text = "->>",
                Location = new Point(320, 140),
                Size = new Size(50, 30),
                Visible = false
            };
            finaleStepButton.Click += FinaleStepButton_Click;
            this.Controls.Add(finaleStepButton);

            applyButton = new Button
            {
                Text = "Применить",
                Location = new Point(85, 190),
                Size = new Size(215, 30)
            };
            applyButton.Click += ApplyButton_Click;
            this.Controls.Add(applyButton);
        }

        private void ShowAllStepsRadioButton_CheckedChanged()
        {
            bool visible = showAllStepsRadioButton.Checked;
            stepLabel.Visible = visible;
            firstStepButton.Visible = visible;
            previousStepButton.Visible = visible;
            nextStepButton.Visible = visible;
            finaleStepButton.Visible = visible;
        }



        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ShowAllStepsRadioButton_CheckedChanged();
            int numberOfSubgraphs = (int)numberOfSubgraphsNumericUpDown.Value;
            bool showAllSteps = showAllStepsRadioButton.Checked;

            bool success = graphManager.AlgorithmApply(numberOfSubgraphs, showAllSteps);

            if (!success)
                MessageBox.Show("Не удалось разбить на подграфы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void FirstStepButton_Click(object sender, EventArgs e)
        {
            graphManager.ShowStartAlgorithm();
        }

        private void PreviousStepButton_Click(object sender, EventArgs e)
        {
            graphManager.ShowPreviousStep();
        }

        private void NextStepButton_Click(object sender, EventArgs e)
        {
            graphManager.ShowNextStep();
        }

        private void FinaleStepButton_Click(object sender, EventArgs e)
        {
            graphManager.ShowEndAlgorithm();
        }
    }
}
