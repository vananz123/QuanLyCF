using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DAO;
using System.Collections.Generic;

namespace GUI.Reports
{
    public partial class FrmReportCategory : DevExpress.XtraReports.UI.XtraReport
    {
        public FrmReportCategory()
        {
            InitializeComponent();
        }
        List<CategoryFood> _list;
        public FrmReportCategory(List<CategoryFood> list)
        {
            InitializeComponent();
            this._list = list;
            this.DataSource = _list;
            LoadData();
        }
        void LoadData()
        {
            lbID.DataBindings.Add("Text", _list, "ID");
            lbName.DataBindings.Add("Text", _list, "Name");
        }

    }
}
