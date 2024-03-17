using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class Tip
    {
        public Tip() { }

        public ToolTip Initialize(List<Button> buttons)
        {
            ToolTip toolTip = new ToolTip();

            foreach (var button in buttons)
            {
                button.MouseEnter += (sender, e) => ShowToolTip(button, toolTip);
                button.MouseLeave += (sender, e) => HideToolTip(button, toolTip);
            }

            AttachToolTip(buttons[(int)Command.AddVertex], "Добавить вершину (V)");
            AttachToolTip(buttons[(int)Command.AddEdge], "Добавить ребро (E)");
            AttachToolTip(buttons[(int)Command.DeleteElement], "Удалить элемент (D)");
            AttachToolTip(buttons[(int)Command.Moving], "Передвигать вершины (M)");

            return toolTip;
        }

        private void ShowToolTip(Button button, ToolTip toolTip)
        {
            if (button != null)
                toolTip.Show(button.Tag.ToString(), button, button.Width, 0);
        }

        private void HideToolTip(Button button, ToolTip toolTip)
        {
            toolTip.Hide(button);
        }

        private void AttachToolTip(Button button, string tooltipText)
        {
            button.Tag = tooltipText;
        }
    }
}
