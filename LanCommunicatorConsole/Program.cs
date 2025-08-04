using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanCommunicatorConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MulticastListener.MulticastChat.Run();
        }
    }
}
