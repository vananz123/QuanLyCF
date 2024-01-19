using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAO;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.Entity;
using System.Globalization;

namespace GUI
{
    public partial class FrmTable : DevExpress.XtraEditors.XtraForm
    {
        public FrmTable()
        {
            InitializeComponent();
            LoadData();
        }
        bool ADD = false;
        bool DEL = false;
        bool EDIT = false;
        void LoadData()
        {
            gridControlTable.DataSource = TableBUS.GetAll();
          
        }
        void AddBinding()
        {
            txtEditTable.DataBindings.Add(new Binding("Text", gridControlTable.DataSource, "Name"));
        }
        void ClearBingding()
        {
            txtEditTable.DataBindings.Clear();
        }
        void BtnHide(bool kt)//true là enble nút ADD ,DEL ,EDIT , Fasle là tắt
        {
            txtEditTable.Enabled = !kt;
            barBtnAdd.Enabled = kt;
            barBtnDel.Enabled = kt;
            barBtnEdit.Enabled = kt;
            barBtnSave.Enabled = !kt;
            barBtnCanel.Enabled = !kt;
            
        }
        void EditGrid(bool kt)//true là cho chọn nhiều hàng trong gridControl , f là tắt
        {
            gridViewTable.OptionsSelection.MultiSelect = kt;
            if(kt == true)
            {
                gridViewTable.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
            else
            {
                gridViewTable.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            }
            
        }
        bool CheckName(List<DAO.TableCoffee> list, string name) //đổi TableCoffee thành đối tượng tương ứng
        {
            if(list.Where(p => p.Name.ToLower()==name.ToLower()).ToList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FrmTable_Load(object sender, EventArgs e)
        {
            BtnHide(true);
            txtEditTable.Enabled = false;
        }
        

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            if (DEL == true)
            {
                for (int i = 0; i < gridViewTable.RowCount; i++)
                {
                    var r = (TableCoffee)gridViewTable.GetRow(i);
                    if(r != null)
                    {
                        if (gridViewTable.IsRowSelected(i) == true)
                        {
                            TableBUS.Delete(r);
                        }
                    }
                }
                DEL = false;
                EditGrid(DEL);
            }
            if(ADD == true)
            {
                string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtEditTable.Text.ToLower());
                try
                {
                    if (CheckName(TableBUS.GetAll(), name) == true)
                    {
                        MessageBox.Show("Tên đã tồn tại");
                    }
                    else
                    {
                        TableCoffee tableCoffee = new TableCoffee() { Name = name, Status = "Trống" };
                        TableBUS.Insert(tableCoffee);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("lỗi");
                }
                ADD = false;
            }
            if (EDIT == true)
            {
                
                try
                {
                    string name = txtEditTable.Text;
                    var i = (TableCoffee)gridViewTable.GetRow(gridViewTable.GetFocusedDataSourceRowIndex());
                    i.Name = name;
                    TableBUS.Update(i);
                }
                catch (Exception)
                {
                    MessageBox.Show("lỗi");
                }
                EDIT = false;
            }
            BtnHide(true);
            ClearBingding();
            LoadData();

        }

        private void barBtnCanel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ADD = false;
            EDIT = false;
            DEL = false;
            ClearBingding();
            EditGrid(DEL);
            BtnHide(true);
        }
        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ADD = true;
            txtEditTable.Text = "";
            BtnHide(false);
        }
        private void barBtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EDIT = true;
            AddBinding();
            BtnHide(false);

        }

        private void barBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DEL = true;
            EditGrid(DEL);
            BtnHide(false);
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControlTable.DataSource = TableBUS.GetAll();
        }
    }
}