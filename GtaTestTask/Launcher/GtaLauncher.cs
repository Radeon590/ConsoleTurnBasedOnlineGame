using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Launcher
{
    internal class GtaLauncher
    {
        public GtaLauncher()
        {
            Console.WriteLine("start launcher");
            
        }

        private void Authorize()
        {
            string username = ReadLine("username");
            string password = ReadLine("password");
        }

        private string ReadLine(string lineName)
        {
            string? result = null;
            while (result is null)
            {
                Console.WriteLine($"enter {lineName}");
                result = Console.ReadLine();
            }

            return result;
        }
    }
}
