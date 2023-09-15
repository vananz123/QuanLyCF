using DAO;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;


namespace GUI.Reports
{
    public partial class FrmReportListFood : DevExpress.XtraReports.UI.XtraReport
    {
        public FrmReportListFood()
        {
            InitializeComponent();
        }
        DataTable listFood;
        public FrmReportListFood(DataTable l)
        {
            InitializeComponent();
            this.listFood = l;
            this.DataSource = listFood;
            load();
        }
        void load()
        {
            txtName.DataBindings.Add("Text", listFood, "Name");
            txtSize.DataBindings.Add("Text", listFood, "Size");
            txtPrice.DataBindings.Add("Text", listFood, "Price");
        }

    }
}
