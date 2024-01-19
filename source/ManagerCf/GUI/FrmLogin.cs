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
        void login()
        {
            Account account = AccountBUS.CheckLogin(txtUser.Text, txtPass.Text);
            if (account != null)
            {
                AccountBUS.SetAccount(account);
                FrmMain main = new FrmMain(account);
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai username hoặc password!!");
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                login();
            }
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                login();
            }
        }


    }
}