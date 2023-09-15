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
using DevExpress.XtraReports.UI;
using GUI.Reports;

namespace GUI
{
    public partial class FrmFood : DevExpress.XtraEditors.XtraForm
    {
        public FrmFood()
        {
            InitializeComponent();
            LoadData();
            
        }
        List<Food> listrp;
        bool ADD = false;
        bool DEL = false;
        bool EDIT = false;
        void LoadData()
        {
            gridControlFood.DataSource = FoodBUS.GetAll();
            listrp = FoodBUS.GetAll();
            lkCategoryE.Properties.DataSource = CategoryBUS.GetAll();
            lkCategoryE.Properties.DisplayMember = "Name";
            lkCategoryE.Properties.ValueMember = "ID";

            
            lkCategory.DataSource = CategoryBUS.GetAll();
            lkCategory.DisplayMember = "Name";
            lkCategory.ValueMember = "ID";

        }
        void AddBinding()
        {
            txtFood.DataBindings.Add(new Binding("Text", gridControlFood.DataSource, "Name"));
            cbSize.DataBindings.Add(new Binding("Text", gridControlFood.DataSource, "Size"));
            txtPrice.DataBindings.Add(new Binding("Text", gridControlFood.DataSource, "Price"));
            lkCategoryE.DataBindings.Add(new Binding("EditValue", gridControlFood.DataSource, "CategoryID"));
        }
        void ClearBingding()
        {
            txtFood.DataBindings.Clear();
            cbSize.DataBindings.Clear();
            txtPrice.DataBindings.Clear();
            lkCategoryE.DataBindings.Clear();
        }
        void BtnHide(bool kt)//true là enble nút ADD ,DEL ,EDIT , Fasle là tắt
        {
            txtFood.Enabled = !kt;
            txtPrice.Enabled = !kt;
            cbSize.Enabled = !kt;
            lkCategoryE.Enabled = !kt;
            barBtnAdd.Enabled = kt;
            barBtnDel.Enabled = kt;
            barBtnEdit.Enabled = kt;
            barBtnSave.Enabled = !kt;
            barBtnCancel.Enabled = !kt;

        }
        void EditGrid(bool kt)//true là cho chọn nhiều hàng trong gridControl , f là tắt
        {
            gridViewFood.OptionsSelection.MultiSelect = kt;
            if (kt == true)
            {
                gridViewFood.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
            else
            {
                gridViewFood.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            }

        }
        bool CheckName(List<Food> list, string name) //đổi TableCoffee thành đối tượng tương ứng
        {
            if (list.Where(p => p.Name.Contains(name)).ToList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ADD = true;
            txtFood.Text = "";
            txtPrice.Text = "";
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

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (DEL == true)
            {
                for (int i = 0; i < gridViewFood.RowCount; i++)
                {
                    var r = (Food)gridViewFood.GetRow(i);
                    if (r != null)
                    {
                        if (gridViewFood.IsRowSelected(i) == true)
                        {
                            FoodBUS.Delete(r);
                        }
                    }
                }
                
                
                DEL = false;
                EditGrid(DEL);
            }
            if (ADD == true)
            {
                string name = txtFood.Text;
                string size = "";
                if(cbSize.SelectedIndex == 0)
                {
                    size = "";
                }
                else
                {
                    size = cbSize.Text;
                }
                int price = Int32.Parse(txtPrice.Text);
                int Category = Convert.ToInt32(lkCategoryE.GetColumnValue("ID").ToString());
                
                try
                {
                    if (CheckName(FoodBUS.GetAll(), name) == true)
                    {
                        MessageBox.Show("Tên đã tồn tại");
                    }
                    else
                    {
                        CategoryFood category = CategoryBUS.GetById(Category);
                        Food food = new Food() { Name = name, Size = size, Price = price, CategoryID = category.ID };
                        FoodBUS.Insert(food);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ADD = false;
            }
            if (EDIT == true)
            {

                try
                {
                    string name = txtFood.Text;
                    string size = cbSize.Text;
                    int price =Int32.Parse(txtPrice.Text);
                    int Category = Int32.Parse(lkCategoryE.GetColumnValue("ID").ToString());
                    var i = (Food)gridViewFood.GetRow(gridViewFood.GetFocusedDataSourceRowIndex());
                    i.Name = name;
                    i.Price = price;
                    i.Size = size;
                    i.CategoryID = Category;
                    FoodBUS.Update(i);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("lỗi");
                }
                EDIT = false;
            }
            BtnHide(true);
            ClearBingding();
            LoadData();

        }

        private void barBtnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ADD = false;
            EDIT = false;
            DEL = false;
            ClearBingding();
            EditGrid(DEL);
            BtnHide(true);
        }

        private void FrmFood_Load(object sender, EventArgs e)
        {
            BtnHide(true);
            txtFood.Enabled = false;
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControlFood.DataSource = FoodBUS.GetAll();
        }

        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmReportFood frm = new FrmReportFood();
            frm.ShowRibbonPreview();
        }
    }
}