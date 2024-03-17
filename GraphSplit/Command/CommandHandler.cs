namespace GraphSplit
{
    public static class CommandHandler
    {
        private static readonly Dictionary<Keys, Command> keyCommandMap = new()
        {
            [Keys.V] = Command.AddVertex,
            [Keys.D] = Command.DeleteElement,
            [Keys.E] = Command.AddEdge
        };

        public static event EventHandler<CommandEventArgs> CommandSelected;
        public static event EventHandler UndoCommand;
        public static event EventHandler SaveCommand;
        public static event EventHandler SaveAsCommand;

        private static Command command;

        public static void SelectedCommand(Command selectedCommand)
        {
            command = selectedCommand;
            OnCommandSelected(selectedCommand);
        }

        public static bool HandleKeyCommand(Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                OnSaveAsCommand();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Z))
            {
                OnUndoCommand();
                return true;
            }

            if (keyData == (Keys.Control | Keys.S))
            {
                OnSaveCommand();
                return true;
            }

            if (keyCommandMap.TryGetValue(keyData, out Command command))
            {
                SelectedCommand(command);
                return true;
            }

            return false;
        }

        private static void OnCommandSelected(Command command)
        {
            CommandSelected?.Invoke(null, new CommandEventArgs(command));
        }

        private static void OnUndoCommand()
        {
            UndoCommand?.Invoke(null, EventArgs.Empty);
        }

        private static void OnSaveCommand()
        {
            SaveCommand?.Invoke(null, EventArgs.Empty);
        }

        private static void OnSaveAsCommand()
        {
            SaveAsCommand?.Invoke(null, EventArgs.Empty);
        }

        public static Command Command { get { return command; } }
    }
}
