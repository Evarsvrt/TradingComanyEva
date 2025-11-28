using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompanyEva.ConsoleApp.Commands
{
    public interface IConsoleCommand
    {
        string Name { get; }

        
        object? Execute(object? state);
    }
}
