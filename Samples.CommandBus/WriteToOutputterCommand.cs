namespace Samples.CommandBus
{
    public interface ITextOutputter
    {
        void Write(string text);
    }

    public class ConsoleTextOutputter : ITextOutputter
    {
        public void Write(string text)
        {
            System.Console.WriteLine(text);
        }
    }

    public class WriteToOutputterCommand : ICommand
    {
        public string Message { get; set; }

        private WriteToOutputterCommand()
        {
            this.Message = "";
        }

        /// <summary>
        /// Example of parameter checking.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static WriteToOutputterCommand Create(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new WriteToOutputterCommand() { Message = text };
        }
    }

    /// <summary>
    /// Example of a void command handler that depends on other services.
    /// </summary>
    public class WriteToOutputterCommandHandler : ICommandHandler<WriteToOutputterCommand>
    {
        private readonly ITextOutputter outputter;

        public WriteToOutputterCommandHandler(ITextOutputter outputter)
        {
            this.outputter = outputter;
        }

        public void Execute(WriteToOutputterCommand command)
        {
            this.outputter.Write(command.Message);

        }
    }
}