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
    class saveBin : IPlug
    {
        DockPanel dpMain;
        StackPanel TopNav;
        public string Name { get { return "saveBin"; } } // name works as a keyvalue in dictionary, must be unique!
        public void Update(DockPanel dp) // method that gets called in pluginmanager.
        {
            try
            {
                dpMain = dp;
                SetProperties();
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

                MenuItem saveFile = new MenuItem();
                saveFile.Header = "_Save Binary";
                saveFile.Click += saveFileEvent;
                if (mi2 != null)
                    mi2.Items.Add(saveFile);
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
                    }
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
                saveFileDialog.Filter = "Binary Files (*.bin)|*.bin";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    MainWindow.dataset.RemotingFormat = SerializationFormat.Binary;

                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, MainWindow.dataset);
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
    }
}