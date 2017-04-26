using IndieXML;
using IndieXMLIPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XMLIntegration
{
    class ImportXML : IPlug
    {
        public string Name { get { return "ImportXML"; } }
        public void Update()
        {
            try
            {
                CreateMenuItem();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // create XML menu button if it does not exist and create an import xml button under it.
        private void CreateMenuItem()
        {
            try
            {
                Menu m = new Menu();
                MenuItem mi = new MenuItem(), mi2 = null;
                bool found = false;
                for (int i = 0; i < MainWindow.TopNav.Children.Count; i++)
                {
                    if (mi2 != null)
                        break;

                    if (MainWindow.TopNav.Children[i].GetType() == typeof(Menu))
                    {
                        m = (Menu)MainWindow.TopNav.Children[i];
                        if (m.Name == "FileMenu")
                        {
                            for (int x = 0; x < m.Items.Count; x++)
                            {
                                if (mi2 != null)
                                    break;

                                mi = ((MenuItem)m.Items[x]);
                                if (mi.Name == "miFile")
                                {
                                    for (int y = 0; y < mi.Items.Count; y++)
                                    {
                                        if (mi2 != null)
                                            break;

                                        // found parent
                                        if (((MenuItem)mi.Items[y]).Name == "miXML")
                                        {
                                            mi2 = ((MenuItem)mi.Items[y]);
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                // parent was not found so we create one.
                if (found == false)
                {
                    MenuItem menuItem = new MenuItem { Name = "miXML", Header = "_XML" };
                    if (mi != null && mi.Name == "miFile")
                        mi.Items.Insert(mi.Items.Count - 1, menuItem);
                    mi2 = menuItem;
                }

                // create the button
                MenuItem getXML = new MenuItem { Header = "_Import as XML" };
                getXML.Click += ImportXMLEvent;
                if (mi2 != null)
                    mi2.Items.Add(getXML);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // event that gets called when import xml button is pressed
        void ImportXMLEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportXml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // read the xml into a dataset and place the child of the first relation as a datacontext
        // also set the text value of our database name textbox to match the name of the dataset.
        private void ImportXml()
        {
            try
            {
                DataSet ds = new DataSet();
                OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "XML Files (*.xml)|*.xml" };
                if (openFileDialog.ShowDialog() == true)
                {
                    ds.ReadXml(openFileDialog.FileName);
                }

                if (ds.Relations.Count < 1)
                {
                    // no relations
                    // what if there are nested relations?
                }
                else
                {
                    MainWindow.DSTables = ds;
                    MainWindow.DGMain.Columns.Clear();
                    MainWindow.DGMain.DataContext = MainWindow.DSTables.Relations[0].ChildTable;
                    MainWindow.TBName = ds.DataSetName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
