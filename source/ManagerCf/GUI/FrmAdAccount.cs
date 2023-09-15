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
using DevExpress.XtraGrid.Views.Grid;
using System.Data.Entity;

namespace GUI
{
    public partial class FrmAdAccount : DevExpress.XtraEditors.XtraForm
    {
        public FrmAdAccount()
        {
            InitializeComponent();
            DAO.qlcafe dbContext = new DAO.qlcafe();
            LoadData();
        }
        void LoadData()
        {
            girdControlAcount.DataSource = AccountBUS.GetALL();
            BtnHide(true);
        }

        void AddBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", girdControlAcount.DataSource, "UserName"));
            txtDisplayName.DataBindings.Add(new Binding("Text", girdControlAcount.DataSource, "DisplayName"));
            txtPassword.DataBindings.Add(new Binding("Text", girdControlAcount.DataSource, "Password"));
            cbRole.DataBindings.Add(new Binding("Text", girdControlAcount.DataSource, "Role"));


        }
        void ClearBingding()
        {
            txtUserName.DataBindings.Clear();
            txtDisplayName.DataBindings.Clear();
            txtPassword.DataBindings.Clear();
            cbRole.DataBindings.Clear();
        }
        void BtnHide(bool kt)//true la mo add,edit,del, f = tat
        {
            txtUserName.Enabled = !kt;
            txtDisplayName.Enabled = !kt;
            txtPassword.Enabled = !kt;
            cbRole.Enabled = !kt;
            barBtnAdd.Enabled = kt;
            barBtnDel.Enabled = kt;
            barBtnEdit.Enabled = kt;
            barBtnSave.Enabled = !kt;
            barBtnCanel.Enabled = !kt;

        }
        void EditGrid(bool kt)//true la mo chon nhien dong , f la tat
        {
            girdviewAccount.OptionsSelection.MultiSelect = kt;
            if (kt == true)
            {
                girdviewAccount.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            }
            else
            {
                girdviewAccount.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
            }

        }
        bool CheckName(List<DAO.Account> list, string name) //đổi TableCoffee thành đối tượng tương ứng
        {
            if (list.Where(p => p.UserName.Contains(name)).ToList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void FrmAdAccount_Load(object sender, EventArgs e)
        {
            BtnHide(true);
            txtUserName.Enabled = false;
            txtDisplayName.Enabled = false;
            txtPassword.Enabled = false;
            cbRole.Enabled = false;
        }
        bool ADD = false;
        bool DEL = false;
        bool EDIT = false;
        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            ADD = true;
            txtUserName.Text = "";
            txtDisplayName.Text = "";
            txtPassword.Text = "";
            cbRole.Text = "";
            BtnHide(false);
        }

        private void barBtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EDIT = true;
            AddBinding();
            BtnHide(false);
        }

        private void barBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DEL = true;
            EditGrid(DEL);
            BtnHide(false);
        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DEL == true)
            {
                for (int i = 0; i < girdviewAccount.RowCount; i++)
                {
                    var r = (Account)girdviewAccount.GetRow(i);
                    if (r != null)
                    {
                        if (girdviewAccount.IsRowSelected(i) == true)
                        {
                            AccountBUS.Delete(r);
                        }
                    }
                }
                DEL = false;
                EditGrid(DEL);
            }
            if (ADD == true)
            {
                string username = txtUserName.Text;
                string displayname = txtDisplayName.Text;
                string password = txtPassword.Text;
                string role = cbRole.Text;
                try
                {
                    if (CheckName(AccountBUS.GetALL(), username) == true)
                    {
                        MessageBox.Show("Tên đã tồn tại");
                    }
                    else
                    {
                        Account account = new Account() { UserName = username, DisplayName = displayname, Password = password, Role = role };
                        AccountBUS.Insert(account);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("lỗi");
                }
                ADD = false;
            }
            if (EDIT == true)
            {

                try
                {
                    string username = txtUserName.Text;
                    string displayname = txtDisplayName.Text;
                    string password = txtPassword.Text;
                    string role = cbRole.Text;
                    var i = (Account)girdviewAccount.GetRow(girdviewAccount.GetFocusedDataSourceRowIndex());
                    i.UserName = username;
                    i.DisplayName = displayname;
                    i.Password = password;
                    i.Role = role;
                    AccountBUS.Update(i);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("lỗi");
                }
                EDIT = false;
            }
            BtnHide(true);
            ClearBingding();
            LoadData();
        }

        private void barBtnCanel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ADD = false;
            EDIT = false;
            DEL = false;
            ClearBingding();
            EditGrid(DEL);
            BtnHide(true);
        }

        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }


    }
}