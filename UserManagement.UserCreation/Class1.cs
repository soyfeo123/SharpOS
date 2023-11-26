using System;
using System.Collections.Generic;
using System.Linq;
using SharpOS.API; //go into every single one of the projects, into references and make sure SharpOS.API is included as one
using System.Text;
using System.Threading.Tasks;

[assembly: SharpOSCmpntInit(typeof(SharpOS.UserManagement.UserCreation.MainUserCreation))]
namespace SharpOS.UserManagement.UserCreation
{
    public class MainUserCreation : SharpOSComponent
    {
        public override SharpOSComponentData OnComponentInit()
        {
            currentSOData = new SharpOSComponentData();
            currentSOData.COMPONENT_NAME = "UserCreationManager";
            currentSOData.SYSTEM = true;
            currentSOData.NON_ENDABLE = false;
            return currentSOData;
        }
        public override void OnComponentDataSent()
        {
            
        }
        public void UIDialogCreateUser()
        {
            Console.Clear();
            Console.WriteLine("\n\nSharpOS User Creation\n");
            Console.WriteLine("This tool will help you create your SharpOS user.\nUsers are required to run SharpOS, to neatly make user management.\n");
            Console.Write("Continue? Press [Y] for yes, [N] for no.\n");
            char key = Console.ReadKey().KeyChar;
            if(key == 'y')
            {
                Console.Clear();
                Console.Write("Enter your desired username: ");
                string username = Console.ReadLine();
                Console.Write("\nEnter your desired password: ");
                string password = Console.ReadLine();
                Console.Write("\nEnter a password hint (optional): ");
                string hint = Console.ReadLine();
                Console.WriteLine("\nFinished! Press any key to continue, " + username + ".");
                Console.ReadKey();
            }
        }
    }
}
