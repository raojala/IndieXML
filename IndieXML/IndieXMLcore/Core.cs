using IndieXML;
using IndieXMLIPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace IndieXMLcore
{
    public class Core : IPlug
    {

        #region PROPSANDVARS
        DockPanel dpMain;
        StackPanel TopNav, BotNav, MainContent;
        DataGrid dgMain;

        TreeView trView;

        public string Name { get { return "Core"; } }
        #endregion

        #region METHODS

        public void Update(DockPanel dp)
        {
            try
            {
                dpMain = dp;
                trView = (TreeView)dpMain.Children[3];

                SetProperties();
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
                foreach (UIElement uiElement in dpMain.Children)
                {
                    if (uiElement.GetType() == typeof(StackPanel))
                    {
                        StackPanel sp = (StackPanel)uiElement;
                        if (sp.Name == "TopNav")
                        {
                            TopNav = sp;
                        }
                        if (sp.Name == "BotNav")
                        {
                            BotNav = sp;
                        }
                        if (sp.Name == "MainContent")
                        {
                            MainContent = sp;
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

        #endregion

        #region EVENTHANDLERS
        // event handler for import/eport xml button
        
        
        #endregion
    }
}

/*
(x64 build)
xml size: 349363KB
xml import time: 1:11.0310633
save bin time: 14.2248283
bin size: 137764KB
bin open time: 15.6126209

Rows: 1618559
*/
 
/*
 private void save_Click(object sender, RoutedEventArgs e)
    {


        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        settings.ConformanceLevel = ConformanceLevel.Fragment;

        using (StreamWriter sw = new StreamWriter("test.xml"))
        using (XmlWriter writer = XmlWriter.Create(sw, settings))
        {
            writer.WriteStartElement(ds.DataSetName);

            for (int i = 0; i < ds.Relations.Count; i++)
            {
                writer.WriteStartElement(ds.Relations[i].ParentTable.TableName);

                for (int y = 0; y < ds.Relations[i].ChildTable.Rows.Count; y++)
                {
                    writer.WriteStartElement(ds.Relations[i].ChildTable.TableName);
                    for (int x = 0; x < ds.Relations[i].ChildTable.Columns.Count; x++)
                    {
                        writer.WriteElementString(ds.Relations[i].ChildTable.Columns[x].ColumnName, ds.Relations[i].ChildTable.Rows[y][x].ToString());
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    } 
 */
