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
using GUI.Reports;
using DevExpress.XtraReports.UI;

namespace GUI
{
    public partial class FrmCategory : DevExpress.XtraEditors.XtraForm
    {
        public FrmCategory()
        {
            InitializeComponent();
            LoadData();
        }
        List<CategoryFood> listrp;
        void LoadData()
        {
            gridControlCategory.DataSource = CategoryBUS.GetAll();
            listrp = CategoryBUS.GetAll();

        }
        void AddBinding()
        {
            txtName.DataBindings.Add(new Binding("Text", gridControlCategory.DataSource, "Name"));
        }
        void ClearBingding()
        {
            txtName.DataBindings.Clear();
        }
        void BtnHide(bool kt)//true la mo add,edit,del, f = tat
        {
            txtName.Enabled = !kt;
            barBtnAdd.Enabled = kt;
            barBtnDelete.Enabled = kt;
            barBtnEdit.Enabled = kt;
            barBtnSave.Enabled = !kt;
            barBtnCancel.Enabled = !kt;

        }
        void EditGrid(bool kt)//true la mo chon nhien dong , f la tat
        {
            gridViewCategory.OptionsSelection.MultiSelect = kt;
            if (kt == true)
            {
                gridViewCategory.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
            else
            {
                gridViewCategory.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            }

        }
        bool CheckName(List<DAO.CategoryFood> list, string name)
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
        bool ADD = false;
        bool DEL = false;
        bool EDIT = false;
        private void FrmCategory_Load(object sender, EventArgs e)
        {
            BtnHide(true);
            txtName.Enabled = false;
        }

        

        private void barBtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EDIT = true;
            AddBinding();
            BtnHide(false);

        }

        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ADD = true;
            txtName.Text = "";
            BtnHide(false);
        }

        private void barBtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DEL = true;
            EditGrid(DEL);
            BtnHide(false);
        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DEL == true)
            {
                var res = MessageBox.Show("Bạn chắc chắn chứ, sẽ xóa tất cả sản phẩm thuộc danh mục này!", "Warning", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    for (int i = 0; i < gridViewCategory.RowCount; i++)
                    {
                        var r = (CategoryFood)gridViewCategory.GetRow(i);
                        if (r != null)
                        {
                            if (gridViewCategory.IsRowSelected(i) == true)
                            {
                                List<Food> listFoodOnCategoty= FoodBUS.GetAll().Where(p => p.CategoryID == r.ID).ToList();
                                foreach(var f in listFoodOnCategoty)
                                {
                                    FoodBUS.Delete(f);
                                }
                                CategoryBUS.Delete(r);
                            }
                        }
                    }
                }
                else if (res == DialogResult.No)
                {
                    
                }
                else//cancel
                {
                    DEL = false;
                    EditGrid(DEL);
                }
                
                DEL = false;
                EditGrid(DEL);
            }
            if (ADD == true)
            {
                string name = txtName.Text;
                try
                {
                    if (CheckName(CategoryBUS.GetAll(), name) == true)
                    {
                        MessageBox.Show("Tên đã tồn tại");
                    }
                    else
                    {
                        DAO.CategoryFood categoryFood = new DAO.CategoryFood() { Name = name };
                        CategoryBUS.Insert(categoryFood);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("lỗi");
                }
                ADD = false;
            }
            if (EDIT == true)
            {

                try
                {
                    string name = txtName.Text;
                    var i = (DAO.CategoryFood)gridViewCategory.GetRow(gridViewCategory.GetFocusedDataSourceRowIndex());
                    i.Name = name;
                    CategoryBUS.Update(i);
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
            ClearBingding();
            DEL = false;
            ADD = false;
            EDIT = false;
            EditGrid(DEL);
            BtnHide(true);
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControlCategory.DataSource = CategoryBUS.GetAll();
            listrp = CategoryBUS.GetAll();
        }

        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmReportCategory frm = new FrmReportCategory(listrp);
            frm.ShowRibbonPreview();
        }
    }
}