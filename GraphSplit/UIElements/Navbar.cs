using System;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class Navbar
    {
        public Navbar() { }

        private const string FileMenuText = "Файл";
        private const string AboutMenuText = "О программе";
        private const string NewFileMenuItemText = "Создать новый файл";
        private const string OpenFileMenuItemText = "Открыть файл";

        public MenuStrip Initialize()
        {
            MenuStrip menuStrip = new MenuStrip();

            // Создаем меню "Файл"
            ToolStripMenuItem fileMenu = CreateToolStripMenuItem(FileMenuText,
                                                                  new ToolStripMenuItem(OpenFileMenuItemText),
                                                                  new ToolStripMenuItem(NewFileMenuItemText));
            // Создаем меню "О программе"
            ToolStripMenuItem aboutMenu = CreateToolStripMenuItem(AboutMenuText);
            aboutMenu.Click += AboutMenuItem_Click;

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(aboutMenu);

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
    }
}
