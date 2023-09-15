using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO;

namespace BUS
{
    public static class BillBUS
    {
        static IBill ibill;
        static IBillInfo ibillinfo;
        static BillBUS()
        {
            ibill = new BillDAO();
            ibillinfo = new BillInfoDAO();
        }
        public static List<Bill> GetAll()
        {
            return ibill.GetAll();
        }
        public static Bill GetById(int id)
        {
            return ibill.GetById(id);
        }
        public static void Update(Bill obj)
        {
            ibill.Update(obj);
        }
        public static List<BillInfo> GetAllBillinfo()
        {
            return ibillinfo.GetAll();
        }

        public static Bill Insert(Bill obj)
        {
            return ibill.Insert(obj);
        }
        public static BillInfo InsertBillInfo(BillInfo obj)
        {
            return ibillinfo.Insert(obj);
        }
        public static void Delete(Bill obj)
        {
            ibill.Delete(obj);
        }
        public static void DeleteBillInfo(BillInfo obj)
        {
            ibillinfo.Delete(obj);
        }
        public static int Pay(int idTable,int totalprice,int status, DataTable dataBillInfo)
        {
            try
            {
                Bill bill = new Bill() { AtCreate = DateTime.Now, TableID = idTable, TotalPrice = totalprice, Status = status };
                Bill b = BillBUS.Insert(bill);
                foreach (DataRow dr in dataBillInfo.Rows)
                {
                    int FoodID = Int32.Parse(dr["ID"].ToString());
                    int Amount = Int32.Parse(dr["Amount"].ToString());
                    BillInfo billInfo = new BillInfo() { BillID = b.ID, FoodID = FoodID, Amount = Amount };
                    InsertBillInfo(billInfo);
                }
                return b.ID;
            }catch (Exception e)
            {
                return 0;
            }
        }
        public static bool ChuyenBan(int idbill,int totalprice, DataTable dataBillInfo)
        {
            try
            {
                Bill bill = ibill.GetById(idbill);
                List<BillInfo> listbillInfo= ibillinfo.GetAll().Where(p => p.BillID == bill.ID).Select(s => s).ToList();
                foreach (DataRow dr in dataBillInfo.Rows)
                {
                    int FoodID = Int32.Parse(dr["ID"].ToString());
                    int Amount = Int32.Parse(dr["Amount"].ToString());
                  
                    BillInfo billInfo2= listbillInfo.Where(p => p.FoodID == FoodID).FirstOrDefault();
                    if (billInfo2 != null)
                    {
                        billInfo2.Amount += Amount;
                        ibillinfo.Update(billInfo2);
                    }
                    if(billInfo2 == null)
                    {
                        BillInfo billInfo = new BillInfo() { BillID = bill.ID, FoodID = FoodID, Amount = Amount };
                        InsertBillInfo(billInfo);
                    }

                }
                bill.TotalPrice += totalprice;
                ibill.Update(bill);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
