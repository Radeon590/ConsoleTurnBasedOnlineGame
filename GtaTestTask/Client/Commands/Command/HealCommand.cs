using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal class HealCommand : ICommand
    {
        public string CommandString => "heal";

        GtaClient ICommand.Client => _client;

        private readonly GtaClient _client;

        public HealCommand(GtaClient client) 
        {
            _client = client;
        }

        public void Execute()
        {
            _client.Heal();
        }
    }
}
