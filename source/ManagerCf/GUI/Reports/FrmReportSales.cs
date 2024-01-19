using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using BUS;
using DAO;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Reports
{
    public partial class FrmReportSales : DevExpress.XtraReports.UI.XtraReport
    {
        public FrmReportSales()
        {
            InitializeComponent();
        }
        public FrmReportSales(List<Bill> bill)
        {

            InitializeComponent();
            var result =(from a in bill join b in BillBUS.GetAllBillinfo()
                    on a.ID equals b.BillID
                          select new
                          {
                              AtCreate = a.AtCreate.ToString("dd/MM/yyyy"),
                              Amount = b.Amount,
                              Sale = a.TotalPrice
                          }).ToList();
            var t = result.GroupBy(p => new { p.AtCreate,p.Sale}).Select(s=> new { AtCreate =s.Key.AtCreate,Amount=s.Sum(m => m.Amount), Sale=s.Key.Sale}).ToList();
            var tk = t.GroupBy(p => p.AtCreate).Select(s => new { AtCreate = s.Key, Amount = s.Sum(m => m.Amount) ,Sale = s.Sum(m1 => m1.Sale) }).ToList();
            this.DataSource = tk;
            txtDate.DataBindings.Add("Text", tk, "AtCreate");
            txtAmount.DataBindings.Add("Text", tk, "Amount");
            txtSale.DataBindings.Add("Text", tk, "Sale");
        }


    }
}
