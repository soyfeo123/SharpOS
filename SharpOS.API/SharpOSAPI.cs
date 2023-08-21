using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SharpOS.API
{
    public struct SharpOSComponentData
    {
        public string COMPONENT_NAME;
        public bool SYSTEM;
        public bool NON_ENDABLE;
        public SharpOSComponent RUNNING_COMPONENT;
    }

    public class SharpOSComponent
    {
        public SharpOSComponentData currentSOData;
        public virtual SharpOSComponentData OnComponentInit()
        {
            
            currentSOData = new SharpOSComponentData();
            return currentSOData;
        }
        public virtual void OnComponentDataSent()
        {

        }
        public virtual void OnComponentEnded()
        {
            if (currentSOData.NON_ENDABLE == false)
            {
                SharpOSKrnl.Program.RUNNING_COMPONENTS.Remove(currentSOData);
            }
            else
            {
                throw new Exception("Component cannot be ended due to the NON_ENDABLE value being set to True.");
            }
        }
        public void EndComponent()
        {
            OnComponentEnded();
        }
    }

    public class SharpOSCmpntInit : Attribute
    {
        public Type MainClass;
        public SharpOSCmpntInit(Type mainClass) { MainClass = mainClass; }
    }

    public class UncompatibleComponentException : Exception
    {
        public UncompatibleComponentException(string message) : base(message) { }
    }

    public static class SharpOSComponentUtl
    {
        /// <summary>
        /// Loads a SharpOS Component through a .NET Framework DLL. (All DLLs must contain .NET Framework v4.2.7! No exceptions.
        /// </summary>
        /// <param name="cmpnt">The path to the DLL.</param>
        /// <param name="dbg">If to send debug information to the output. (Default: false).</param>
        /// <param name="autoStart">If to automatically handle starting the component. (Putting it to the component list, etc.) (Default: true).</param>
        /// <returns>The component.</returns>
        /// <exception cref="UncompatibleComponentException">Happens if the component loaded is, well, incompatible.</exception>
        public static SharpOSComponent LoadComponentFromDLL(string cmpnt, bool dbg = false, bool autoStart = true)
        {

            var dll = Assembly.LoadFile(cmpnt);
            
            if (dbg)
            {
                AssemblyName info = dll.GetName();
                Console.WriteLine("ASM_NAME: " + info.Name);
                Console.WriteLine("ASM_VER: " + info.Version);
            }
            SharpOSCmpntInit mainInfo = dll.GetCustomAttribute<SharpOSCmpntInit>();
            if(mainInfo == null)
            {
                throw new UncompatibleComponentException("Component does not make use of the SharpOSCmpntInit attribute. No main class found.");
            }
            object activator = Activator.CreateInstance(mainInfo.MainClass);
            SharpOSComponent component = activator as SharpOSComponent;
            if(component == null)
            {
                throw new UncompatibleComponentException("Component does not have SharpOSComponent class inherited in the class defined in SharpOSCmpntInit.");
            }
            if (autoStart)
            {
                SharpOSComponentData data = component.OnComponentInit();
                data.RUNNING_COMPONENT = component;
                SharpOSKrnl.Program.RUNNING_COMPONENTS.Add(data);
                component.OnComponentDataSent();
            }
            return component;
        }
    }
}
