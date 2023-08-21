using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOS.API;
using System.Threading.Tasks;
using System.IO;
using SharpOS.UserManagement.UserCreation;

[assembly: SharpOSCmpntInit(typeof(SharpOS.UserManagement.UserManagementComponent))]

namespace SharpOS.UserManagement
{
    public class UserManagementComponent : SharpOSComponent
    {
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
            }
            else if(keyPressed == 'c')
            {
                DrawNewUserScreen();
            }
            else
            {
                DrawSelectUser();
            }
        }
        void DrawNewUserScreen()
        {
            for(int i = 0; i < SharpOSKrnl.Program.RUNNING_COMPONENTS.Count; i++)
            {
                if (SharpOSKrnl.Program.RUNNING_COMPONENTS[i].COMPONENT_NAME == "UserCreationManager")
                {
                    (SharpOSKrnl.Program.RUNNING_COMPONENTS[i].RUNNING_COMPONENT as MainUserCreation).UIDialogCreateUser();
                }
            }
            DrawSelectUser();
        }
    }
}
