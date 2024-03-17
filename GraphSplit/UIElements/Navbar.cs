using GraphSplit.JSON;


namespace GraphSplit.UIElements
{
    public partial class Navbar
    {
        public Navbar(MainForm mainForm) { this.mainForm = mainForm; }

        private const string FileMenuText = "Файл";
        private const string AboutMenuText = "О программе";
        private const string NewFileMenuItemText = "Создать";
        private const string OpenFileMenuItemText = "Открыть";
        private const string SaveFileItemText = "Сохранить";
        private const string SaveAsFileItemText = "Сохранить как";

        private string lastUseName = string.Empty;

        public PaintArea paintArea;
        private MainForm mainForm;

        public MenuStrip Initialize()
        {
            MenuStrip menuStrip = new MenuStrip();

            var openFileMenuItem = new ToolStripMenuItem(OpenFileMenuItemText);
            openFileMenuItem.Click += OpenFileMenuItem_Click;

            var NewFileMenuItem = new ToolStripMenuItem(NewFileMenuItemText);
            NewFileMenuItem.Click += NewFileMenuItem_Click;

            var SaveFileItem = new ToolStripMenuItem(SaveFileItemText);
            SaveFileItem.Click += SaveFileItem_Click;

            var SaveAsFileItem = new ToolStripMenuItem(SaveAsFileItemText);
            SaveAsFileItem.Click += SaveAsFileItem_Click;


            ToolStripMenuItem fileMenu = CreateToolStripMenuItem(FileMenuText,
                                                                  openFileMenuItem,
                                                                  NewFileMenuItem,
                                                                  SaveFileItem,
                                                                  SaveAsFileItem);
            // Создаем меню "О программе"
            ToolStripMenuItem aboutMenu = CreateToolStripMenuItem(AboutMenuText);
            aboutMenu.Click += AboutMenuItem_Click;

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(aboutMenu);


            CommandHandler.SaveCommand += SaveFileItem_Click;
            CommandHandler.SaveAsCommand += SaveAsFileItem_Click;

            return menuStrip;
        }


        private ToolStripMenuItem CreateToolStripMenuItem(string text, params ToolStripMenuItem[] items)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem(text);

            foreach (ToolStripMenuItem item in items)
                menuItem.DropDownItems.Add(item);

            return menuItem;
        }


        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void SaveFileItem_Click(object sender, EventArgs e)
        {
            string fileName = lastUseName;

            if (fileName == string.Empty) 
            {
                fileName = GetNameFile();
                if (fileName == string.Empty) return;
            }

            SetLastUseName(fileName);

            SaveLoadJSON.SaveToJSON(lastUseName, paintArea.GetVertices());
        }

        private void SaveAsFileItem_Click(object sender, EventArgs e)
        {
            string fileName = GetNameFile();

            if (fileName == string.Empty) return;

            SetLastUseName(fileName);

            SaveLoadJSON.SaveToJSON(fileName, paintArea.GetVertices());
        }
        private void NewFileMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Хотите сохранить изменения в текущем файле перед созданием нового?", 
                                         "Сохранение изменений", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                SaveFileItem_Click(sender, e);
            else if (result == DialogResult.Cancel)
                return;

            SetLastUseName(string.Empty);

            paintArea.Clear();
        }


        private void OpenFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                SetLastUseName(filePath);

                paintArea.Clear();
                paintArea.Load(SaveLoadJSON.LoadFromJSON(filePath));
            }
        }

        private string GetNameFile() 
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON Files (*.json)|*.json";
                saveFileDialog.Title = "Save as...";
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.DefaultExt = "json";
                saveFileDialog.OverwritePrompt = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    return saveFileDialog.FileName;
                else
                    return string.Empty;
            }
        }

        private void SetLastUseName(string name) 
        {
            lastUseName = name;
            mainForm.LastUseName = Path.GetFileName(lastUseName); 
        }
    }
}
