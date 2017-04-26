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

        // create XML menu button if it does not exist and create an export xml button under it.
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
                MenuItem setXML = new MenuItem { Header = "_Export as XML" };
                setXML.Click += ExportXMLEvent;
                if (mi2 != null)
                    mi2.Items.Add(setXML);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        // this event gets called when import xml key is pressed
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

        // oh boy this is a doosey....
        // get database name from database textbox and parse it to a proper format. each space is underscore.
        // then, write each database concept line by line.
        private void ExportXml()
        {
            try
            {
                // database name
                string databasename = MainWindow.TBName;
                databasename = databasename.Replace(" ", "_");

                // save
                SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "XML Files (*.xml)|*.xml" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // tell the writer that indentation is enabled, and that the indent is tab key press
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = "\t" };

                    using (StreamWriter sw = new StreamWriter(filePath))
                    using (XmlWriter writer = XmlWriter.Create(sw, settings))
                    {
                        writer.WriteStartElement(databasename);

                        for (int i = 0; i < MainWindow.DSTables.Relations.Count; i++)
                        {
                            writer.WriteStartElement(MainWindow.DSTables.Relations[i].ParentTable.TableName);

                            for (int y = 0; y < MainWindow.DSTables.Relations[i].ChildTable.Rows.Count; y++)
                            {
                                writer.WriteStartElement(MainWindow.DSTables.Relations[i].ChildTable.TableName);
                                for (int x = 0; x < MainWindow.DSTables.Relations[i].ChildTable.Columns.Count; x++)
                                {
                                    // compare a column to a parentname added with "_Id" to confirm it is not a primary key column before write.
                                    if (MainWindow.DSTables.Relations[i].ParentTable.TableName + "_Id" != MainWindow.DSTables.Relations[i].ChildTable.Columns[x].ColumnName)
                                    {
                                        // write the line.
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
