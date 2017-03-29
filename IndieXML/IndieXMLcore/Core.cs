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

        /*  
            XML tiedoston koko: 12 620KB
            importtaus aika: 2.2450556 sekuntia
            save aika (bin): 1.1165021 sekuntia
            Binin koko: 17 852KB
            load aika: 0.6721342
         */
        private void SaveFile()
        {
            FileStream fs = new FileStream(@"test.bin", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            debugTime = DateTime.Now;
            bf.Serialize(fs, ((DataView)dgMain.ItemsSource).ToTable());
            fs.Close();
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

        #endregion
    }
}