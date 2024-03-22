using GraphSplit.UIElements.Paint;

namespace GraphSplit.Forms
{
    public partial class DeepSettingGenerate : Form
    {
        private TextBox verticesCountTextBox;
        private TextBox minDistanceTextBox;
        private TextBox maxDistanceTextBox;
        private Button generateButton;

        public PaintArea paintArea;


        public DeepSettingGenerate(PaintArea paintArea)
        {
            this.paintArea = paintArea;
            InitializeComponent();
            InitializeUI();
            SetDefaultValues();
        }

        private void InitializeUI()
        {
            this.Text = "Настройки генерации графа";
            this.Size = new Size(450, 290);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateLabel("Количество вершин:", 20, 20);
            verticesCountTextBox = CreateTextBox(320, 20);

            CreateLabel("Мин. расстояние между вершинами:", 20, 70);
            minDistanceTextBox = CreateTextBox(320, 70);

            CreateLabel("Макс. расстояние между вершинами:", 20, 120);
            maxDistanceTextBox = CreateTextBox(320, 120);

            generateButton = new Button
            {
                Location = new Point(120, 180),
                Size = new Size(200, 30),
                Text = "Сгенерировать граф"
            };
            generateButton.Click += GenerateButton_Click;
            this.Controls.Add(generateButton);
        }

        private void SetDefaultValues()
        {
            verticesCountTextBox.Text = "250";
            minDistanceTextBox.Text = "35";
            maxDistanceTextBox.Text = "70";
        }

        private TextBox CreateTextBox(int x, int y)
        {
            TextBox textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(100, 5)
            };
            this.Controls.Add(textBox);
            return textBox;
        }

        private void CreateLabel(string text, int x, int y)
        {
            Label label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(300, 20)
            };
            this.Controls.Add(label);
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput(out int verticesCount, out int minDistance, out int maxDistance))
            {
                bool result = GenerateGraph(verticesCount, minDistance, maxDistance);
                if (!result)
                {
                    MessageBox.Show("Невозможно сгенерировать граф с такими параметрами", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для всех полей.", "Неверный ввод", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput(out int verticesCount, out int minDistance, out int maxDistance)
        {
            bool isValid = true;
            isValid &= int.TryParse(verticesCountTextBox.Text, out verticesCount) && verticesCount > 0;
            isValid &= int.TryParse(minDistanceTextBox.Text, out minDistance) && minDistance >= 0;
            isValid &= int.TryParse(maxDistanceTextBox.Text, out maxDistance) && maxDistance >= minDistance;
            return isValid;
        }

        private bool GenerateGraph(int verticesCount, int minDistance, int maxDistance)
        {
            return paintArea.GenerateRandomGraph(verticesCount, minDistance, maxDistance);
        }
    }
}
