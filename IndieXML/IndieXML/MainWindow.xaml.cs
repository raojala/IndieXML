using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IndieXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static CustomDataSet ds = new CustomDataSet();

        static public DataSet dataset
        {
            get
            {
                return ds;
            }
            set
            {
                ds = (CustomDataSet)value;
            }
        }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                OnStart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public void OnStart ()
        {
            try
            {
                dgMainView.LoadingRow += DataGrid_LoadingRow;
                ds = new CustomDataSet();
                InitDataGrid(ds);
                PluginCore pluginCore = new PluginCore(dpMain, ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // event method to add rownumbers
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

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

        private void dgMainView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //MessageBox.Show("kappa");
        }

        private void InitDataGrid(DataSet data)
        {
            try
            {
                trvTables.Items.Add(data.Relations[0].ParentTable.ToString());
                trvTables.Items.Add(data.Relations[1].ParentTable.ToString());
                trvTables.Padding = new Thickness { Right = 10 };

                dgMainView.DataContext = data.Relations[0].ChildTable.DefaultView;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void trvTables_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                for (int i = 0; i < trvTables.Items.Count; i++)
                {
                    if (trvTables.Items[i] == ((TreeView)sender).SelectedItem)
                    {
                        dgMainView.DataContext = ds.Relations[i].ChildTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void miNew_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            p.Start();
        }
    }
}
