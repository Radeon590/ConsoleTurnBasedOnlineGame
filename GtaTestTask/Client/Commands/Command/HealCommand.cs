using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal class HealCommand : Command
    {
        public override string CommandString => "heal";

        public HealCommand(GtaClient client) : base(client)
        {
        }

        public override void Execute()
        {
            _client.Heal();
        }
    }
}
