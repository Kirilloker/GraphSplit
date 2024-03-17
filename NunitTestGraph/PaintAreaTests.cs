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
    // Перед тестами создаем PaintArea
    public void Setup()
    {
        mainForm = new MainForm();
        paintArea = new PaintArea(mainForm);
        paintArea.Initialize();
    }

    [Test]
    // Тест на добавление вершин
    public void CreateVertex()
    {
        paintArea.CreateVertex(new Point(100, 100));
        Assert.AreEqual(1, paintArea.GetVertices().Count);
    }

    [Test]
    // Тест на удаление вершин
    public void RemoveVertex()
    {
        paintArea.CreateVertex(new Point(100, 100));
        var vertex = paintArea.GetVertices().First();
        paintArea.RemoveVertex(vertex);
        Assert.AreEqual(0, paintArea.GetVertices().Count);
    }

    [Test]
    // Тест на добавление ребер
    public void CreateEdge()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.CreateVertex(new Point(200, 200));
        var vertices = paintArea.GetVertices();
        paintArea.CreateEdge(vertices[0], vertices[1]);
        Assert.IsTrue(vertices[0].AdjacentEdgesRender.Any());
        Assert.IsTrue(vertices[1].AdjacentEdgesRender.Any());
    }

    [Test]
    // Тест на удаление ребер
    public void RemoveEdge_()
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
    // Тест на корректную работы отмены действий
    public void MainForm_UndoCommand_RevertsToPreviousState()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.UpdateUndoHistory();
        paintArea.CreateVertex(new Point(200, 200));
        paintArea.MainForm_UndoCommand(this, EventArgs.Empty);

        Assert.AreEqual(1, paintArea.GetVertices().Count);
    }

    [Test]
    // Тест на очистку полотна
    public void Clear_ClearsAllData()
    {
        paintArea.CreateVertex(new Point(100, 100));
        paintArea.UpdateUndoHistory();
        paintArea.Clear();

        Assert.AreEqual(0, paintArea.GetVertices().Count);
        Assert.IsEmpty(paintArea.GetVertices());
    }
}