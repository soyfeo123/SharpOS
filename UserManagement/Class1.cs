﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOS.API;
using System.Threading.Tasks;
using System.IO;
using SharpOS.UserManagement.UserCreation;
using System.Threading;
using System.Media;

[assembly: SharpOSCmpntInit(typeof(SharpOS.UserManagement.UserManagementComponent))]

namespace SharpOS.UserManagement
{
    public class UserManagementComponent : SharpOSComponent
    {
        SoundPlayer startupSound = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "SystemSounds", "Boot", "SO_STARTUP_RELAX.wav"));
        
        public override SharpOSComponentData OnComponentInit()
        {

            SharpOSComponentData data = new SharpOSComponentData();
            data.COMPONENT_NAME = "User Management";
            data.SYSTEM = true;
            data.NON_ENDABLE = true;
            return data;
        }
        public override void OnComponentDataSent()
        {
            startupSound.Load();
            Console.WriteLine("LoadComponentFromDLL UserCreation");
            SharpOSComponentUtl.LoadComponentFromDLL(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "System", "UserManagement.UserCreation.dll"), true, true);
            Console.Title = "SharpOS User Login";
            if(Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "LoginStartup")).Length  > 0)
            {
                foreach(string file in  Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "LoginStartup")))
                {
                    SharpOSComponentUtl.LoadComponentFromDLL(file, false, true);
                }
            }
            DrawSelectUser();
        }

        public void PlayStartup()
        {
            startupSound.Play();
        }

        void DrawSelectUser()
        {
            Console.Clear();
            
            Console.WriteLine("Welcome to SharpOS.\n");
            string[] allUsers = new string[0];
            bool hasUser = false;
            if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users")) && (Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users")) != null || Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users")).Length > 0))
            {
                Console.WriteLine("Select your user:");
                allUsers = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users"));
                for (int i = 0; i < allUsers.Length; i++)
                {
                    Console.WriteLine((i + 1).ToString() + ") " + new DirectoryInfo(allUsers[i]).Name);
                }
                Console.WriteLine("\nOr:");
                hasUser = true;
            }
            Console.WriteLine("Press [C] to create a new user\n\n");
            Console.Write(">");
            char keyPressed = Console.ReadKey().KeyChar;
            if (char.IsNumber(keyPressed))
            {
                if(int.Parse(keyPressed.ToString()) > allUsers.Length)
                {
                    Console.WriteLine("\nThat user does not exist. Press any key to retry.");
                    Console.ReadKey();
                    DrawSelectUser();
                }
                else
                {
                    SharpOSUser user = SharpOSUser.LoginAndInitUser(new DirectoryInfo(allUsers[int.Parse(keyPressed.ToString()) - 1]).Name);
                    PlayStartup();
                    Console.WriteLine("\nWelcome, " + user.USER_NAME + ".");
                    Thread.Sleep(5000);
                    SharpOSComponentUtl.LoadComponentFromDLL(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SharpOS", "Shell.dll"), false, true);
                }
            }
            else if(keyPressed == 'c')
            {
                // TODO: fix this annoying heck random glitch
                //DrawNewUserScreen();
                Console.WriteLine("We're sorry, but that feature is under maintanance and will be disabled until it is fixed.\n\nNeed an alternative on creating a user?\nFollow these steps:\n\nGo to SharpOS's root folder, then create a \"Users\" folder.\nAfter that, create a new folder with your desired username.\nThen, create a \"p.txt\"file, and add your desired user password.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                DrawSelectUser();
            }
            else
            {
                DrawSelectUser();
            }
        }
        void DrawNewUserScreen()
        {
            foreach(SharpOSComponentData rc in SharpOSKrnl.Program.RUNNING_COMPONENTS)
            {
                if(rc.COMPONENT_NAME == "UserCreationManager")
                {
                    (rc.RUNNING_COMPONENT as UserCreation.MainUserCreation).UIDialogCreateUser();
                }
            }
            DrawSelectUser();
        }
    }
}
