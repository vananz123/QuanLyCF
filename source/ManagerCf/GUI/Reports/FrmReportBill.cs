using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DAO;
using BUS;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace GUI.Reports
{
    public partial class FrmReportBill : DevExpress.XtraReports.UI.XtraReport
    {
        public FrmReportBill()
        {
            InitializeComponent();
        }
        public FrmReportBill(int billid)
        {
            InitializeComponent();
            Bill bill = BillBUS.GetById(billid);
            List<BillInfo> billInfos = BillBUS.GetAllBillinfo().Where(p => p.BillID == bill.ID).Select(s => s).ToList();
            var billinfofood = (from a in FoodBUS.GetAll()
                                join b in billInfos on a.ID equals b.FoodID
                                select new
                                {
                                    Name = a.Name,
                                    Size = a.Size,
                                    Amount = b.Amount,
                                    Price = a.Price
                                }).ToList();
            pTable.Value = bill.TableID;
            pAtCreate.Value = bill.AtCreate;
            pTotalPrice.Value = string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00} VNĐ", bill.TotalPrice);

            this.DataSource = billinfofood;
            lbName.DataBindings.Add("Text", billinfofood, "Name");
            lbSize.DataBindings.Add("Text", billinfofood, "Size");
            lbAmount.DataBindings.Add("Text", billinfofood, "Amount");
            lbPrice.DataBindings.Add("Text", billinfofood, "Price");
        }
        

    }
}
