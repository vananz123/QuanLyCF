using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DAO;
using BUS;
using System.Collections.Generic;

namespace GUI.Reports
{
    public partial class FrmReportFood : DevExpress.XtraReports.UI.XtraReport
    {
        public FrmReportFood()
        {
            InitializeComponent();
            var f = (from a in FoodBUS.GetAll()
                    join b in CategoryBUS.GetAll() on a.CategoryID equals b.ID
                    select new
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Size = a.Size,
                        Price = a.Price,
                        CategoryID = b.Name
                    }).ToList();
            this.DataSource = f;
            lbID.DataBindings.Add("Text", f, "ID");
            lbName.DataBindings.Add("Text", f, "Name");
            lbSize.DataBindings.Add("Text", f, "Size");
            lbPrice.DataBindings.Add("Text", f, "Price");
            lbCategotyID.DataBindings.Add("Text", f, "CategoryID");
        }
       

    }
}
