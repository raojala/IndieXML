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

        DataSet ds;

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
                ds = new DataSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitDataGrid()
        {
            try
            {
                // tee tyhjä default dataset.


                //ds.Tables.Add (new DataTable());
                //ds.Tables[0].TableName = "New Table";
                //ds.Tables[0].Columns.Add("Col 1");
                //ds.Tables[0].Columns.Add("Col 2");
                //ds.Tables[0].Columns.Add("Col 3");
                //dgMain.ItemsSource = ds.Tables[0].DefaultView;
                dgMain.CellEditEnding += SetNewLastRow;
            }
            catch (Exception)
            {
                throw;
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
                            dgMain.LoadingRow += DataGrid_LoadingRow;
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

                MenuItem setXML = new MenuItem();
                setXML.Header = "_Export XML";
                setXML.Click += ExportXMLEvent;
                file.Items.Add(setXML);

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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ImportXml()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    DataSet ds = new DataSet();
                    DataView dv;
                    // file.Source = new Uri(openFileDialog.FileName);
                    debugTime = DateTime.Now;
                    ds.ReadXml(openFileDialog.FileName);
                    dv = ds.Tables[0].DefaultView;
                    dgMain.Columns.Clear();
                    dgMain.ItemsSource = dv;
                    System.Windows.MessageBox.Show((DateTime.Now - debugTime).ToString());
                }
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
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    DataTable dt = ((DataView)dgMain.ItemsSource).ToTable();
                    debugTime = DateTime.Now;
                    dt.WriteXml(filePath);
                    MessageBox.Show((DateTime.Now - debugTime).ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveFile()
        {
            
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    DataTable dt = ((DataView)dgMain.ItemsSource).ToTable();
                    dt.RemotingFormat = SerializationFormat.Binary;

                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        debugTime = DateTime.Now;
                        bf.Serialize(fs, dt);
                        fs.Close();
                        MessageBox.Show((DateTime.Now - debugTime).ToString());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadFile()
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = "";
                    filePath = openFileDialog.FileName;
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        debugTime = DateTime.Now;
                        DataTable dt = (DataTable)bf.Deserialize(fs);
                        fs.Close();
                        MessageBox.Show((DateTime.Now - debugTime).ToString());
                        dgMain.Columns.Clear();
                        dgMain.ItemsSource = dt.DefaultView;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
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

        // event handler for import/eport xml button
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

        // even to listen save and load button clicks
        void saveFileEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFile();
            }
            catch (System.OutOfMemoryException oex)
            {
                throw oex;
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

/*
(x64 build)
xml size: 349363KB
xml import time: 1:11.0310633
save bin time: 14.2248283
bin size: 137764KB
bin open time: 15.6126209

Rows: 1618559
*/