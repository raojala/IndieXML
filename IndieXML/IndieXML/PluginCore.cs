using IndieXMLIPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IndieXML
{
    class PluginCore
    {
        // default constructor
        public PluginCore()
        {
            try
            {
                LoadPlugins();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // load in our dlls
        private void LoadPlugins()
        {
            try
            {

                Type pluginType = typeof(IPlug); // what type to look for from assembly types
                Dictionary<string, IPlug> plugins = new Dictionary<string, IPlug>(); // list of plugins

                // Paths
                string pluginPath = Directory.GetCurrentDirectory() + @"\Plugins\"; // get path for our plugin directory
                string[] files = Directory.GetFiles(pluginPath, "*.dll"); // get paths to every .dll file.

                foreach (string s in files)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(s); // get assemblyname from dll paths string
                    Assembly assembly = Assembly.Load(an); // load in the assembly

                    Type[] types = assembly.GetTypes(); //list all types the assembly has
                    foreach (Type type in types)
                    {
                        // check that the type we are looking at is a class that implements our interface and not an abstract class or interface file
                        if (type.GetInterface(pluginType.FullName) != null && !type.IsAbstract && !type.IsInterface) 
                        {
                            IPlug plug = (IPlug)Activator.CreateInstance(type); // run the plugin
                            plugins.Add(plug.Name, plug); // store the plugin in dictionary with a keyword, for later use.
                            plug.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}