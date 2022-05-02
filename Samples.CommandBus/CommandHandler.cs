namespace Samples.CommandBus
{
    /// <summary>
    /// Interface for the definition of a command handler that accepts a command DTO and produces a return value.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public interface ICommandHandler<TCommand, TReturn> where TCommand : ICommand where TReturn : IReturn
    {
        TReturn Execute(TCommand command);

    }

    /// <summary>
    /// Interface for the definition of a command handler that accepts a command DTO has has a void return.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);

    }
}