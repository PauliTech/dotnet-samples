namespace Samples.CommandBus
{
    /// <summary>
    /// Command bus interface.
    /// </summary>
    public interface ICommandBus
    {
        void Execute<TCommand>(TCommand command) where TCommand : ICommand;

        TResponse Execute<TCommand, TResponse>(TCommand command) where TCommand : ICommand where TResponse : IReturn;
    }

    /// <summary>
    /// Implementation of a command bus that can action command handlers with return types and with void returns.
    /// </summary>
    public class CommandBus : ICommandBus
    {
        public Dictionary<Type, Func<ICommand, IReturn>> handlers = new();
        public Dictionary<Type, Action<ICommand>> voidHandlers = new();

        /// <summary>
        /// Called during service collection start to populate the collection with command handlers.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="handler"></param>
        public void Register<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> handler) where TCommand : ICommand where TResponse : IReturn
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.handlers.Add(typeof(TCommand), (command) => handler.Execute((TCommand)command));
        }

        /// <summary>
        /// Called during service collection start to populate the collection with command handlers.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="handler"></param>
        public void Register<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.voidHandlers.Add(typeof(TCommand), (command) => handler.Execute((TCommand)command));
        }

        /// <summary>
        /// Given the Command select the command handler and execute it.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        public void Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (this.voidHandlers.ContainsKey(typeof(TCommand)))
            {
                this.voidHandlers[typeof(TCommand)].Invoke(command);
            }
        }

        /// <summary>
        /// Given the Command select the command handler and execute it, returns the command retrun value.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        public TResponse Execute<TCommand, TResponse>(TCommand command) where TCommand : ICommand where TResponse : IReturn
        {
            if (this.handlers.ContainsKey(typeof(TCommand)))
            {
                return (TResponse)this.handlers[typeof(TCommand)].Invoke(command);
            }

            throw new KeyNotFoundException($"command handler for type {typeof(TCommand)} not found");
        }
    }
}