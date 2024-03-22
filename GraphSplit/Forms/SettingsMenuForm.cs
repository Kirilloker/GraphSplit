using Newtonsoft.Json;

namespace GraphSplit
{
    public partial class SettingsMenuForm : Form
    {
        private TrackBar trackBarVertexRadius;
        private TrackBar trackBarVertexStroke;
        private TrackBar trackBarEdgeLineSize;
        private Button btnSaveSettings;
        private Label labelVertexRadius;
        private Label labelVertexStroke;
        private Label labelEdgeLineSize;
        private Label labelVertexRadiusValue;
        private Label labelVertexStrokeValue;
        private Label labelEdgeLineSizeValue;

        public SettingsMenuForm()
        {
            InitializeComponent();
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            SetupFormControls();
            LoadSettings();
        }

        private void SetupFormControls()
        {
            labelVertexRadius = new Label
            {
                Text = "Размер радиуса вершины:",
                Location = new Point(80, 30),
                Size = new Size(200, 20)
            };
            trackBarVertexRadius = new TrackBar
            {
                Minimum = 5,
                Maximum = 60,
                TickFrequency = 5,
                Location = new Point(80, 60),
                Size = new Size(200, 45),
                Value = 10
            };
            labelVertexRadiusValue = new Label
            {
                Text = trackBarVertexRadius.Value.ToString(),
                Location = new Point(280, 60),
                Size = new Size(30, 20)
            };

            trackBarVertexRadius.Scroll += (sender, e) => labelVertexRadiusValue.Text = trackBarVertexRadius.Value.ToString();

            labelVertexStroke = new Label
            {
                Text = "Размер обводки вершины:",
                Location = new Point(80, 130),
                Size = new Size(200, 20)
            };
            trackBarVertexStroke = new TrackBar
            {
                Minimum = 3,
                Maximum = 10,
                TickFrequency = 1,
                Location = new Point(80, 160),
                Size = new Size(200, 45),
                Value = 3
            };
            labelVertexStrokeValue = new Label
            {
                Text = trackBarVertexStroke.Value.ToString(),
                Location = new Point(280, 160),
                Size = new Size(30, 20)
            };

            trackBarVertexStroke.Scroll += (sender, e) => labelVertexStrokeValue.Text = trackBarVertexStroke.Value.ToString();

            labelEdgeLineSize = new Label
            {
                Text = "Размер линии ребра:",
                Location = new Point(80, 230),
                Size = new Size(200, 20)
            };
            trackBarEdgeLineSize = new TrackBar
            {
                Minimum = 2,
                Maximum = 10,
                TickFrequency = 1,
                Location = new Point(80, 260),
                Size = new Size(200, 45),
                Value = 2
            };
            labelEdgeLineSizeValue = new Label
            {
                Text = trackBarEdgeLineSize.Value.ToString(),
                Location = new Point(280, 260),
                Size = new Size(30, 20)
            };

            trackBarEdgeLineSize.Scroll += (sender, e) => labelEdgeLineSizeValue.Text = trackBarEdgeLineSize.Value.ToString();

            btnSaveSettings = new Button
            {
                Text = "Сохранить",
                Location = new Point(82, 350),
                Size = new Size(200, 40)
            };

            btnSaveSettings.Click += btnSaveSettings_Click;

            Controls.Add(labelVertexRadius);
            Controls.Add(trackBarVertexRadius);
            Controls.Add(labelVertexRadiusValue);
            Controls.Add(labelVertexStroke);
            Controls.Add(trackBarVertexStroke);
            Controls.Add(labelVertexStrokeValue);
            Controls.Add(labelEdgeLineSize);
            Controls.Add(trackBarEdgeLineSize);
            Controls.Add(labelEdgeLineSizeValue);
            Controls.Add(btnSaveSettings);

            trackBarVertexRadius.Scroll += (sender, e) => labelVertexRadiusValue.Text = trackBarVertexRadius.Value.ToString();
            trackBarVertexStroke.Scroll += (sender, e) => labelVertexStrokeValue.Text = trackBarVertexStroke.Value.ToString();
            trackBarEdgeLineSize.Scroll += (sender, e) => labelEdgeLineSizeValue.Text = trackBarEdgeLineSize.Value.ToString();

        }

        private void LoadSettings()
        {
            trackBarVertexRadius.Value = GraphSettings.VertexRadius;
            trackBarVertexStroke.Value = GraphSettings.VertexBorder; 
            trackBarEdgeLineSize.Value = GraphSettings.EdgeLineSize;

            labelVertexRadiusValue.Text = trackBarVertexRadius.Value.ToString();
            labelVertexStrokeValue.Text = trackBarVertexStroke.Value.ToString();
            labelEdgeLineSizeValue.Text = trackBarEdgeLineSize.Value.ToString();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            GraphSettings.ChangeEdgeLineSize(trackBarEdgeLineSize.Value);
            GraphSettings.ChangeVertexRadius(trackBarVertexRadius.Value);
            GraphSettings.ChangeVertexBorder(trackBarVertexStroke.Value);

            GraphSettings.SaveSettings();
        }
    }
}
