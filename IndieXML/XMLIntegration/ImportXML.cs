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
        DataGrid dgMain;
        StackPanel TopNav;
        public string Name { get { return "ImportXML"; } }

        public void Update()
        {
            try
            {
                SetProperties();
                CreateMenuItem();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateMenuItem()
        {
            try
            {
                Menu m = new Menu();
                MenuItem mi = new MenuItem(), mi2 = null;
                bool found = false;
                for (int i = 0; i < TopNav.Children.Count; i++)
                {
                    if (mi2 != null)
                        break;

                    if (TopNav.Children[i].GetType() == typeof(Menu))
                    {
                        m = (Menu)TopNav.Children[i];
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
                
                if (found == false)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Name = "miXML";
                    menuItem.Header = "_XML";
                    if (mi != null && mi.Name == "miFile")
                        mi.Items.Insert(mi.Items.Count - 1, menuItem);
                    mi2 = menuItem;
                }

                MenuItem getXML = new MenuItem();
                getXML.Header = "_Import as XML";
                getXML.Click += ImportXMLEvent;
                if (mi2 != null)
                    mi2.Items.Add(getXML);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetProperties()
        {
            try
            {
                TopNav = MainWindow.TopNav;
                dgMain = MainWindow.dgMain;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        // not working
        private void ImportXml()
        {
            try
            {
                DataSet ds = new DataSet();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
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
                    dgMain.Columns.Clear();
                    dgMain.DataContext = MainWindow.DSTables.Relations[0].ChildTable;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
