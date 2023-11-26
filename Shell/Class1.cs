using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpOS.API;

[assembly: SharpOSCmpntInit(typeof(SharpOS.UserMode.MainShell))]

namespace SharpOS.UserMode
{
    public class MainShell  : SharpOSComponent
    {
        public SharpOSUser shellUser { get; private set; }

        public override SharpOSComponentData OnComponentInit()
        {
            SharpOSComponentData data = new SharpOSComponentData();
            data.COMPONENT_NAME = "SharpOSShell";
            data.SYSTEM = true;
            data.NON_ENDABLE = true;
            return data;
        }

        public void PlayStartup()
        {
            SoundPlayer startupSound = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "SystemSounds", "Boot", "SO_STARTUP_RELAX.wav"));
            startupSound.Load();
            startupSound.Play();
        }

        public override void OnComponentDataSent()
        {
            shellUser = SharpOSUser.GetCurrentKernelUser();
            //PlayStartup();
            ConsoleKey key;
            int index = 0;
            List<string> viewMenuOptions = new List<string>()
            {
                "Desktop",
                "SharpOS Menu"
            };
            do
            {
                DrawMenuOptions(viewMenuOptions, index, "Select what you would like to view:");
                key = Console.ReadKey().Key;
                if(key == ConsoleKey.DownArrow && index + 1 < viewMenuOptions.Count)
                {
                    index++;
                }
                if(key == ConsoleKey.UpArrow && index - 1 >= 0)
                {
                    index--;
                }
            } while (key != ConsoleKey.Enter);

            if(index == 0)
            {
                Desktop();
            }
            if(index == 1)
            {
                StartMenu();
            }
        }

        public void StartMenu()
        {
            ConsoleKey key;
            int index = 0;
            List<string> MenuOptions = new List<string>()
            {
                "Power: Shutdown SharpOS",
                "Power: Restart SharpOS",
                "Back"
            };
            do
            {
                DrawMenuOptions(MenuOptions, index, "Select an action:");
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow && index + 1 < MenuOptions.Count)
                {
                    index++;
                }
                if (key == ConsoleKey.UpArrow && index - 1 >= 0)
                {
                    index--;
                }
            } while (key != ConsoleKey.Enter);
            
            if (index == MenuOptions.Count - 1)
            {
                OnComponentDataSent();
            }
            if (index == MenuOptions.Count - 2)
            {
                SharpOSComponentUtl.RestartSharpOS();
            }
            if(index == MenuOptions.Count - 3)
            {
                Console.Clear();
                Console.WriteLine("Shutting down...");
                new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "SystemSounds", "Boot", "SO_SHUTDOWN.wav")).Play();
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public void Desktop()
        {
            ConsoleKey key;
            int index = 0;
            List<string> MenuOptions = new List<string>()
            {
                "Desktop Feature under construction! Come back in a later version.",
                "Back"
            };
            do
            {
                DrawMenuOptions(MenuOptions, index, "Select an app:");
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow && index + 1 < MenuOptions.Count)
                {
                    index++;
                }
                if (key == ConsoleKey.UpArrow && index - 1 >= 0)
                {
                    index--;
                }
            } while (key != ConsoleKey.Enter);
            if(index == 0)
            {
                Desktop();
            }
            if(index ==  MenuOptions.Count - 1)
            {
                OnComponentDataSent();
            }
        }

        public void DrawMenuOptions(List<string> options, int selectedIndex, string selectText)
        {
            Console.Clear();
            Console.WriteLine($"{selectText}\n");
            foreach(string option in options)
            {
                if(option == options[selectedIndex])
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(option);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
