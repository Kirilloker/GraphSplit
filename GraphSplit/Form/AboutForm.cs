namespace GraphSplit
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitializeForm(); 
        }

        private void InitializeForm()
        {
            this.Size = new Size(500, 500);

            Label label = new Label();
            label.Text = "Это программа для рисовании графов";
            label.AutoSize = true; 
            label.Location = new Point(10, 10);
            this.Controls.Add(label);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }
    }
}
