using System;
using System.Windows.Forms;

namespace GraphSplit.UIElements
{
    public partial class Tip
    {
        public Tip() {}

        public ToolTip Initialize()
        {
            List<Button> buttons = MainForm.GetInstance().GetButtons();
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = false;

            void AttachMouseEnterHandler(Button button, string tooltipText)
            {
                button.MouseEnter += (sender, e) =>
                {
                    if (button != null)
                        toolTip.Show(tooltipText, button, button.Width, 0);
                };
            }

            foreach (var button in buttons)
                button.MouseLeave += (sender, e) =>
                    toolTip.Hide(button);

            AttachMouseEnterHandler(buttons[(int)Command.AddVertex], "Добавить вершину (V)");
            AttachMouseEnterHandler(buttons[(int)Command.AddEdge], "Добавить ребро (E)");
            AttachMouseEnterHandler(buttons[(int)Command.DeleteElement], "Удалить элемент (D)");

            return toolTip;
        }
    }
}
