namespace GraphSplit.Forms
{
    public partial class AlgorithmForm : Form
    {
        private static int numberOfAnts = 3;
        private static int numberOfPartitions = 2;
        private static double coloringProbability = 0.95;
        private static double movingProbability = 0.95;
        private static int numberOfVerticesForBalance = 20;
        private static int numberOfIterations = 500;


        public static event EventHandler AlgoritmApply;


        private TextBox numberOfAntsTextBox;
        private TextBox numberOfPartitionsTextBox;
        private TextBox coloringProbabilityTextBox;
        private TextBox movingProbabilityTextBox;
        private TextBox numberOfVerticesForBalanceTextBox;
        private TextBox numberOfIterationsTextBox;
        private ComboBox checkBoxComboBox;
        private Button applyButton;

        public AlgorithmForm()
        {
            InitializeComponent();
            InitializeUI();
            SetDefaultValues();
        }

        private void InitializeUI()
        {
            this.Text = "Параметры алгоритма";
            this.Size = new Size(360, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label numberOfAntsLabel = new Label();
            numberOfAntsLabel.Text = "Количество муравьев:";
            numberOfAntsLabel.Location = new Point(20, 20);
            numberOfAntsLabel.Size = new(180, 20);
            this.Controls.Add(numberOfAntsLabel);

            numberOfAntsTextBox = new TextBox();
            numberOfAntsTextBox.Location = new Point(220, 20);
            numberOfAntsTextBox.Size = new Size(100, 20);
            this.Controls.Add(numberOfAntsTextBox);

            Label numberOfPartitionsLabel = new Label();
            numberOfPartitionsLabel.Text = "Количество частей:";
            numberOfPartitionsLabel.Location = new Point(20, 70);
            numberOfPartitionsLabel.Size = new(150, 20);
            this.Controls.Add(numberOfPartitionsLabel);

            numberOfPartitionsTextBox = new TextBox();
            numberOfPartitionsTextBox.Location = new Point(220, 70);
            numberOfPartitionsTextBox.Size = new Size(100, 20);
            this.Controls.Add(numberOfPartitionsTextBox);

            Label coloringProbabilityLabel = new Label();
            coloringProbabilityLabel.Text = "Шанс перекраски:";
            coloringProbabilityLabel.Size = new(150, 20);
            coloringProbabilityLabel.Location = new Point(20, 120);
            this.Controls.Add(coloringProbabilityLabel);

            coloringProbabilityTextBox = new TextBox();
            coloringProbabilityTextBox.Location = new Point(220, 120);
            coloringProbabilityTextBox.Size = new Size(100, 20);
            this.Controls.Add(coloringProbabilityTextBox);

            Label movingProbabilityLabel = new Label();
            movingProbabilityLabel.Text = "Шанс перемещения:";
            movingProbabilityLabel.Location = new Point(20, 170);
            movingProbabilityLabel.Size = new Size(180, 20);
            this.Controls.Add(movingProbabilityLabel);

            movingProbabilityTextBox = new TextBox();
            movingProbabilityTextBox.Location = new Point(220, 170);
            movingProbabilityTextBox.Size = new Size(100, 20);
            this.Controls.Add(movingProbabilityTextBox);

            Label numberOfVerticesForBalanceLabel = new Label();
            numberOfVerticesForBalanceLabel.Text = "Кол-во вершин-баланса:";
            numberOfVerticesForBalanceLabel.Size = new Size(184, 20);
            numberOfVerticesForBalanceLabel.Location = new Point(20, 220);
            this.Controls.Add(numberOfVerticesForBalanceLabel);

            numberOfVerticesForBalanceTextBox = new TextBox();
            numberOfVerticesForBalanceTextBox.Location = new Point(220, 220);
            numberOfVerticesForBalanceTextBox.Size = new Size(100, 20);
            this.Controls.Add(numberOfVerticesForBalanceTextBox);

            Label numberOfIterationsLabel = new Label();
            numberOfIterationsLabel.Text = "Количество итераций:";
            numberOfIterationsLabel.Location = new Point(20, 270);
            numberOfIterationsLabel.Size = new Size(180, 20);
            this.Controls.Add(numberOfIterationsLabel);

            numberOfIterationsTextBox = new TextBox();
            numberOfIterationsTextBox.Location = new Point(220, 270);
            numberOfIterationsTextBox.Size = new Size(100, 20);
            this.Controls.Add(numberOfIterationsTextBox);


            checkBoxComboBox = new ComboBox();
            checkBoxComboBox.Location = new Point(20, 350);
            checkBoxComboBox.Size = new Size(300, 20);
            checkBoxComboBox.DropDownStyle = ComboBoxStyle.DropDownList; 
            checkBoxComboBox.Items.AddRange(new object[] { "Без учета расстояния", "С учетом расстояния", "С учетом нормализованного расстояния" });
            this.Controls.Add(checkBoxComboBox);


            applyButton = new Button();
            applyButton.Location = new Point(70, 400);
            applyButton.Size = new Size(200, 30);
            applyButton.Text = "Применить";
            applyButton.Click += ApplyButton_Click;
            this.Controls.Add(applyButton);
        }

        private void SetDefaultValues()
        {
            numberOfAntsTextBox.Text = numberOfAnts.ToString();
            numberOfPartitionsTextBox.Text = numberOfPartitions.ToString();
            coloringProbabilityTextBox.Text = coloringProbability.ToString();
            movingProbabilityTextBox.Text = movingProbability.ToString();
            numberOfVerticesForBalanceTextBox.Text = numberOfVerticesForBalance.ToString();
            numberOfIterationsTextBox.Text = numberOfIterations.ToString();
            checkBoxComboBox.Text = "Без учета расстояния";
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                numberOfAnts = int.Parse(numberOfAntsTextBox.Text);
                numberOfPartitions = int.Parse(numberOfPartitionsTextBox.Text);
                coloringProbability = double.Parse(coloringProbabilityTextBox.Text);
                movingProbability = double.Parse(movingProbabilityTextBox.Text);
                numberOfVerticesForBalance = int.Parse(numberOfVerticesForBalanceTextBox.Text);
                numberOfIterations = int.Parse(numberOfIterationsTextBox.Text);


                AlgoritmApply?.Invoke(null, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Please enter valid input for all fields.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;
            int intValue;
            double doubleValue;

            isValid &= int.TryParse(numberOfAntsTextBox.Text, out intValue) && intValue >= 1 && intValue <= 1000;
            isValid &= int.TryParse(numberOfPartitionsTextBox.Text, out intValue) && intValue >= 1 && intValue <= 1000;
            isValid &= double.TryParse(coloringProbabilityTextBox.Text, out doubleValue) && doubleValue >= 0.01 && doubleValue <= 1;
            isValid &= double.TryParse(movingProbabilityTextBox.Text, out doubleValue) && doubleValue >= 0.01 && doubleValue <= 1;
            isValid &= int.TryParse(numberOfVerticesForBalanceTextBox.Text, out intValue) && intValue >= 0 && intValue <= 1000;
            isValid &= int.TryParse(numberOfIterationsTextBox.Text, out intValue) && intValue >= 0 && intValue <= 1000;

            return isValid;
        }

    }
}
