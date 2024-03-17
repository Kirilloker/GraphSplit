using GraphSplit;
using System.Drawing;
using System.Windows.Forms;

namespace GraphSplitTests
{
    [TestFixture]
    public class MainFormTests
    {
        private MainForm form;

        [SetUp]
        // Перед тестами создаем MainForm
        public void SetUp()
        {
            form = new MainForm();
            form.InitializeForm();
        }


        [Test]
        // Тест на минимальный размер
        public void MinimumSizeSet()
        {
            var expectedMinimumSize = new Size(800, 600);
            Assert.AreEqual(expectedMinimumSize, form.MinimumSize);
        }

        [Test]
        // Тест на максимальный размер
        public void MaximumSizeSet()
        {
            var expectedMaximumSize = new Size(1300, 800);
            Assert.AreEqual(expectedMaximumSize, form.MaximumSize);
        }

        [Test]
        // Тест на правильную установку имени окна
        public void FormTitleCorrectly()
        {
            string testName = "TestGraph";
            form.LastUseName = testName;

            Assert.IsTrue(form.Text.Contains(testName));

            form.LastUseName = "";
            Assert.IsTrue(form.Text.EndsWith("(*)"));
        }
    }
}