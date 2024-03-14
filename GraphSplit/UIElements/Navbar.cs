using System;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class Navbar
    {
        public Navbar() { }

        public MenuStrip Initialize() 
        {
            MenuStrip menuStrip = new MenuStrip();

            ToolStripMenuItem fileMenu = new ("Файл");
            ToolStripMenuItem aboutMenu = new ("О программе");

            ToolStripMenuItem newFileMenuItem = new ("Новый");
            ToolStripMenuItem openFileMenuItem = new ("Открыть");
            fileMenu.DropDownItems.Add(newFileMenuItem);
            fileMenu.DropDownItems.Add(openFileMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(aboutMenu);

            return menuStrip;
        }

    }
}
