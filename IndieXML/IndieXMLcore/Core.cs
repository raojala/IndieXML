using IndieXMLIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IndieXMLcore
{
    public class Core : IPlug
    {

        Menu menu = new Menu();

        public string Name
        {
            get
            {
                return "Core";
            }
        }

        public void Do (StackPanel TopNav, StackPanel BotNav, StackPanel MainContent)
        {
            try
            {
                //System.Windows.MessageBox.Show("Do Something in First Plugin");

                MenuItem file = new MenuItem();
                file.Header = "File";
                file.Name = "miFile";

                MenuItem quit = new MenuItem();
                quit.Header = "Quit";
                quit.Name = "miQuit";
                quit.Click += ApplicationQuit; // bind quit button for quit event.

                file.Items.Add(quit);
                
                menu.Items.Add(file);
                TopNav.Children.Add(menu);
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }

        // application shutdown event
        private void ApplicationQuit(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
