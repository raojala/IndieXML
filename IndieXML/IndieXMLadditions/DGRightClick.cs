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
                DataGrid dg = MainWindow.dgMain;
                dg.MouseRightButtonUp += dgMainView_MouseRightButtonUp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        TextBox txbColumnName;
        TextBox txbNewName;
        private void cmItems ()
        {
            try
            {
                cm.Items.Clear();

                // right click new column button
                MenuItem addColumn = new MenuItem();
                addColumn.Header = "_Add Column";
                //addColumn.Click += addColumnEvent;

                txbColumnName = new TextBox();
                txbColumnName.LostFocus += addColumnEvent;
                txbColumnName.MinWidth = 55;

                addColumn.Items.Add(txbColumnName);

                // right click delete columns buttons
                MenuItem delColumn = new MenuItem();
                delColumn.Header = "_Delete Column";
                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.dgMain.DataContext)
                    {
                        for (int ii = 0; ii < MainWindow.DSTables.Tables[i].Columns.Count; ii++)
                        {
                            bool isKey = false;
                            foreach (DataColumn dc in primarykeys)
                            {
                                if (dc.ColumnName == MainWindow.DSTables.Tables[i].Columns[ii].ColumnName)
                                {
                                    isKey = true;
                                }
                            }

                            if (isKey == false)
                            {
                                MenuItem mi = new MenuItem();
                                mi.Header = "_Delete " + MainWindow.DSTables.Tables[i].Columns[ii].ColumnName;
                                MenuItem mi2 = new MenuItem();
                                mi2.Header = "Confirm";
                                mi2.Click += callDelete;
                                mi.Items.Add(mi2);
                                delColumn.Items.Add(mi);
                            }
                        }
                    }
                }

                // right click rename column
                MenuItem renameColumn = new MenuItem();
                renameColumn.Header = "_Rename Column";
                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.dgMain.DataContext)
                    {
                        for (int ii = 0; ii < MainWindow.DSTables.Tables[i].Columns.Count; ii++)
                        {
                            bool isKey = false;
                            foreach (DataColumn dc in primarykeys)
                            {
                                if (dc.ColumnName == MainWindow.DSTables.Tables[i].Columns[ii].ColumnName)
                                {
                                    isKey = true;
                                }
                            }

                            if (isKey == false)
                            {
                                MenuItem mi = new MenuItem();
                                mi.Header = "_Rename " + MainWindow.DSTables.Tables[i].Columns[ii].ColumnName;

                                txbNewName = new TextBox();
                                txbNewName.MinWidth = 55;
                                txbNewName.LostFocus += renameColumnEvent;

                                mi.Items.Add(txbNewName);
                                renameColumn.Items.Add(mi);
                            }
                        }
                    }
                }

                cm.Items.Add(addColumn);
                cm.Items.Add(delColumn);
                cm.Items.Add(renameColumn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgMainView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

                cmItems();
                cm.IsOpen = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void addColumnEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txbColumnName.Text.Length > 0)
                {
                    for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                    {
                        if (MainWindow.DSTables.Tables[i] == MainWindow.dgMain.DataContext)
                        {
                            DataColumn dc = new DataColumn();
                            dc.ColumnName = txbColumnName.Text;
                            MainWindow.DSTables.Tables[i].Columns.Add ( dc);
                            MainWindow.DSTables.Tables[i].Columns[MainWindow.DSTables.Tables[i].Columns.Count-1].SetOrdinal(MainWindow.DSTables.Tables[i].Columns.Count - 1);

                            MainWindow.dgMain.DataContext = null;
                            MainWindow.dgMain.DataContext = MainWindow.DSTables.Tables[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void renameColumnEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable temp = new DataTable();

                string s = ((MenuItem)((TextBox)sender).Parent).Header.ToString();
                string[] split;
                split = s.Split(' ');

                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.dgMain.DataContext)
                    {
                        temp = MainWindow.DSTables.Tables[i];
                    }
                }

                MessageBox.Show(txbNewName.Text.Length.ToString());


                for (int iy = 0; iy < temp.Columns.Count; iy++)
                {
                    if (split[1] == temp.Columns[iy].ColumnName)
                    {
                        MessageBox.Show("muuttuu");
                    }
                }

                MainWindow.dgMain.DataContext = null;
                MainWindow.dgMain.DataContext = temp;


                if (txbNewName.Text.Length > 0)
                {
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void callDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = ((MenuItem)((MenuItem)sender).Parent).Header.ToString();
                string[] split;
                split = s.Split(' ');

                for (int i = 0; i < MainWindow.DSTables.Tables.Count; i++)
                {
                    if (MainWindow.DSTables.Tables[i] == MainWindow.dgMain.DataContext)
                    {
                        for (int ii = 0; ii < MainWindow.DSTables.Tables[i].Columns.Count; ii++)
                        {
                            if (split[1] == MainWindow.DSTables.Tables[i].Columns[ii].ColumnName)
                            {
                                MainWindow.DSTables.Tables[i].Columns.Remove(MainWindow.DSTables.Tables[i].Columns[ii]);
                            }
                        }

                        MainWindow.dgMain.DataContext = null;
                        MainWindow.dgMain.DataContext = MainWindow.DSTables.Tables[i];
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
