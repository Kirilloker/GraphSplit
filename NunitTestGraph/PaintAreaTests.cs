using GraphSplit.GraphElements;
using GraphSplit.UIElements;
using GraphSplit;
using System.Drawing;

[TestFixture]
public class PaintAreaTests
{
    private MainForm mainForm;
    private PaintArea paintArea;

    [SetUp]
    public void Setup()
    {
        mainForm = new MainForm();
        paintArea = new PaintArea(mainForm);
        paintArea.Initialize();
    }

    [Test]
    public void Initialize_CreatesPictureBoxWithExpectedProperties()
    {
        var pictureBox = paintArea.Initialize();
        Assert.AreEqual(new Size(600, 400), pictureBox.Size);
        Assert.AreEqual(Color.White, pictureBox.BackColor);
    }

    [Test]
    public void CreateVertex_AddsVertexToList()
    {
        paintArea.CreateVertex(new Point(100, 100));
        Assert.AreEqual(1, paintArea.GetVertices().Count);
    }

    [Test]
    public void RemoveVertex_RemovesVertexFromList()
    {
        paintArea.CreateVertex(new Point(100, 100));
        var vertex = paintArea.GetVertices().First();
        paintArea.RemoveVertex(vertex);
        Assert.AreEqual(0, paintArea.GetVertices().Count);
    }

    [Test]
    public void CreateEdge_AddsEdgeToVertices()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.CreateVertex(new Point(200, 200));
        var vertices = paintArea.GetVertices();
        paintArea.CreateEdge(vertices[0], vertices[1]);
        Assert.IsTrue(vertices[0].AdjacentEdgesRender.Any());
        Assert.IsTrue(vertices[1].AdjacentEdgesRender.Any());
    }

    [Test]
    public void RemoveEdge_RemovesEdgeFromVertices()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.CreateVertex(new Point(200, 200));
        var vertices = paintArea.GetVertices();
        var startVertex = vertices[0];
        var endVertex = vertices[1];
        var edge = new Edge(startVertex, endVertex);
        startVertex.AddEdge(edge);
        endVertex.AddEdge(edge);

        paintArea.RemoveEdge(edge);

        Assert.IsFalse(startVertex.AdjacentEdgesRender.Contains(edge));
        Assert.IsFalse(endVertex.AdjacentEdgesRender.Contains(edge));
    }

    [Test]
    public void UpdateUndoHistory_UpdatesUndoHistoryCorrectly()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.UpdateUndoHistory();
        Assert.AreEqual(1, paintArea.GetVertices().Count);

        paintArea.CreateVertex(new Point(200, 200));
        paintArea.UpdateUndoHistory();

        Assert.AreNotEqual(1, paintArea.GetVertices().Count);
        Assert.AreEqual(2, paintArea.GetVertices().Count);
    }

    [Test]
    public void MainForm_UndoCommand_RevertsToPreviousState()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.UpdateUndoHistory();
        paintArea.CreateVertex(new Point(200, 200));
        paintArea.MainForm_UndoCommand(this, EventArgs.Empty);

        Assert.AreEqual(1, paintArea.GetVertices().Count);
    }

    [Test]
    public void Clear_ClearsAllData()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.UpdateUndoHistory();
        paintArea.Clear();

        Assert.AreEqual(0, paintArea.GetVertices().Count);
        Assert.IsEmpty(paintArea.GetVertices());
    }
}