using GraphSplit.GraphElements;

namespace GraphSplit.UIElements.Paint
{
    public class GraphUndo
    {
        private readonly List<List<Vertex>> undoHistory = new List<List<Vertex>>();
        private const int maxUndoCount = 30;

        GraphManager graphManager;
        public GraphUndo(GraphManager graphManager) 
        { 
            this.graphManager = graphManager;
            CommandHandler.UndoCommand += MainForm_UndoCommand;

        }


        public void MainForm_UndoCommand(object sender, EventArgs e)
        {
            if (undoHistory.Count <= 0) return;

            // Баг после алгоритма, нужно дважды отменять действие
            List<Vertex> lastState = undoHistory[undoHistory.Count - 1];
            undoHistory.RemoveAt(undoHistory.Count - 1);
            graphManager.Load(lastState);
        }


        public void UpdateUndoHistory(List<Vertex> vertices)
        {
            undoHistory.Add(Vertex.CloneVertices(vertices));
            if (undoHistory.Count > maxUndoCount)
                undoHistory.RemoveAt(0);
        }

        public void Clear()
        {
            undoHistory.Clear();
        }
    }
}
