using IndieXMLIPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public string Name
        {
            get
            {
                return "Core";
            }
        }
        #endregion

        #region METHODS
        public void Update(StackPanel TopNav, StackPanel BotNav, StackPanel MainContent)
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

                // Call methdods that only need one call.
                CreateDataGrid(MainContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateDataGrid(StackPanel MainContent)
        {
            try
            {
                // create datagrid and set in few columns and rows (default for new file)
                DataTable dt = new DataTable();
                dt.Columns.Add("Column 1");
                dt.Columns.Add("Column 2");
                dt.Columns.Add("Column 3");
                dt.Rows.Add();
                dt.Rows.Add();
                dt.Rows.Add();

                DataGrid dg = new DataGrid();
                dg.Name = "dgMainView";
                dg.LoadingRow += DataGrid_LoadingRow;
                dg.ItemsSource = dt.AsDataView();

                MainContent.Children.Add(dg);

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

        #endregion

    }
}
