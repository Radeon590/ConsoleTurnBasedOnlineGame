using GtaTestTask.Client.Commands.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands
{
    internal class Commands : List<Command.Command>
    {
        public void Execute()
        {
            foreach (var command in this) 
            {
                command.Execute();
            }
        }
    }
}
