namespace GraphSplit
{
    public class CommandEventArgs : EventArgs
    {
        public Command Command { get; }

        public CommandEventArgs(Command command)
        {
            Command = command;
        }
    }

}
