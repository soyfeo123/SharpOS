using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SharpOS.API;
using System.Windows.Forms;
using System.Threading;

namespace SharpOSKrnl
{
    public class Program
    {
        public static List<SharpOSComponentData> RUNNING_COMPONENTS = new List<SharpOSComponentData>();
        static void Main(string[] args)
        {
            Console.Title = "SO_BOOTMNGR";
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "System", "Boot.dll")))
            {
                Console.WriteLine("ReadFileA failed (0x0231000 SO_FILENOTFOUND)\nSystem halted"); Application.Run();
            }
            Thread.Sleep(4000);
            SharpOSComponent boot = SharpOSComponentUtl.LoadComponentFromDLL(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "System", "Boot.dll"), false, true);
            Application.Run();
        }
    }
}
