using IndieXML;
using IndieXMLIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace IndieXMLadditions
{
    class DGRightClick : IPlug
    {
        ContextMenu cm = new ContextMenu();
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

                foreach (DataGridColumn dgc in dg.Columns)
                {
                    //dgc.
                }

                dg.MouseRightButtonUp += dgMainView_MouseRightButtonUp;

                cmItems();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cmItems ()
        {
            try
            {
                MenuItem addColumn = new MenuItem();
                addColumn.Header = "Add _Column";

                cm.Items.Add(addColumn);
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
                //cm.IsOpen = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
