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
using DevExpress.XtraCharts;

namespace GUI
{
    public partial class FrmStatistical : DevExpress.XtraEditors.XtraForm
    {
        public FrmStatistical()
        {
            InitializeComponent();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void FrmStatistical_Load(object sender, EventArgs e)
        {
            List<Bill> listbill = BillBUS.GetAll();
            Series seri = new Series("DOANH THU", ViewType.Bar);
            foreach (var i in listbill)
            {
                seri.Points.Add(new SeriesPoint(i.AtCreate, i.TotalPrice));
            }
            chartControlSales.Series.Add(seri);
            seri.Label.TextPattern = "{A}: {VP: p0}";

        }
    }
}