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
    class LoadBin : IPlug
    {
        public string Name { get { return "loadBin"; } } // name works as a keyvalue in dictionary, must be unique!
        public void Update() // method that gets called in pluginmanager.
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

        // Push menuitem to mainwindow file menu
        private void CreateMenuItem ()
        {
            try
            {
                Menu m = new Menu();
                MenuItem mi = new MenuItem(), mi2 = null;

                // find the file menu from top navigation children
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

                MenuItem loadFile = new MenuItem { Header = "_Load Binary" };
                loadFile.Click += LoadFileEvent;
                if (mi2 != null)
                    mi2.Items.Add(loadFile);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // load file and deserialize it into a dataset so we can display it in a datagrid.
        private void LoadFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Binary Files (*.bin)|*.bin" };
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MainWindow.DSTables = (DataSet)bf.Deserialize(fs);
                        fs.Close();
                        MainWindow.DGMain.Columns.Clear();
                        MainWindow.DGMain.DataContext = MainWindow.DSTables.Relations[0].ChildTable;
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

        // event that gets called when load bin is pressed
        void LoadFileEvent(object sender, RoutedEventArgs e)
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
    }
}