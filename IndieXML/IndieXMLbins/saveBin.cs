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
    class SaveBin : IPlug
    {
        public string Name { get { return "saveBin"; } } // name works as a keyvalue in dictionary, must be unique!
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

        // same as in loadbin class.
        private void CreateMenuItem()
        {
            try
            {
                Menu m = new Menu();
                MenuItem mi = new MenuItem(), mi2 = null;
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

                                        if (((MenuItem)mi.Items[y]).Name == "miSave")
                                        {
                                            mi2 = ((MenuItem)mi.Items[y]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                MenuItem saveFile = new MenuItem { Header = "_Save Binary" };
                saveFile.Click += SaveFileEvent;
                if (mi2 != null)
                    mi2.Items.Add(saveFile);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        // serialize dataset to binary file.
        private void SaveFile()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Binary Files (*.bin)|*.bin" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // This is important to change, otherwise writing out larger files will cause it to fail.
                    // This should be defaulted to xml format in our case.
                    MainWindow.DSTables.RemotingFormat = SerializationFormat.Binary;

                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, MainWindow.DSTables);
                        fs.Close();
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

        // this event gets called when that export xml button is pressed
        void SaveFileEvent(object sender, RoutedEventArgs e)
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
    }
}