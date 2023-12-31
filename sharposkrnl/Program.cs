﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SharpOS.API;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace SharpOSKrnl
{
    public class Program
    {
        public static List<SharpOSComponentData> RUNNING_COMPONENTS = new List<SharpOSComponentData>();
        public static SharpOS.API.SharpOSUser CURRENT_USER = new SharpOSUser();
        public static void Main(string[] args)
        {
            Console.Title = "SO_BOOTMNGR";
            Console.WriteLine("Press alt+enter for fullscreen, press enter to remain windowed");
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "System", "Boot.dll")))
            {
                Console.WriteLine("ReadFileA failed (0x0231000 SO_FILENOTFOUND)\nSystem halted"); Application.Run();
            }
            Console.WriteLine("Setting user");
            CURRENT_USER.USER_NAME = "SYS";
            CURRENT_USER.USER_ISADMIN = true;
            Thread.Sleep(4000);
            try
            {
                SharpOSComponentUtl.LoadComponentFromDLL(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "System", "Boot.dll"), true, true);
                Application.Run();
            }
            catch(Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
                
                Console.WriteLine("An unhandled exception has occured and SharpOS needs to restart.\n\nException: " + ex + "\nHRESULT: " + ex.HResult + "\n\nThese exceptions (errors) are most likely caused due to faulty components.\nTry uninstalling a component you recently installed, and if the issue persists, contact @pabtsm_cool.\nIf you haven't installed a new component, it could be an already installed component.\nCreate a clone of your user (in the users folder) and uninstall all apps, in case of that scenario.\n\nPress any key to attempt to restart SharpOS.");
                Console.Beep(300, 250);
                Console.ReadKey();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Main(null);
            }
        }
    }
}
