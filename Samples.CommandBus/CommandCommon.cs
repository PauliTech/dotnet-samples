using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Samples.CommandBus
{
    /// <summary>
    /// Interface for the Command DTO, used to select which command handler is ran in the bus.
    /// </summary>
    public interface ICommand
    {

    }

    /// <summary>
    /// Interface for the return type of the command if it has Output.
    /// </summary>
    public interface IReturn
    {

    }
}