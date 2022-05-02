using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Samples.CommandBus
{
    public class MathReturnValue : IReturn
    {
        public int Result { get; set; }
    }

    public class AdditionCommand : ICommand
    {
        public int Augend { get; set; }
        public int Addend { get; set; }

        // Could add parameter validity checking in a static method that constructs this object.
    }

    public class AdditionCommandHandler : ICommandHandler<AdditionCommand, MathReturnValue>
    {
        public MathReturnValue Execute(AdditionCommand command)
        {
            var result = command.Augend + command.Addend;

            var returnValue = new MathReturnValue() { Result = result };

            return returnValue;
        }
    }
}