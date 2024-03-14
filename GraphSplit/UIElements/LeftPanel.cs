using System;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class LeftPanel
    {
        public LeftPanel() {}

        Panel toolPanel;

        public Panel Initialize() 
        {
            toolPanel = new Panel();
            toolPanel.BackColor = Color.Gray;
            toolPanel.Dock = DockStyle.Left;
            toolPanel.Width = 100;

            return toolPanel;
        }
    }
}
