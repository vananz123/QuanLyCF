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
using System.Data.Entity;
using DevExpress.Utils.Svg;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;
using GUI.Reports;
using DevExpress.XtraReports.UI;

namespace GUI
{
    public partial class FrmOrder : DevExpress.XtraEditors.XtraForm
    {
        public FrmOrder()
        {
            InitializeComponent();
        }
        DataTable tablefood = new DataTable("tablefood");
        List<int> listIdTable=new List<int>();
        int IdTable = 0;
        SimpleButton TableBtn(TableCoffee i)
        {
            SimpleButton btn = new SimpleButton();
            btn.Size = new Size(150, 150);
            btn.Text ="Bàn "+ i.ID.ToString();
            btn.Click += TableBtn_Click;
            if (i.Status == "Đã có")
            {
                btn.Appearance.BackColor =Color.Red;
                btn.Enabled = false;
            }
            return btn;
        }
        void loadData()
        {
            LoadTable();
            cbCatagory.Properties.Items.Add("Tất cả");

            foreach (var i in CategoryBUS.GetAll())
            {
                cbCatagory.Properties.Items.Add(i.Name);
            }
            gridControlAddFood.DataSource = FoodBUS.GetAll();
            
            gridControlBIllA.DataSource =BillBUS.GetAll().Where(p => p.Status == 0).Select(s => s).ToList();
            gridControlBillInfo.DataSource = tablefood;
            LoadComboBoxChuyenBan();
        }
        public void LoadTable()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var i in TableBUS.GetAll())
            {
                flowLayoutPanel1.Controls.Add(TableBtn(i));
            }

        }
        public void LoadComboBoxChuyenBan()
        {
            var ff = (from a in TableBUS.GetAll()
                      join b in BillBUS.GetAll()
                      on a.ID equals b.TableID
                      where b.Status == 0 && a.Status == "Đã có"
                      select new
                      {
                          IDBill = b.ID,
                          TableName = a.ID
                      }).ToList();
            cbChuyenBan.DataSource = ff;
            cbChuyenBan.DisplayMember = "TableName";
            cbChuyenBan.ValueMember = "IDBill";
        }
        bool CheckTableFood(int id)
        {
            foreach (DataRow dr in tablefood.Rows)
            {
                if (dr["ID"].ToString() == id.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        void RefreshData()
        {
            LoadTable();
            gridControlBIllA.DataSource = BillBUS.GetAll().Where(p => p.Status == 0).Select(s => s).ToList();
            gridControlBillInfo.DataSource = tablefood;
            LoadComboBoxChuyenBan();
            txtTotalPrice.Text = "";
        }
        private void FrmOrder_Load(object sender, EventArgs e)
        {
            DataColumn col0 = tablefood.Columns.Add("ID", typeof(int));
            DataColumn col1 = tablefood.Columns.Add("Name", typeof(string));
            DataColumn col2 = tablefood.Columns.Add("Size", typeof(string));
            DataColumn col3 = tablefood.Columns.Add("Amount", typeof(int));
            DataColumn col4 = tablefood.Columns.Add("Price", typeof(int));
            loadData();
        }
        public void TableBtn_Click(object sender,EventArgs  args)
        {
            SimpleButton btn = (SimpleButton)sender;
            gcBillPay.Text = "Hóa đơn: " + btn.Text;
            string[] arrStr = btn.Text.Split(' ');
            IdTable = Int32.Parse(arrStr[1]);
            btnPay.Enabled = true;
            btnPayAfter.Enabled = true;
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridViewAddFood.RowCount; i++)
            {
                var food = (DAO.Food)gridViewAddFood.GetRow(i);
                if (food != null)
                {
                    if (gridViewAddFood.IsRowSelected(i) == true)
                    {
                        if (CheckTableFood(food.ID) == true)
                        {
                            foreach (DataRow dr in tablefood.Rows)
                            {
                                string id = dr["ID"].ToString();
                                if ( id== food.ID.ToString())
                                {
                                    int amount = Int32.Parse(dr["Amount"].ToString())+ Int32.Parse(cbAmount.Text);
                                    
                                    dr["Amount"] = amount;
                                    dr["Price"] = amount * FoodBUS.GetById(Int32.Parse(id)).Price;
                                }
                            }
                        }
                        else
                        {
                            DataRow dr = tablefood.NewRow();
                            dr["ID"] = food.ID;
                            dr["Name"] = food.Name;
                            dr["Size"] = food.Size;
                            dr["Amount"] =Int32.Parse(cbAmount.Text);
                            dr["Price"] = food.Price * Int32.Parse(cbAmount.Text);
                            tablefood.Rows.Add(dr);
                        }
                    }
                }
            }
            int kt = cbCatagory.SelectedIndex;
            ReloadFoodAdd(kt);
            gridControlBillInfo.RefreshDataSource();
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00} VNĐ", TotalPrice());
        }
        void ReloadFoodAdd(int kt)
        {
            if (kt > 0)
            {
                var list = FoodBUS.GetFoodByIdCategory(kt);
                gridControlAddFood.DataSource = list.ToList();
            }
            else
            {
                gridControlAddFood.DataSource = FoodBUS.GetAll();
            }
        }
        private void cbCatagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int kt = cbCatagory.SelectedIndex;
            ReloadFoodAdd(kt);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DataRow data = gridViewBillInfo.GetFocusedDataRow();
            List<DataRow> toDelete = new List<DataRow>();
            foreach (DataRow dr in tablefood.Rows)
            {
                if (data["ID"].ToString() == dr["ID"].ToString())
                {
                    toDelete.Add(dr);
                }
            }
            foreach (DataRow dr in toDelete)
            {
                tablefood.Rows.Remove(dr);
            }
            gridControlBillInfo.RefreshDataSource();
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00} VNĐ", TotalPrice());

        }
        private void repositoryItemSpinEdit1_ValueChanged(object sender, EventArgs e)
        {
            SpinEdit spin = (SpinEdit)sender;
            DataRow dr = gridViewBillInfo.GetFocusedDataRow();
            int ID = Int32.Parse(dr["ID"].ToString());
            int soLuong = Int32.Parse(spin.Text.TrimEnd('.'));
            int priceS = FoodBUS.GetById(ID).Price;
            int price = soLuong * priceS;
            System.Console.WriteLine(spin.Text.TrimEnd('.'));
            foreach (DataRow d in tablefood.Rows)
            {
                if (d["ID"].ToString() == ID.ToString())
                {
                    d["Amount"] = soLuong;
                    d["Price"] = price.ToString();
                }
            }
            gridControlBillInfo.RefreshDataSource();
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00} VNĐ", TotalPrice());
        }
        int TotalPrice()
        {
            int totalprice = 0;
            foreach(DataRow dr in tablefood.Rows)
            {
                int price = Int32.Parse(dr["PRice"].ToString());
                totalprice += price;
            }
            return totalprice;
        }
        void Pay(int paystatus,int ab)//thanh toán truyền vào trạng thái thanh toán trước hoặc sau
        {
            if (IdTable > 0)
            {
                if (tablefood.Rows.Count > 0)
                {
                    //tạo bill
                    int billid = BillBUS.Pay(IdTable, TotalPrice(),paystatus, tablefood);
                    if (billid != 0)
                    {
                        listIdTable.Add(IdTable);
                        MessageBox.Show("Thánh toán thành công");
                        TableBUS.UpdateStaticTable(IdTable, true);
                        LoadTable();
                        tablefood.Rows.Clear();
                        RefreshData();
                        if (ab == 1)
                        {
                            FrmReportBill frm = new FrmReportBill(billid);
                            frm.ShowPreviewDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Thanh toán không thành công");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng thêm món vào hóa đơn");
                }
            }
        }
        private void btnPay_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn thanh toán ngay!", "Thanh toán!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Pay(1,1);
                RefreshData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
        private void btnPayAfter_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn thanh toán sau!", "Thanh toán!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Pay(0,0);
                RefreshData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void gridViewBillA_MasterRowEmpty(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowEmptyEventArgs e)
        {
            GridView gridView = sender as GridView;
            DAO.Bill bill = gridView.GetRow(e.RowHandle) as DAO.Bill;
            if(bill != null)
            {
                var billinfo = (from a in BillBUS.GetAllBillinfo()
                                join b in FoodBUS.GetAll() on a.FoodID equals b.ID
                                select new
                                {
                                    BillID =a.BillID,
                                    Name = b.Name,
                                    Price = b.Price,
                                    Size = b.Size,
                                    Amount = a.Amount

                                }).ToList();

                e.IsEmpty = !billinfo.Any(x => x.BillID == bill.ID);
            }
        }
        private void gridViewBillA_MasterRowGetChildList(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetChildListEventArgs e)
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

        private void gridViewBillA_MasterRowGetRelationCount(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 1;
        }

        private void gridViewBillA_MasterRowGetRelationName(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            e.RelationName = "Chi tiết";
        }

        private void repositoryItemBtnPayAfter_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn thanh toán ngay!", "Thanh toán!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Bill bill = (Bill)gridViewBillA.GetFocusedRow();
                bill.Status = 1;
                BillBUS.Update(bill);
                MessageBox.Show("Thanh toán thành công");
                FrmReportBill frm = new FrmReportBill(bill.ID);
                frm.ShowPreview();
                RefreshData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
        private void btnChuyenBan_Click(object sender, EventArgs e)
        {
            int idbill = Convert.ToInt32(cbChuyenBan.SelectedValue.ToString());
            string mess = "Bạn chắc chắn muốn chuyển đến bàn " + idbill.ToString();
            DialogResult dialogResult = MessageBox.Show(mess, "Chuyển bàn!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                BillBUS.ChuyenBan(idbill, TotalPrice(), tablefood);
                MessageBox.Show("Chuyển bàn thành công");
                tablefood.Rows.Clear();
                RefreshData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void btnPreviewFood_Click(object sender, EventArgs e)
        {
            FrmReportListFood frm = new FrmReportListFood(tablefood);
            frm.ShowPreview();
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshData();
        }
    }
}