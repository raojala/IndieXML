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

        public PluginCore (StackPanel TopNav, StackPanel BotNav, StackPanel MainContent)
        {
            LoadPlugins(TopNav, BotNav, MainContent);
        }

        private void LoadPlugins(StackPanel TopNav, StackPanel BotNav, StackPanel MainContent)
        {
            try
            {

                // specifies the folder for plugins and lists the DLL paths to an array
                string pluginPath = Directory.GetCurrentDirectory() + @"\Plugins\";
                string[] files = Directory.GetFiles(pluginPath, "*.dll");

                // use dll paths to load the DLL files to a list
                List<Assembly> assemblies = new List<Assembly>();
                foreach (string s in files)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(s);
                    Assembly assembly = Assembly.Load(an);
                    //System.Windows.MessageBox.Show(assembly.GetType().ToString());
                    assemblies.Add(assembly); // add plugin to list
                }

                Type pluginType = typeof(IPlug);
                List<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }

                Dictionary<string, IPlug> plugins = new Dictionary<string, IPlug>();
                foreach (Type type in pluginTypes)
                {

                    IPlug plug = (IPlug)Activator.CreateInstance(type);
                    plugins.Add(plug.Name, plug);
                    plug.Do(TopNav, BotNav, MainContent);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}