using IndieXML;
using IndieXMLIPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
        StackPanel TopNav;
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

        private void SetProperties()
        {
            try
            {
                TopNav = MainWindow.TopNav;
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
                string databasename = MainWindow.TBName.Text;
                databasename = databasename.Replace(" ", "_");
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "\t";

                    using (StreamWriter sw = new StreamWriter(filePath))
                    using (XmlWriter writer = XmlWriter.Create(sw, settings))
                    {
                        // writer.WriteStartDocument(true);
                        writer.WriteStartElement(databasename);

                        for (int i = 0; i < MainWindow.DSTables.Relations.Count; i++)
                        {
                            writer.WriteStartElement(MainWindow.DSTables.Relations[i].ParentTable.TableName);

                            for (int y = 0; y < MainWindow.DSTables.Relations[i].ChildTable.Rows.Count; y++)
                            {
                                writer.WriteStartElement(MainWindow.DSTables.Relations[i].ChildTable.TableName);
                                for (int x = 0; x < MainWindow.DSTables.Relations[i].ChildTable.Columns.Count; x++)
                                {
                                    if (MainWindow.DSTables.Relations[i].ParentTable.TableName + "_Id" != MainWindow.DSTables.Relations[i].ChildTable.Columns[x].ColumnName)
                                    {
                                        writer.WriteElementString(MainWindow.DSTables.Relations[i].ChildTable.Columns[x].ColumnName, MainWindow.DSTables.Relations[i].ChildTable.Rows[y][x].ToString());
                                    }
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
