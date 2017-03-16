using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndieXML
{
    class PluginCore
    {
        private string[] files;
        public string[] Files
        {
            get
            {
                return files;
            }
        }


        public PluginCore ()
        {
            LoadPlugins();
        }

        public void LoadPlugins()
        {
            string pluginPath = Directory.GetCurrentDirectory() + @"\Plugins\";
            files = Directory.GetFiles(pluginPath, "*.dll");

            foreach(string s in files)
            {

            }



        }


    }
}