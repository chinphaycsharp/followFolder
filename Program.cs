using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace followFolder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            bool console = false;

            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
                if (args[i] == "-console")
                {
                    console = true;
                    break;
                }
            }
            if (console)
            {
                myService.RunCmd(new myService());
            }
            else
            {
                System.ServiceProcess.ServiceBase sb = new System.ServiceProcess.ServiceBase();
                System.ServiceProcess.ServiceBase.Run(new myService());
            }
        }
    }
}
