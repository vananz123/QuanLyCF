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
        DataTable tablefood = new DataTable("tablefood");//billinfo
        int IdTable = 0;//bàn được chọn
        bool Thanhtoan = true;// true là tạo mới  bill , false là cập nhật bill
        SimpleButton TableBtn(TableCoffee i)
        {
            SimpleButton btn = new SimpleButton();
            btn.Size = new Size(130, 130);
            btn.Text = i.Name;
            btn.LookAndFeel.SetSkinStyle("DevExpress Style");
            btn.Click += TableBtn_Click;
            if (i.Status == "Đã có")
            {
                btn.ImageOptions.ImageUri.Uri = "Actions_check";
                btn.Enabled = false;
            }
            return btn;
        }
        void loadData()
        {
            LoadTable();
            loadCategory();
            gridControlAddFood.DataSource = FoodBUS.GetAll();
            gridControlBIllA.DataSource =BillBUS.GetAll().Where(p => p.Status == 0).Select(s => s).ToList();
            gridControlBillInfo.DataSource = tablefood;
        }
        public void LoadTable()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var i in TableBUS.GetAll())
            {
                flowLayoutPanel1.Controls.Add(TableBtn(i));
            }

        }
        public void loadCategory()
        {
            List<CategoryFood> categoryFoods = CategoryBUS.GetAll();
            CategoryFood allcategory = new CategoryFood() { ID = 0, Name = "Tất cả" };
            categoryFoods.Insert(0, allcategory);
            lkCategory.Properties.DataSource = categoryFoods;
            lkCategory.Properties.DisplayMember = "Name";
            lkCategory.Properties.ValueMember = "ID";
            lkCategory.EditValue = 0;
        }
        public void LoadComboBoxChuyenBan()
        {
            List<TableCoffee> tt = TableBUS.GetAll().Where(p => p.Status == "Trống").Select(p => p).ToList();
            lkChuyenBan.Properties.DataSource = tt;
            lkChuyenBan.Properties.DisplayMember = "Name";
            lkChuyenBan.Properties.ValueMember = "ID";
        }
        bool CheckTableFood(int id)//kiểm tra food có bị trùng tong tablefood
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
        void RefreshData(bool need =true)
        {
            LoadTable();
            IdTable = 0;
            if(need == true)
            {
                gridControlBIllA.DataSource = BillBUS.GetAll().Where(p => p.Status == 0).Select(s => s).ToList();
            }
            tablefood.Rows.Clear();
            gridControlBillInfo.DataSource = tablefood;
            LoadComboBoxChuyenBan();
            btnPay.Enabled = false;
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", TotalPrice());
            txtChange.Text = "0";
            txtCustomerPay.Text = "";
            lbTableBill.Text = "Hãy chọn bàn";
            Thanhtoan = true;
            BtnHide(false);
        }
        private void FrmOrder_Load(object sender, EventArgs e)
        {
            
            DataColumn col0 = tablefood.Columns.Add("ID", typeof(int));
            DataColumn col1 = tablefood.Columns.Add("Name", typeof(string));
            DataColumn col2 = tablefood.Columns.Add("Size", typeof(string));
            DataColumn col3 = tablefood.Columns.Add("Amount", typeof(int));
            DataColumn col4 = tablefood.Columns.Add("Price", typeof(int));
            tablefood.RowDeleted += new DataRowChangeEventHandler(Row_Deleted);
            tablefood.TableNewRow += new DataTableNewRowEventHandler(Table_NewRow);
            tablefood.RowChanged += new DataRowChangeEventHandler(Row_Changed);
            loadData();
            RefreshData();
        }
        private void Row_Changed(object sender, DataRowChangeEventArgs e)
        {
            gridControlBillInfo.RefreshDataSource();
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", TotalPrice());
        }
        private void Row_Deleted(object sender, DataRowChangeEventArgs e)
        {
            if (tablefood.Rows.Count == 0)
            {
                txtCustomerPay.Text = "";
                btnChuyenBan.Enabled = false;
                btnPreviewFood.Enabled = false;
                BtnHide(false);
            }
            gridControlBillInfo.RefreshDataSource();
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", TotalPrice());
        }
        private void Table_NewRow(object sender,DataTableNewRowEventArgs e)
        {
            if(tablefood.Rows.Count == 0)
            {
                txtCustomerPay.Text = "";
                btnChuyenBan.Enabled = true;
                btnPreviewFood.Enabled = true;
            }
            if(Thanhtoan == false && tablefood.Rows.Count == 0)
            {
                IdTable = 0;
                lbTableBill.Text = "Hãy chọn bàn";
            }
            BtnHide(true);
            gridControlBillInfo.RefreshDataSource();
            txtTotalPrice.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}" , TotalPrice());
        }
        public void TableBtn_Click(object sender,EventArgs  args)
        {
            SimpleButton btn = (SimpleButton)sender;
            lbTableBill.Text = btn.Text;
            try
            {
                IdTable = TableBUS.GetAll().Where(p => p.Name == btn.Text).First().ID;
            }catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if(Thanhtoan == false)
            {
                tablefood.Rows.Clear();
            }
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
                                if (id == food.ID.ToString())
                                {
                                    int amount = Int32.Parse(dr["Amount"].ToString()) + Int32.Parse(cbAmount.Text);

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
                            dr["Amount"] = Int32.Parse(cbAmount.Text);
                            dr["Price"] = food.Price * Int32.Parse(cbAmount.Text);
                            tablefood.Rows.Add(dr);
                        }
                    }
                }
            }
            int kt = Convert.ToInt32(lkCategory.GetColumnValue("ID").ToString());
            ReloadFoodAdd(kt);
            Thanhtoan = true;
            BtnHide(true);
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
        private void lkCategory_EditValueChanged(object sender, EventArgs e)
        {
            int kt = Convert.ToInt32(lkCategory.GetColumnValue("ID").ToString());
            ReloadFoodAdd(kt);
        }
        void BtnHide(bool kt)
        {
            txtCustomerPay.Enabled = kt;
            btnPayAfter.Enabled = kt;
            txtCustomerPay.Enabled = kt;
            gridColumn6.Visible = kt;
            gridViewBillInfo.OptionsBehavior.Editable = kt;
            if (Thanhtoan == false)
            {
                gridColumn6.Visible = !kt;
                gridViewBillInfo.OptionsBehavior.Editable = !kt;
                btnPayAfter.Enabled = !kt;
                btnChuyenBan.Enabled = kt;
            }
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

        }
        private void repositoryItemSpinEdit1_ValueChanged(object sender, EventArgs e)
        {
            SpinEdit spin = (SpinEdit)sender;
            DataRow dr = gridViewBillInfo.GetFocusedDataRow();
            int ID = Int32.Parse(dr["ID"].ToString());
            int soLuong = Int32.Parse(spin.Text.TrimEnd('.'));
            int priceS = FoodBUS.GetById(ID).Price;
            int price = soLuong * priceS;
            foreach (DataRow d in tablefood.Rows)
            {
                if (d["ID"].ToString() == ID.ToString())
                {
                    d["Amount"] = soLuong;
                    d["Price"] = price.ToString();
                }
            }
        }
        int TotalPrice()
        {
            int totalprice = 0;
            if(tablefood.Rows.Count > 0)
            {
                foreach (DataRow dr in tablefood.Rows)
                {
                    int price = Int32.Parse(dr["Price"].ToString());
                    totalprice += price;
                }
            }
            return totalprice;
        }
        void Pay(int paystatus,int ab,int customerpay =0)//thanh toán truyền vào trạng thái thanh toán trước hoặc sau
        {
            if (IdTable >0)
            {
                if(Thanhtoan == true)
                {
                    if (tablefood.Rows.Count > 0)
                    {
                        //tạo bill
                        int billid = BillBUS.Pay(IdTable, TotalPrice(), paystatus, tablefood);
                        if (billid != 0)
                        {
                            MessageBox.Show("Thánh toán thành công");
                            TableBUS.UpdateStatusTable(IdTable, true);
                            LoadTable();
                            RefreshData();
                            Thanhtoan = true;
                            BtnHide(false);
                            if (ab == 1)
                            {
                                FrmReportBill frm = new FrmReportBill(billid, customerpay);
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
                else
                {
                    int billid = Convert.ToInt32(lbBillId.Text);
                    // getbill
                    Bill bill = BillBUS.GetById(billid);
                    bill.Status = paystatus;
                    BillBUS.Update(bill);
                    RefreshData();
                    Thanhtoan = true;
                    BtnHide(false);
                    FrmReportBill frm = new FrmReportBill(billid, customerpay);
                    frm.ShowPreviewDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn bàn!!");
            }
        }
        private void btnPay_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn thanh toán ngay!", "Thanh toán!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int cpay = Convert.ToInt32(txtCustomerPay.Text);
                Pay(1, 1,cpay);
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
                Pay(0, 0);
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
        dynamic billInfo_Food(Bill bill)
        {
            dynamic billinfo = (from a in BillBUS.GetAllBillinfo()
                                join b in FoodBUS.GetAll() on a.FoodID equals b.ID
                                where a.BillID == bill.ID
                                select new
                                {
                                    ID = b.ID,
                                    BillID = a.BillID,
                                    Name = b.Name,
                                    Price = b.Price,
                                    Size = b.Size,
                                    Amount = a.Amount

                                }).ToList();
            return billinfo;
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
                dynamic billinfo = billInfo_Food(bill);
                e.ChildList = billinfo;
                
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
            RefreshData(false);
            //load thông tin bill
            Bill bill = (Bill)gridViewBillA.GetFocusedRow();
            lbTableBill.Text =TableBUS.GetById(bill.TableID).Name;
            IdTable = bill.TableID;
            lbBillId.Text = bill.ID.ToString();
            //load bang danh sach mon
            dynamic billinfo = billInfo_Food(bill);
            foreach(var food in billinfo)
            {
                int sl = Convert.ToInt32(food.Amount);
                DataRow dr = tablefood.NewRow();
                dr["ID"] = food.ID;
                dr["Name"] = food.Name;
                dr["Size"] = food.Size;
                dr["Amount"] = sl;
                dr["Price"] = food.Price * sl;
                tablefood.Rows.Add(dr);
            }
            Thanhtoan = false;
            BtnHide(true);
        }
        private void btnChuyenBan_Click(object sender, EventArgs e)
        {
            if (lkChuyenBan.Text != "--Chọn bàn--")
            {
                int idtablenew = Convert.ToInt32(lkChuyenBan.GetColumnValue("ID").ToString());
                DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn muốn chuyển đến: " + lkChuyenBan.Text, "Chuyển bàn!!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    int idbill = Convert.ToInt32(lbBillId.Text);
                    Bill bill = BillBUS.GetById(idbill);
                    bill.TableID = idtablenew;
                    BillBUS.Update(bill);
                    TableBUS.UpdateStatusTable(idtablenew, true);
                    TableBUS.UpdateStatusTable(IdTable, false);
                    MessageBox.Show("Chuyển bàn thành công");
                    RefreshData();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn bàn cần chuyển!!");
            }
        }

        private void btnPreviewFood_Click(object sender, EventArgs e)
        {
            FrmReportListFood frm = new FrmReportListFood(tablefood,IdTable);
            frm.ShowPreview();
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshData();
        }

        private void txtCustomerPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtCustomerPay_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit t = (TextEdit)sender;
            try
            {
                int p = Convert.ToInt32(t.Text);
                int change = p - TotalPrice();
                txtChange.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", change);
                if (change >= 0)
                {
                    btnPay.Enabled = true;
                }
            }catch(Exception)
            {
                txtChange.Text = "0";
            }
        }

        private void txtTotalPrice_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit t = txtCustomerPay;
            try
            {
                int p = Convert.ToInt32(t.Text);
                int change = p - TotalPrice();
                txtChange.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", change);
                if (change >= 0)
                {
                    btnPay.Enabled = true;
                }
            }
            catch (Exception)
            {
                txtChange.Text = "0";
            }
        }

        private void gridViewBillA_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            
        }

        private void gridViewBillA_Click(object sender, EventArgs e)
        {
            RefreshData(false);
            //load thông tin bill
            Bill bill = (Bill)gridViewBillA.GetFocusedRow();
            lbTableBill.Text = TableBUS.GetById(bill.TableID).Name;
            IdTable = bill.TableID;
            lbBillId.Text = bill.ID.ToString();
            //load bang danh sach mon
            dynamic billinfo = billInfo_Food(bill);
            foreach (var food in billinfo)
            {
                int sl = Convert.ToInt32(food.Amount);
                DataRow dr = tablefood.NewRow();
                dr["ID"] = food.ID;
                dr["Name"] = food.Name;
                dr["Size"] = food.Size;
                dr["Amount"] = sl;
                dr["Price"] = food.Price * sl;
                tablefood.Rows.Add(dr);
            }
            Thanhtoan = false;
            BtnHide(true);
        }

        private void btnAddB_Click(object sender, EventArgs e)
        {
            /*int idbill = Convert.ToInt32(lbBillId.Text);
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn bổ sung món vào: " + lbTableBill.Text, "Bổ sung món!!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                BillBUS.Add_Before(idbill, TotalPrice(), tablefood);
                MessageBox.Show("Chuyển bàn thành công");
                RefreshData();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }*/
            FrmMsg msg = new FrmMsg();
            msg.ShowDialog();
        }
    }
}