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
        public string Name { get { return "ImportXML"; } }
        StackPanel sp;
        public void Update(DockPanel dp)
        {
            try
            {
                SetProperties(dp);
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
                for (int i = 0; i < sp.Children.Count; i++)
                {
                    if (mi2 != null)
                        break;

                    if (sp.Children[i].GetType() == typeof(Menu))
                    {
                        m = (Menu)sp.Children[i];
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

        private void SetProperties(DockPanel dock)
        {
            try
            {
                foreach (UIElement uiElement in dock.Children)
                {
                    if (uiElement.GetType() == typeof(StackPanel))
                    {
                        StackPanel temp = (StackPanel)uiElement;
                        if (temp.Name == "TopNav")
                        {
                            sp = temp;
                        }
                    }
                    else if (uiElement.GetType() == typeof(DataGrid))
                    {
                        if (((DataGrid)uiElement).Name == "dgMainView")
                        {
                            dgMain = (DataGrid)uiElement;
                        }
                    }
                }
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
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";

                if (openFileDialog.ShowDialog() == true)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(openFileDialog.FileName);
                    MainWindow.dataset = ds;

                    // MainWindow.dataset.ReadXml(openFileDialog.FileName);

                    dgMain.Columns.Clear();
                    dgMain.DataContext = MainWindow.dataset.Relations[0].ChildTable;

                    //if (MainWindow.dataset.Relations.Count <= 0)
                    //{
                    //    //ExportXml();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
