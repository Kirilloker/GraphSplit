namespace GraphSplit
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            MainForm.GetInstance().Show();
            Application.Run();
        }
    }
}