using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpOS.API;
[assembly: SharpOSCmpntInit(typeof(SharpOS.Boot.BootMain))]
namespace SharpOS.Boot
{
    public class BootMain : SharpOSComponent
    {
        public override SharpOSComponentData OnComponentInit()
        {
            
            SharpOSComponentData returnedCmpnt = new SharpOSComponentData();
            returnedCmpnt.COMPONENT_NAME = "Boot";
            returnedCmpnt.SYSTEM = true;
            returnedCmpnt.NON_ENDABLE = true;
            return returnedCmpnt;

        }
        public override void OnComponentDataSent()
        {
            Console.Title = "SharpOS Boot Manager";
            Console.WriteLine("Operation SHARPOSAPI.LOADCOMPONENTFROMDLL (Boot.dll) successful");
            Thread.Sleep(2000);
            Console.Clear();
            Console.CursorVisible = false;
            DoLetterRiseIGuess(0, 'S');
            DoLetterRiseIGuess(1, 'h');
            DoLetterRiseIGuess(2, 'a');
            DoLetterRiseIGuess(3, 'r');
            DoLetterRiseIGuess(4, 'p');
            DoLetterRiseIGuess(5, 'O');
            DoLetterRiseIGuess(6, 'S');
            Console.Clear();
            Console.WriteLine("SharpOS");
            Thread.Sleep(1000);
            Console.WriteLine("\na console OS made with C#");
            Console.CursorVisible = true;
            Thread.Sleep(5000);
        }

        void DoLetterRiseIGuess(int letterPos, char letter)
        {
            Console.CursorLeft = letterPos;
            Console.CursorTop = 3;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(letter);
            Thread.Sleep(75);
            Console.CursorLeft = letterPos;
            Console.Write("\b \b");
            Console.CursorLeft = letterPos;
            Console.CursorTop = 2;
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(letter);
            Thread.Sleep(75);
            Console.CursorLeft = letterPos;
            Console.Write("\b \b");
            Console.CursorLeft = letterPos;
            Console.CursorTop = 1;
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(letter);
            Thread.Sleep(75);
            Console.CursorLeft = letterPos;
            Console.Write("\b \b");
            Console.CursorLeft = letterPos;
            Console.CursorTop = 0;
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(letter);
            Thread.Sleep(75);
            
        }
    }
}
