using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        // Make all content that is intended to be changed from a plugin an property with its own get and set
        private static DataSet dsTables = new DataSet();
        private static TreeView trView;
        private static StackPanel topNav;
        private static StackPanel botNav;
        private static DataGrid dgmain;
        private static TextBox tbName;

        public static string TBName
        {
            get
            {
                return tbName.Text;
            }
            set
            {
                tbName.Text = value;
            }
        }
        public static DataGrid DGMain
        {
            get { return dgmain; }
        }
        public static StackPanel BotNav
        {
            get { return botNav; }
        }
        public static StackPanel TopNav
        {
            get { return topNav; }
        }
        public static DataSet DSTables
        {
            get
            {
                return dsTables;
            }
            set
            {
                dsTables = value;
            }
        }
        public static TreeView TrView
        {
            get { return trView; }
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
        
        // called when the program starts
        public void OnStart ()
        {
            try
            {
                // careful when ordering these, set UI elements first, initialize grid, then create new plugincore
                tbName = txbDatabaseName;
                botNav = spBotNav;
                topNav = spTopNav;
                trView = trvTables;
                dgmain = dgMainView;
                dgMainView.LoadingRow += DataGrid_LoadingRow;
                InitDataGrid();
                PluginCore pluginCore = new PluginCore();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // event method to add rownumbers for our datagrid
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

        // this redoes the treeview items based on our dataset datacontext.
        // it is called everytime datacontext changes. it adds parenttable from
        // every relation to treeview
        private void DGMainView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                trView.Items.Clear();
                foreach (DataRelation rel in DSTables.Relations)
                {
                    trView.Items.Add(rel.ParentTable.ToString());
                    trView.Padding = new Thickness { Right = 10 };
                }
                cmbColumns.DataContext = ((DataTable)dgmain.DataContext);
                cmbColumns.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // loads in a default.xml when the program starts
        private void InitDataGrid()
        {
            try
            {
                DSTables.ReadXml("default.xml");
                dgMainView.DataContext = DSTables.Relations[0].ChildTable;
                TBName = DSTables.DataSetName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // set datagrid datacontext to match the item selected in the treeview item
        private void TRVTables_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                for (int i = 0; i < trView.Items.Count; i++)
                {
                    if (trView.Items[i] == ((TreeView)sender).SelectedItem)
                    {
                        dgMainView.DataContext = DSTables.Relations[i].ChildTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // start new instance of IndieXML if new is pressed
        // this looks pretty crude... research more...
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            p.Start();
        }

        // check if autogenerated column is the key and cancel it 
        // this is used to prevent auto generated primary key columns to show in datagrid
        private void DGMainView_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            foreach (DataTable dt in DSTables.Tables)
            {
                foreach (DataColumn dc in dt.PrimaryKey)
                {
                    if (e.Column.Header.ToString() == dc.ColumnName)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        // this even is used when treeview rightclick is implemented
        private void TRVTables_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("treeview rightclic Valikko");
        }

    }
}
