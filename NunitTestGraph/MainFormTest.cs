using NUnit.Framework;
using GraphSplit;
using System.Windows.Forms;
using Moq;
using GraphSplit.UIElements;
using System.Drawing;

namespace GraphSplitTests
{
    [TestFixture]
    public class MainFormTests
    {
        private MainForm form;

        [SetUp]
        public void SetUp()
        {
            form = new MainForm();
            form.InitializeForm();
        }


        [Test]
        public void MainForm_HasMinimumSizeSet()
        {
            var expectedMinimumSize = new Size(800, 600);
            Assert.AreEqual(expectedMinimumSize, form.MinimumSize);
        }

        [Test]
        public void MainForm_HasMaximumSizeSet()
        {
            var expectedMaximumSize = new Size(1300, 800);
            Assert.AreEqual(expectedMaximumSize, form.MaximumSize);
        }

        [Test]
        public void LastUseName_SetsFormTitleCorrectly()
        {
            string testName = "TestGraph";
            form.LastUseName = testName;

            Assert.IsTrue(form.Text.Contains(testName));
        }

        [Test]
        public void LastUseName_WithEmptyString_SetsFormTitleWithAsterisk()
        {
            form.LastUseName = "";

            Assert.IsTrue(form.Text.EndsWith("(*)"));
        }

        [Test]
        public void GetButtons_ReturnsControlButtons()
        {
            var buttons = form.GetButtons();
            Assert.IsNotNull(buttons);
            Assert.IsInstanceOf<List<Button>>(buttons);
            Assert.IsTrue(buttons.Count > 0);
        }
    }
}