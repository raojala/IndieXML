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

namespace IndieXMLbins
{
    class loadBin : IPlug
    {
        DataGrid dgMain;
        StackPanel TopNav;
        TreeView trView;
        public string Name { get { return "loadBin"; } } // name works as a keyvalue in dictionary, must be unique!
        public void Update() // method that gets called in pluginmanager.
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

        private void CreateMenuItem ()
        {
            try
            {
                Menu m = new Menu();
                MenuItem mi = new MenuItem(), mi2 = null;
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

                                        if (((MenuItem)mi.Items[y]).Name == "miLoad")
                                        {
                                            mi2 = ((MenuItem)mi.Items[y]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                MenuItem loadFile = new MenuItem();
                loadFile.Header = "_Load Binary";
                loadFile.Click += loadFileEvent;
                if (mi2 != null)
                    mi2.Items.Add(loadFile);
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
                trView = MainWindow.TrView;
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
                openFileDialog.Filter = "Binary Files (*.bin)|*.bin";
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MainWindow.DSTables = (DataSet)bf.Deserialize(fs);
                        fs.Close();
                        dgMain.Columns.Clear();
                        dgMain.DataContext = MainWindow.DSTables.Relations[0].ChildTable;
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

        void loadFileEvent(object sender, RoutedEventArgs e)
        {
            LoadFile();
        }
    }
}