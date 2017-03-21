using IndieXMLIPlugin;
using System;
using System.Collections.Generic;
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
        public PluginCore(StackPanel TopNav, StackPanel BotNav, StackPanel MainContent)
        {
            LoadPlugins(TopNav, BotNav, MainContent); // we need menus here so we can pass them on in case plugins want to implement menuitems
        }

        private void LoadPlugins(StackPanel TopNav, StackPanel BotNav, StackPanel MainContent)
        {
            try
            {
                Type pluginType = typeof(IPlug); // what type to look for from assembly types //#CodeID001
                Dictionary<string, IPlug> plugins = new Dictionary<string, IPlug>(); // list of plugins

                // Paths
                string pluginPath = Directory.GetCurrentDirectory() + @"\Plugins\"; // get path for our plugin directory
                string[] files = Directory.GetFiles(pluginPath, "*.dll"); // get paths to every .dll file.
                
                foreach (string s in files)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(s); // get assemblyname from dll paths string
                    Assembly assembly = Assembly.Load(an); // load in the assembly
                    
                    Type[] types = assembly.GetTypes(); //#CodeID001, list all types the assembly has
                    foreach (Type type in types)
                    {
                        if (type.GetInterface(pluginType.FullName) != null && !type.IsAbstract && !type.IsInterface) // check that the type we are looking at is a class that implements our interface and not an abstract class or interface file
                        {
                            System.Windows.MessageBox.Show(type.GetInterface(pluginType.FullName).ToString()); // temporary debug statement #!#!#!#!#!#!#

                            IPlug plug = (IPlug)Activator.CreateInstance(type);
                            plugins.Add(plug.Name, plug); // store the plugin in dictionary with a keyword, for later use.
                            plug.Update(TopNav, BotNav, MainContent);
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