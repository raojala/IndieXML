using IndieXML;
using IndieXMLIPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace IndieXMLfromurl
{
    class getXMLfromurl : IPlug
    {
        TextBox tb;
        public string Name
        {
            get
            {
                return "GetXMLfromURL";
            }
        }

        public void Update()
        {
            CreateMenuItem();
        }

        private void CreateMenuItem()
        {
            try
            {
                Menu m = new Menu();
                MenuItem mi = new MenuItem();

                for (int i = 0; i < MainWindow.TopNav.Children.Count; i++)
                {
                    if (MainWindow.TopNav.Children[i].GetType() == typeof(Menu))
                    {
                        m = (Menu)MainWindow.TopNav.Children[i];
                        if (m.Name == "FileMenu")
                        {
                            for (int x = 0; x < m.Items.Count; x++)
                            {
                                if (((MenuItem)m.Items[x]).Name == "miFile")
                                {
                                    mi = ((MenuItem)m.Items[x]);
                                }
                            }
                        }
                    }
                }

                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                tb = new TextBox();
                tb.Width = 55;
                Button btnOk = new Button();
                btnOk.Content = "Ok!";
                btnOk.Click += xmlfromurl;

                sp.Children.Add(tb);
                sp.Children.Add(btnOk);

                MenuItem getXML = new MenuItem();
                getXML.Header = "Import XML from _Url";

                getXML.Items.Add(sp);
                
                //getXML.Items.Add(tb);
                mi.Items.Insert(mi.Items.Count-1, getXML);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void xmlfromurl(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportXmlfromurl();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // not working
        private void ImportXmlfromurl()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(tb.Text);
                DataSet ds = MainWindow.DSTables;
                ds.Clear();
                ds.ReadXml(new XmlNodeReader(xml));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
