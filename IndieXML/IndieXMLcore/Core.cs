﻿using IndieXMLIPlugin;
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

namespace IndieXMLcore
{
    public class Core : IPlug
    {

        #region PROPSANDVARS
        DockPanel dpMain;
        StackPanel TopNav, BotNav, MainContent;
        DataGrid dgMain;

        DateTime debugTime;

        public string Name
        {
            get
            {
                return "Core";
            }
        }
        #endregion

        #region METHODS

        public void Update(DockPanel dp)
        {
            try
            {
                dpMain = dp;
                SetProperties();
                Buttons();
                InitDataGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitDataGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Col 1");
            dt.Columns.Add("Col 2");
            dt.Columns.Add("Col 3");
            dgMain.ItemsSource = dt.DefaultView;
            ((DataView)dgMain.ItemsSource).Table.Rows.Add();
            dgMain.CellEditEnding += SetNewLastRow;
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
                        dgMain = (DataGrid)uiElement;
                        if (dgMain.Name == "dgMainView")
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Buttons()
        {
            try
            {
                // first item in menu is always the text that is showing in the topnav
                // so every menuitem is a child of a menuitem that is a child of a menu
                // the first child of a menu is the menu name, and all the childs of that child
                // are actual menuitems in the dropdown.

                // menuitem that shows in topnav
                MenuItem file = new MenuItem();
                file.Header = "_File"; // underscore in header places shortcut key.

                MenuItem saveFile = new MenuItem();
                saveFile.Header = "_Save File";
                saveFile.Click += saveFileEvent;
                file.Items.Add(saveFile);

                MenuItem loadFile = new MenuItem();
                loadFile.Header = "_Load File";
                loadFile.Click += loadFileEvent;
                file.Items.Add(loadFile);

                MenuItem getXML = new MenuItem();
                getXML.Header = "_Import XML";
                getXML.Click += ImportXMLEvent;
                file.Items.Add(getXML);


                // menuitems that show under the file in topnav
                MenuItem quit = new MenuItem();
                quit.Header = "_Quit";
                quit.Click += ApplicationQuit; // bind quit button for quit event.
                file.Items.Add(quit);
                // actual menu object that holds our menu
                Menu menu = new Menu();
                menu.Items.Add(file);

                // add menu to topnav
                TopNav.Children.Add(menu);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ImportXml()
        {
            try
            {

                DataSet ds = new DataSet();
                DataView dv;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    // file.Source = new Uri(openFileDialog.FileName);
                    debugTime = DateTime.Now;
                    ds.ReadXml(openFileDialog.FileName);
                }

                dv = ds.Tables[0].DefaultView;

                dgMain.Columns.Clear();
                dgMain.ItemsSource = dv;
                System.Windows.MessageBox.Show((DateTime.Now - debugTime).ToString());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SaveFile()
        {
            string filePath = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                FileStream test = new FileStream(filePath, FileMode.Create);
                test.Close();
            }
            else
            {
                File.Delete(filePath);
                FileStream test = new FileStream(filePath, FileMode.Create);
                test.Close();
            }

            /*isoissa xml tiedostoissa loppuu muisti.*/
            //FileStream fs = new FileStream(filePath, FileMode.Append);
            //BinaryFormatter bf = new BinaryFormatter();
            //debugTime = DateTime.Now;

            //for (int i = 0; i < ((DataView)dgMain.ItemsSource).ToTable().Rows.Count; i++)
            //{
            //    bf.Serialize(fs, ((DataRow)((DataView)dgMain.ItemsSource).ToTable().Rows[i]));
            //}

            // #### how to serialize one row at a time ??!?!?!?!?! ############################################################################
            
            //fs.Close();
            MessageBox.Show((DateTime.Now - debugTime).ToString());
        }


        private void LoadFile()
        {
            FileStream fs = new FileStream(@"test.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            debugTime = DateTime.Now;
            DataTable dt = (DataTable)bf.Deserialize(fs);
            fs.Close();
            MessageBox.Show((DateTime.Now - debugTime).ToString());
            dgMain.Columns.Clear();
            dgMain.ItemsSource = dt.DefaultView;
        }

        #endregion

        #region EVENTHANDLERS

        // application shutdown event for quit menuitem
        private void ApplicationQuit(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // event method to add rownumbers
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        // event handler for import xml button
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

        // even to listen save and load button clicks
        void saveFileEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFile();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void loadFileEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadFile();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // add new row if last row in the collection was edited.
        private void SetNewLastRow(object sender, DataGridCellEditEndingEventArgs e)
        {
            /* add one new row at the beginning of an cell edit. 
             * (otherwise if uses doesn't create one manually after edit, 
             * the last row won't save)
             * Items.count needs -2 instead of -1 BECAUSE datagrid items has one item always
             * stored to the last position which is {new item placeholder}
             */

            if (dgMain.Items.Count - 1 == dgMain.Items.IndexOf(dgMain.CurrentItem) && e.EditAction == DataGridEditAction.Commit)
            {
                ((DataView)dgMain.ItemsSource).Table.Rows.Add();
            }
        }

        #endregion
    }
}

// XML - BIN load time comparisons
/*  W3Schools, plant_catalog.xml (copypasted existing data to make it bigger)
XML file size: 12620KB
importtaus aika: 2.2450556 sekuntia
-> save to bin time: 1.1165021 sekuntia
Bin size: 17 852KB
Load bin time: 0.6721342

3.340 times faster
*/

/* W3Schools, plant_catalog.xml (copypasted existing data to make it bigger)
XML file size: 108982KB
import time: 12.9539345 seconds
-> save to bin time: 5.1722389 seconds
Bin size: 155115KB
*Restart
Load bin time: 3.4223638 seconds

3.785 times faster
*/

/* W3Schools, plant_catalog.xml (Unedited)
XML file size: 8KB
import time: 0.0150168 seconds
-> save to bin time: 0.0080184 seconds
Bin size: 16KB
*Restart
Load bin time: 0.0090109 seconds

1.6665 times faster
*/

/* W3Schools, plant_catalog.xml (copypasted existing data to make it bigger)
XML file size: 314540KB
import time: 38.2134741seconds
-> save to bin time: seconds
Bin size: KB
*Restart
Load bin time: seconds

## FAILED, SYSTEM OUT OF MEMORY
fix ideas, save line at a time and remove it from datatable after?
*/
