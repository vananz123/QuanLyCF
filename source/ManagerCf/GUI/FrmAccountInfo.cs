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
    public partial class FrmAccountInfo : DevExpress.XtraEditors.XtraForm
    {
        public FrmAccountInfo()
        {
            InitializeComponent();
        }
        void loadAccout()
        {
            Account acc = AccountBUS.GetAccount();
            txtDisName.Text = acc.DisplayName;
            txtName.Text = acc.UserName;
            txtPass.Text = acc.Password;
            txtRole.Text = acc.Role;
            if(hide == true)
            {
                txtPass.Properties.PasswordChar = '*';
            }
        }
        private void FrmAccountInfo_Load(object sender, EventArgs e)
        {
            loadAccout();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string disname= txtDisName.Text;
            string pw =txtPass.Text;
            Account acc = AccountBUS.GetAccount();
            if(acc != null)
            {
                acc.DisplayName = disname;
                acc.Password = pw;
                AccountBUS.Update(acc);
                AccountBUS.SetAccount(acc);
                MessageBox.Show("Cập nhật thành công!");
                loadAccout();
            }
        }
        bool hide = true;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (hide == false)
            {
                txtPass.Properties.PasswordChar = '\0';
                hide = true;

            }
            else
            {
                txtPass.Properties.PasswordChar = '*';
                hide = false;
            }
        }
    }
}