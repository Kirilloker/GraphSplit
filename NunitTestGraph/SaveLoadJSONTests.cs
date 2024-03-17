using GraphSplit.GraphElements;
using GraphSplit.JSON;
using System.Drawing;

namespace GraphSplitTests.JSON
{
    [TestFixture]
    public class SaveLoadJSONTests
    {
        private const string TestFilename = "testVertices.json";

        [TearDown]
        // Удаляем файл перед тестированием
        public void TearDown()
        {
            if (File.Exists(TestFilename))
            {
                File.Delete(TestFilename);
            }
        }

        [Test]
        // Тест на корректное сохранение файла 
        public void SaveToJSON_SavesCorrectly()
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(100, 100), 0),
                new Vertex(new Point(200, 200), 1)
            };
            var edge = new Edge(vertices[0], vertices[1]);

            SaveLoadJSON.SaveToJSON(TestFilename, vertices);

            Assert.IsTrue(File.Exists(TestFilename));
            string content = File.ReadAllText(TestFilename);
            Assert.IsNotEmpty(content);
        }

        [Test]
        // Тест на правильную загрузку данных из файла
        public void LoadFromJSON_LoadsVerticesCorrectly()
        {
            var expectedVertices = new List<Vertex>
            {
                new Vertex(new Point(100, 100), 0),
                new Vertex(new Point(200, 200), 1)
            };
            expectedVertices[0].AddEdge(new Edge(expectedVertices[0], expectedVertices[1]));
            SaveLoadJSON.SaveToJSON(TestFilename, expectedVertices);

            var loadedVertices = SaveLoadJSON.LoadFromJSON(TestFilename);

            Assert.AreEqual(expectedVertices.Count, loadedVertices.Count);
            for (int i = 0; i < expectedVertices.Count; i++)
            {
                Assert.AreEqual(expectedVertices[i].Location, loadedVertices[i].Location);
                Assert.AreEqual(expectedVertices[i].Index, loadedVertices[i].Index);
                Assert.AreEqual(expectedVertices[i].AdjacentEdgesRender.Count, loadedVertices[i].AdjacentEdgesRender.Count);
            }
        }

        [Test]
        // тест на правильную загрузку данных из файла с сохранением ребер
        public void SaveAndLoad_WithComplexGraph_RestoresGraphIntegrity()
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(100, 100), 0),
                new Vertex(new Point(200, 200), 1),
                new Vertex(new Point(300, 300), 2)
            };
            new Edge(vertices[0], vertices[1]);
            new Edge(vertices[0], vertices[2]);
            new Edge(vertices[1], vertices[2]);

            SaveLoadJSON.SaveToJSON(TestFilename, vertices);
            var loadedVertices = SaveLoadJSON.LoadFromJSON(TestFilename);

            Assert.AreEqual(vertices.Count, loadedVertices.Count);
            foreach (var vertex in loadedVertices)
            {
                Assert.IsNotNull(vertex);
                Assert.IsTrue(vertex.AdjacentEdgesRender.Count > 0);
            }
        }

        [Test]
        // Тест на создание пустого файла
        public void SaveToJSON_CreatesEmptyJSONArray()
        {
            var vertices = new List<Vertex>();

            SaveLoadJSON.SaveToJSON(TestFilename, vertices);

            string content = File.ReadAllText(TestFilename);
            Assert.AreEqual("[]", content.Trim());
        }

        [Test]
        // Тест на загрузку пустого файла
        public void LoadFromJSON_ReturnsEmptyList()
        {
            File.WriteAllText(TestFilename, "[]");
            var loadedVertices = SaveLoadJSON.LoadFromJSON(TestFilename);

            Assert.IsNotNull(loadedVertices);
            Assert.IsEmpty(loadedVertices);
        }

        [Test]
        // Тест на загрузку файла с неправильным содержимым
        public void LoadFromJSON_InvalidJSON()
        {
            File.WriteAllText(TestFilename, "{invalid JSON}");
            Assert.Throws<System.Text.Json.JsonException>(() => SaveLoadJSON.LoadFromJSON(TestFilename));
        }

        [Test]
        // Тест на корректную загрузку location 
        public void SaveAndLoad_RestoresExactLocationOfVertices()
        {
            var originalVertices = new List<Vertex>
            {
                new Vertex(new Point(123, 456), 0),
                new Vertex(new Point(789, 101), 1)
            };
            SaveLoadJSON.SaveToJSON(TestFilename, originalVertices);

            var loadedVertices = SaveLoadJSON.LoadFromJSON(TestFilename);

            for (int i = 0; i < originalVertices.Count; i++)
            {
                Assert.AreEqual(originalVertices[i].Location, loadedVertices[i].Location);
            }
        }

        [Test]
        // Тест на загрузку файла с правильным количеством ребер между вершинами
        public void SaveAndLoad_RestoresEdgesBetweenVertices()
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Point(100, 100), 0),
                new Vertex(new Point(200, 200), 1),
                new Vertex(new Point(300, 300), 2)
            };
            vertices[0].AddEdge(new Edge(vertices[0], vertices[1]));
            vertices[1].AddEdge(new Edge(vertices[1], vertices[2]));

            SaveLoadJSON.SaveToJSON(TestFilename, vertices);
            var loadedVertices = SaveLoadJSON.LoadFromJSON(TestFilename);

            Assert.AreEqual(vertices[0].AdjacentEdgesRender.Count, loadedVertices[0].AdjacentEdgesRender.Count);
            Assert.AreEqual(vertices[1].AdjacentEdgesRender.Count, loadedVertices[1].AdjacentEdgesRender.Count);
            Assert.AreEqual(vertices[2].AdjacentEdgesRender.Count, loadedVertices[2].AdjacentEdgesRender.Count);
        }

        [Test]
        // Тест на проверку загрузку не существующего файла
        public void SaveAndLoad_FileNotFound_ThrowsFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => SaveLoadJSON.LoadFromJSON("nonexistent_file.json"));
        }
    }
}