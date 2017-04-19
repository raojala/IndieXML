using IndieXML;
using IndieXMLIPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace XMLIntegration
{
    class ExportXML : IPlug
    {
        public string Name { get { return "ExportXML"; } }
        StackPanel sp = new StackPanel();
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

                MenuItem setXML = new MenuItem();
                setXML.Header = "_Export as XML";
                setXML.Click += ExportXMLEvent;
                if (mi2 != null)
                    mi2.Items.Add(setXML);
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ExportXMLEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportXml();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExportXml()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    //if (File.Exists(filePath))
                    //{
                    //    File.Delete(filePath);
                    //}

                    //DataTable dt = ((DataView)dgMain.ItemsSource).ToTable();
                    //ds.WriteXml(filePath);
                    //ds.WriteXmlSchema(filePath + "_schema");

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    settings.ConformanceLevel = ConformanceLevel.Fragment;

                    using (StreamWriter sw = new StreamWriter(filePath))
                    using (XmlWriter writer = XmlWriter.Create(sw, settings))
                    {
                        writer.WriteStartElement(MainWindow.dataset.DataSetName);

                        for (int i = 0; i < MainWindow.dataset.Relations.Count; i++)
                        {
                            writer.WriteStartElement(MainWindow.dataset.Relations[i].ParentTable.TableName);

                            for (int y = 0; y < MainWindow.dataset.Relations[i].ChildTable.Rows.Count; y++)
                            {
                                writer.WriteStartElement(MainWindow.dataset.Relations[i].ChildTable.TableName);
                                for (int x = 0; x < MainWindow.dataset.Relations[i].ChildTable.Columns.Count; x++)
                                {
                                    writer.WriteElementString(MainWindow.dataset.Relations[i].ChildTable.Columns[x].ColumnName, MainWindow.dataset.Relations[i].ChildTable.Rows[y][x].ToString());
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
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
