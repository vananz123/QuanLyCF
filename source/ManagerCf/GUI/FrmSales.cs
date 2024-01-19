using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
using DevExpress.XtraReports.UI;
using GUI.Reports;

namespace GUI
{
    public partial class FrmSales : DevExpress.XtraEditors.XtraForm
    {
        public FrmSales()
        {
            InitializeComponent();
        }
        bool DEL = false;
        private void gridViewMaster_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            GridView gridView = sender as GridView;
            DAO.Bill bill = gridView.GetRow(e.RowHandle) as DAO.Bill;
            if (bill != null)
            {
                var billinfo = (from a in BillBUS.GetAllBillinfo()
                                join b in FoodBUS.GetAll() on a.FoodID equals b.ID
                                select new
                                {
                                    BillID = a.BillID,
                                    Name = b.Name,
                                    Price = b.Price,
                                    Size = b.Size,
                                    Amount = a.Amount

                                }).ToList();

                e.IsEmpty = !billinfo.Any(x => x.BillID == bill.ID);
            }
        }

        private void gridViewMaster_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
        {
            GridView gridView = sender as GridView;
            DAO.Bill bill = gridView.GetRow(e.RowHandle) as DAO.Bill;
            if (bill != null)
            {
                var billinfo = (from a in BillBUS.GetAllBillinfo()
                                join b in FoodBUS.GetAll() on a.FoodID equals b.ID
                                select new
                                {
                                    BillID = a.BillID,
                                    Name = b.Name,
                                    Price = b.Price,
                                    Size = b.Size,
                                    Amount = a.Amount

                                }).ToList();

                e.ChildList = billinfo.Where(x => x.BillID == bill.ID).ToList();
            }
        }

        private void gridViewMaster_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gridViewMaster_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Chi tiết";
        }

        private void FrmSales_Load(object sender, EventArgs e)
        {
            gridControlBill.DataSource = BillBUS.GetAll();
            BtnHide(true);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControlBill.DataSource = BillBUS.GetAll();
        }

        private void barBtnWatch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DateTime date1 = dateEdit1.DateTime;
            DateTime date2 = dateEdit2.DateTime;
            List<Bill> bill;
            if(date2.Year == 0001)
            {
                var w = from p in BillBUS.GetAll()
                        where p.AtCreate >= date1
                        select p;
                bill = w.ToList();
            }
            else
            {
                var w = from p in BillBUS.GetAll()
                        where p.AtCreate >= date1 && p.AtCreate <= date2
                        select p;
                bill = w.ToList();
            }
            gridControlBill.DataSource = bill;
        }

        private void dateEdit1_DateTimeChanged(object sender, EventArgs e)
        {
            DateEdit d = (DateEdit)sender;
            if(d.DateTime.Year != 0001)
            {
                barBtnWatch.Enabled = true;
            }
            else
            {
                barBtnWatch.Enabled = false;
            }
        }
        void BtnHide(bool kt)//true là enble nút ADD ,DEL ,EDIT , Fasle là tắt
        {
            barBtnDel.Enabled = kt;
            barBtnSave.Enabled = !kt;
            barBtnCannel.Enabled = !kt;

        }
        void EditGrid(bool kt)//true là cho chọn nhiều hàng trong gridControl , f là tắt
        {
            gridViewMaster.OptionsSelection.MultiSelect = kt;
            if (kt == true)
            {
                gridViewMaster.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
            else
            {
                gridViewMaster.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            }

        }
        private void barBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DEL = true;
            BtnHide(false);
            EditGrid(true);
        }

 

        private void barBtnCannel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DEL = false;
            EditGrid(DEL);
            BtnHide(!DEL);
        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(DEL == true)
            {
                var res = MessageBox.Show("Bạn chắc chắn chứ!", "Warning", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    for (int i = 0; i < gridViewMaster.RowCount; i++)
                    {
                        var r = (Bill)gridViewMaster.GetRow(i);
                        if (r != null)
                        {
                            if (gridViewMaster.IsRowSelected(i) == true)
                            {
                                List<BillInfo> listBillInfoOnCategoty = BillBUS.GetAllBillinfo().Where(p => p.BillID == r.ID).ToList();
                                foreach (var f in listBillInfoOnCategoty)
                                {
                                    BillBUS.DeleteBillInfo(f);
                                }
                                BillBUS.Delete(r);
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
            }
            DEL = false;
            EditGrid(DEL);
            gridControlBill.DataSource=BillBUS.GetAll();
            
        }

        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DateTime date1 = dateEdit1.DateTime;
            DateTime date2 = dateEdit2.DateTime;
            List<Bill> bill =BillBUS.GetAll() ;
            if (date2.Year == 0001)
            {
                var w = from p in BillBUS.GetAll()
                        where p.AtCreate >= date1
                        select p;
                bill = w.ToList();
            }
            else
            {
                var w = from p in BillBUS.GetAll()
                        where p.AtCreate >= date1 && p.AtCreate <= date2
                        select p;
                bill = w.ToList();
            }
            FrmReportSales frm = new FrmReportSales(bill);
            frm.ShowPreview();
        }
    }
}