using DevExpress.XtraBars;
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
using DAO;
using BUS;

namespace GUI
{
    public partial class FrmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        Account account;
        public FrmMain(Account acc)
        {
            InitializeComponent();
            this.account = acc;
        }
        void OpenForm(Type typeForm)
        {
            foreach(Form frm in MdiChildren)
            {
                if(frm.GetType()== typeForm)
                {
                    frm.Activate();
                    return;
                }
            }
            Form f = (Form)Activator.CreateInstance(typeForm);
            f.MdiParent = this;
            f.Show();
        }
        private void btnTable_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmOrder));
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmTable));
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmTableStatus));
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmFood));
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmCategory));
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmAdAccount));
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmStatistical));
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmSales));
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if(account.Role=="Nhân viên")
            {
                ribbonPage3.Visible = false;
                OpenForm(typeof(FrmOrder));
            }
            else
            {
                OpenForm(typeof(FrmStatistical));
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmLogin login = new FrmLogin();
            login.Show();
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(FrmAccountInfo));
        }
    }
}