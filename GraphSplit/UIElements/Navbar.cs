using GraphSplit.Forms;
using GraphSplit.JSON;
using GraphSplit.UIElements.Paint;

namespace GraphSplit.UIElements
{
    public partial class Navbar
    {
        public Navbar(MainForm mainForm) { this.mainForm = mainForm; }

        private const string FileMenuText = "Файл";
        private const string NewFileMenuItemText = "Создать";
        private const string OpenFileMenuItemText = "Открыть";
        private const string SaveFileItemText = "Сохранить";
        private const string SaveAsFileItemText = "Сохранить как";

        private const string SettingsMenuText = "Настройки";

        private const string AboutMenuText = "О программе";

        private const string OptionsMenuText = "Опции";
        private const string Generate10VertexText = "Создать граф с 30 случайными вершинами";
        private const string Generate20VertexText = "Создать граф с 40 случайными вершинами";
        private const string Generate30VertexText = "Создать граф с 50 случайными вершинами";
        private const string DeepSettingsGenerateVertexText = "Глубокая настройка создание графа";

        private const string AlgorithmMenuText = "Алгоритм";


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
            
            ToolStripMenuItem aboutMenu = CreateToolStripMenuItem(AboutMenuText);
            aboutMenu.Click += AboutMenuItem_Click;

            ToolStripMenuItem SettingsMenu = CreateToolStripMenuItem(SettingsMenuText);
            SettingsMenu.Click += SettingsMenu_Click;

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(SettingsMenu);
            menuStrip.Items.Add(aboutMenu);


            CommandHandler.SaveCommand += SaveFileItem_Click;
            CommandHandler.SaveAsCommand += SaveAsFileItem_Click;


            var optionsMenu = new ToolStripMenuItem(OptionsMenuText);

            var randomGraph10 = new ToolStripMenuItem(Generate10VertexText);
            randomGraph10.Click += (sender, e) => paintArea.GenerateRandomGraph(30, 100, 200);

            var randomGraph20 = new ToolStripMenuItem(Generate20VertexText);
            randomGraph20.Click += (sender, e) => paintArea.GenerateRandomGraph(40, 80, 170);

            var randomGraph30 = new ToolStripMenuItem(Generate30VertexText);
            randomGraph30.Click += (sender, e) => paintArea.GenerateRandomGraph(50, 60, 150);

            var deepGenerate = new ToolStripMenuItem(DeepSettingsGenerateVertexText);
            deepGenerate.Click += DeepSettingsGenerate_Click;

            optionsMenu.DropDownItems.Add(randomGraph10);
            optionsMenu.DropDownItems.Add(randomGraph20);
            optionsMenu.DropDownItems.Add(randomGraph30);
            optionsMenu.DropDownItems.Add(deepGenerate);

            menuStrip.Items.Add(optionsMenu);

            ToolStripMenuItem algorithmMenu = CreateToolStripMenuItem(AlgorithmMenuText);
            algorithmMenu.Click += AlgorithmMenuItem_Click;

            menuStrip.Items.Add(algorithmMenu);

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

        private void DeepSettingsGenerate_Click(object sender, EventArgs e)
        {
            DeepSettingGenerate deepForm = new DeepSettingGenerate(paintArea);
            deepForm.Show();
        }

        private void AlgorithmMenuItem_Click(object sender, EventArgs e)
        {
            AlgorithmForm algorithmForm = new AlgorithmForm(paintArea.GetGraphManager());
            algorithmForm.Show();
        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            SettingsMenuForm settingMenuForm = new SettingsMenuForm();
            settingMenuForm.Show();
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
