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
using System.Windows.Input;

namespace IndieXMLadditions
{
    class DGRightClick : IPlug
    {
        ContextMenu cm = new ContextMenu();
        List<DataColumn> primarykeys = new List<DataColumn>();

        // name of our plugin, this will be passed down to assembly dictionary as a key
        public string Name
        {
            get
            {
                return "DGRightClick";
            }
        }

        public void Update()
        {
            try
            {
                DataGrid dg = MainWindow.DGMain;
                dg.MouseRightButtonUp += DgMainView_MouseRightButtonUp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // event to call the contextmenu when right mousebutton is pressed.
        private void DgMainView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // set primary keys up for comparison
                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    foreach (DataColumn dc in MainWindow.DSTables.Tables[i].PrimaryKey)
                    {
                        primarykeys.Add(dc);
                    }
                }

                // create buttons and open contextmenu
                CmItems();
                cm.IsOpen = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        TextBox txbColumnName;
        TextBox txbNewName;
        
        // contextmenu
        private void CmItems ()
        {
            try
            {
                cm.Items.Clear();

                // right click new column button
                MenuItem addColumn = new MenuItem { Header = "_Add Column" };
                // name of the new column
                txbColumnName = new TextBox { MinWidth = 55 };
                txbColumnName.LostFocus += AddColumnEvent;
                addColumn.Items.Add(txbColumnName);

                // right click delete columns buttons
                MenuItem delColumn = new MenuItem { Header = "_Delete Column" };

                // generate delete menu items based on the current datatable.
                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.DGMain.DataContext)
                    {
                        for (int ii = 0; ii < MainWindow.DSTables.Tables[i].Columns.Count; ii++)
                        {
                            // compare each column to primary keys, and don't make delete key for them.
                            bool isKey = false;
                            foreach (DataColumn dc in primarykeys)
                            {
                                if (dc.ColumnName == MainWindow.DSTables.Tables[i].Columns[ii].ColumnName)
                                {
                                    isKey = true;
                                }
                            }

                            // if column is not primary key, proceed and create the delete key.
                            if (isKey == false)
                            {
                                MenuItem mi = new MenuItem{ Header = "_Delete " + MainWindow.DSTables.Tables[i].Columns[ii].ColumnName };
                                MenuItem mi2 = new MenuItem { Header = "_Confirm"};
                                mi2.Click += CallDelete;
                                mi.Items.Add(mi2);
                                delColumn.Items.Add(mi);
                            }
                        }
                    }
                }

                // right click rename column
                MenuItem renameColumn = new MenuItem { Header = "_Rename Column" };

                // create rename menu items based on current datatable
                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.DGMain.DataContext)
                    {
                        for (int ii = 0; ii < MainWindow.DSTables.Tables[i].Columns.Count; ii++)
                        {
                            // again, prevent primary key to be changed
                            bool isKey = false;
                            foreach (DataColumn dc in primarykeys)
                            {
                                if (dc.ColumnName == MainWindow.DSTables.Tables[i].Columns[ii].ColumnName)
                                {
                                    isKey = true;
                                }
                            }

                            // create the menu item if not primary key.
                            if (isKey == false)
                            {
                                MenuItem mi = new MenuItem { Header = "_Rename " + MainWindow.DSTables.Tables[i].Columns[ii].ColumnName };

                                txbNewName = new TextBox { MinWidth = 55 };
                                txbNewName.LostFocus += RenameColumnEvent;
                                txbNewName.TextChanged += StoreNewName;

                                mi.Items.Add(txbNewName);
                                renameColumn.Items.Add(mi);
                            }
                        }
                    }
                }

                // finally add all the context menu items to the contextmenu.
                cm.Items.Add(addColumn);
                cm.Items.Add(delColumn);
                cm.Items.Add(renameColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // this event is called when the textbox under the add column looses it's focus.
        private void AddColumnEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbColumnName.Text.Length > 0)
                {
                    for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                    {
                        if (MainWindow.DSTables.Tables[i] == MainWindow.DGMain.DataContext)
                        {
                            DataColumn dc = new DataColumn { ColumnName = txbColumnName.Text };
                            MainWindow.DSTables.Tables[i].Columns.Add ( dc);
                            MainWindow.DSTables.Tables[i].Columns[MainWindow.DSTables.Tables[i].Columns.Count-1].SetOrdinal(MainWindow.DSTables.Tables[i].Columns.Count - 1);

                            MainWindow.DGMain.DataContext = null;
                            MainWindow.DGMain.DataContext = MainWindow.DSTables.Tables[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // store txbNewName.Text to a variable, for some reason it doesn't work quite right without it.
        string newName = "";
        private void StoreNewName(object sender, TextChangedEventArgs e)
        {
            newName = ((TextBox)sender).Text;
        }

        // this event is called when the textbox under the rename column looses it's focus. lostfocus is a bad idea, change it.
        // (some reason text length check did not work here last i tried)
        private void RenameColumnEvent(object sender, RoutedEventArgs e)
        {
            try
            {

                DataTable temp = new DataTable();

                // Get the name of the column we wish to change.
                string s = ((MenuItem)((TextBox)sender).Parent).Header.ToString();
                string[] split;
                split = s.Split(' ');

                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.DGMain.DataContext)
                    {
                        temp = MainWindow.DSTables.Tables[i];
                    }
                }

                for (int i = 0; i < temp.Columns.Count; i++)
                {
                    if (split[1] == temp.Columns[i].ColumnName)
                    {
                        temp.Columns[split[1]].ColumnName = newName;
                    }
                }

                newName = "";
                MainWindow.DGMain.DataContext = null;
                MainWindow.DGMain.DataContext = temp;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // this event gets called when the confirm button under delete column menuitem is pressed
        private void CallDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = ((MenuItem)((MenuItem)sender).Parent).Header.ToString();
                string[] split;
                split = s.Split(' ');
                
                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.DGMain.DataContext)
                    {
                        for (int ii = 0; ii < MainWindow.DSTables.Tables[i].Columns.Count; ii++)
                        {
                            // check if the this column is the column we want to delete
                            if (split[1] == MainWindow.DSTables.Tables[i].Columns[ii].ColumnName)
                            {
                                MainWindow.DSTables.Tables[i].Columns.Remove(MainWindow.DSTables.Tables[i].Columns[ii]);
                            }
                        }

                        MainWindow.DGMain.DataContext = null;
                        MainWindow.DGMain.DataContext = MainWindow.DSTables.Tables[i];
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
