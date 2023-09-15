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

namespace GUI
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Account account = AccountBUS.CheckLogin(txtUser.Text, txtPass.Text);
            if (account!= null)
            {
                FrmMain main =new FrmMain(account);
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("ko");
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}