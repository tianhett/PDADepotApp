using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HomotorDepotMgr.Utility;
using System.Data.SQLite;

namespace HomotorDepotMgr
{
    public partial class CaseDetailMgrModelAdd : Form
    {
        private string caseNumberID = string.Empty;
        private string model = string.Empty;
        private string caseNo = string.Empty;
        private string prodID = string.Empty;
        private string barcode = string.Empty;
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msg = new MsgDialog();
        Hook hkCaseDetailMgrModelAdd = new Hook("CaseDetailMgrModelAdd");

        public CaseDetailMgrModelAdd(string caseNumberID, string caseNo, string model)
        {
            InitializeComponent();
            this.caseNumberID = caseNumberID;
            this.model = model.Trim();
            this.caseNo = caseNo;
            SetProdInfo(model);
            this.txtNum.SelectAll();
            hkCaseDetailMgrModelAdd.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrModelAdd_KeyHandlerDelegate);
            hkCaseDetailMgrModelAdd.Start();
        }

        public void SetProdInfo(string model)
        {
            try
            {
                string sql = string.Empty;
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                sql = @"select * from FromERPDetail where Model=@Model";
                parameters = new List<SQLiteParameter>() {
                    new SQLiteParameter("@Model", model.Trim())
                };
                DataTable partsDt = db.ExecuteDataTable(sql, parameters.ToArray());
                if (partsDt != null && partsDt.Rows.Count > 0)
                {
                    this.barcode = partsDt.Rows[0]["Barcode"].ToString();
                    this.prodID = partsDt.Rows[0]["ProdID"].ToString();
                    int curTotalNum = GetCurrentCaseProductTotal(this.prodID, this.caseNumberID);
                    int otherTotalNum = GetOtherCaseProductTotal(this.prodID, this.caseNumberID);
                    this.model = partsDt.Rows[0]["Model"].ToString();
                    lblProdName.Text = partsDt.Rows[0]["ProdName"].ToString();
                    lblModel.Text = partsDt.Rows[0]["Model"].ToString();
                    lblsNum.Text = partsDt.Rows[0]["Num"].ToString();
                    lblTotalNum.Text = (curTotalNum + otherTotalNum).ToString();
                    if (curTotalNum == 0)
                    {
                        txtNum.Text = "";
                    }
                    else
                    {
                        txtNum.Text = curTotalNum.ToString();
                    }
                    txtNum.Focus();
                }
                else
                {
                    msg.ShowMessage("该配件不在发货单中", 1);
                    lblProdName.Text = "该配件不在发货单中";
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void SaveDetail(int curTotalNum)
        {
            try
            {
                int otherTotalNum = GetOtherCaseProductTotal(this.prodID, this.caseNumberID);
                int caseDetailNum = Convert.ToInt32(lblTotalNum.Text);
                int caseSNum = Convert.ToInt32(lblsNum.Text);
                if (caseDetailNum > caseSNum)
                {
                    msg.ShowMessage("实发数量不能超过应发数量", 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtNum.Text) && Convert.ToInt32(txtNum.Text) > 0)
                    {
                        string sql = @"select * from CaseNumberDetail where ProdID=@ProdID and CaseNumberID=@CaseNumberID";
                        SQLiteParameter[] parameters = new SQLiteParameter[]{
                            new SQLiteParameter("@ProdID", this.prodID),
                            new SQLiteParameter("@CaseNumberID", this.caseNumberID)
                        };
                        DataTable partDt = db.ExecuteDataTable(sql, parameters);
                        if (partDt != null && partDt.Rows.Count > 0)
                        {
                            sql = @"update CaseNumberDetail set Num=@Num where CaseNumberID=@CaseNumberID and ProdID=@ProdID";
                            parameters = new SQLiteParameter[]{
                                new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                new SQLiteParameter("@CaseNumberID", this.caseNumberID),
                                new SQLiteParameter("@ProdID", this.prodID)
                            };
                            db.ExecuteNonQuery(sql, parameters);
                        }
                        else
                        {
                            if (caseDetailNum - otherTotalNum > 0)
                            {
                                sql = @"insert into CaseNumberDetail(ID,ProdID,Num,CaseNumberID,Barcode,CreateTime,LoginID)  
                                    values (@ID,@ProdID,@Num,@CaseNumberID,@Barcode,@CreateTime,@LoginID)";
                                parameters = new SQLiteParameter[]{
                                    new SQLiteParameter("@ID", Guid.NewGuid().ToString()),
                                    new SQLiteParameter("@ProdID", this.prodID),
                                    new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                    new SQLiteParameter("@CaseNumberID", this.caseNumberID),
                                    new SQLiteParameter("@Barcode", this.barcode),
                                    new SQLiteParameter("@CreateTime", DateTime.Now.ToString()),
                                    new SQLiteParameter("@LoginID", GlobalShare.LoginID)
                                };
                                db.ExecuteNonQuery(sql, parameters);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        int hkCaseDetailMgrModelAdd_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrModelAdd"))
            {
                if (vkCode == VirtualKey.VK_ENTER)
                {
                    CaseDetailAddOKHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    CaseDetailAddCancelHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_F1)
                {
                    CaseDetailAddDeleteHandler();
                    return -1;
                }
            }
            return 0;
        }

        public void CaseDetailAddOKHandler()
        {
            if (Convert.ToInt32(lblTotalNum.Text) > Convert.ToInt32(lblsNum.Text))
            {
                msg.ShowMessage("实发数量不能超过应发数量", 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(txtNum.Text) && Convert.ToInt32(txtNum.Text) > 0)
                {
                    SaveDetail(Convert.ToInt32(lblTotalNum.Text));
                    hkCaseDetailMgrModelAdd.Stop();
                    CaseDetailMgr detailFrm = new CaseDetailMgr(this.caseNumberID, "", this.caseNo);
                    this.Close();
                    detailFrm.Show();
                }
            }
        }

        public void CaseDetailAddCancelHandler()
        {
            hkCaseDetailMgrModelAdd.Stop();
            CaseDetailMgr detailFrm = new CaseDetailMgr(this.caseNumberID, "", this.caseNo);
            this.Close();
            detailFrm.Show();
        }

        public void CaseDetailAddDeleteHandler()
        {
            hkCaseDetailMgrModelAdd.Stop();
            MsgBox delMsgBox = new MsgBox("是否确认要删除该明细？", "警告", 3);
            delMsgBox.ConfirmSelectionDelegate += new MsgBox.ConfirmSelection(delMsgBox_ConfirmSelectionDelegate);
            delMsgBox.Show();
        }

        void delMsgBox_ConfirmSelectionDelegate(int selection)
        {
            if (selection == 1)
            {
                DeleteDetail();
                CaseDetailMgr detailFrm = new CaseDetailMgr(this.caseNumberID, "", this.caseNo);
                this.Close();
                detailFrm.Show();
            }
            else
            {
                hkCaseDetailMgrModelAdd.Start();
            }
        }

        public void DeleteDetail()
        {
            try
            {
                string sql = @"delete from CaseNumberDetail where CaseNumberID=@CaseNumberID and ProdID=@ProdID";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@ProdID", this.prodID),
                    new SQLiteParameter("@CaseNumberID", this.caseNumberID)
                };
                db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnAddDel_Click(object sender, EventArgs e)
        {
            CaseDetailAddDeleteHandler();
        }

        private void btnAddCancel_Click(object sender, EventArgs e)
        {
            CaseDetailAddCancelHandler();
        }

        private void btnAddOk_Click(object sender, EventArgs e)
        {
            CaseDetailAddOKHandler();
        }

        private void txtNum_TextChanged(object sender, EventArgs e)
        {
            int otherTotalNum = GetOtherCaseProductTotal(this.prodID, this.caseNumberID);
            if (!string.IsNullOrEmpty(txtNum.Text))
            {
                lblTotalNum.Text = (Convert.ToInt32(txtNum.Text) + otherTotalNum).ToString();
            }
            else
            {
                lblTotalNum.Text = otherTotalNum.ToString();
            }
        }


        #region 获取配件数量
        /// <summary>
        /// 获取当前箱号里面，配件ID是prodID的配件数量
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="caseNumberID"></param>
        /// <returns></returns>
        public int GetCurrentCaseProductTotal(string prodID, string caseNumberID)
        {
            int currentTotalNum = 0;
            try
            {
                string sql = @"select ifnull(SUM(Num),0) from CaseNumberDetail where ProdID=@ProdID and CaseNumberID=@CaseNumberID";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@ProdID", string.IsNullOrEmpty(prodID) ? this.prodID : prodID),
                    new SQLiteParameter("@CaseNumberID", string.IsNullOrEmpty(caseNumberID) ? this.caseNumberID : caseNumberID)
                };
                DataTable dt = db.ExecuteDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    currentTotalNum = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
            }
            return currentTotalNum;
        }

        /// <summary>
        /// 获取除当前箱号外的其他箱号里面，配件ID是prodID的配件数量
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="caseNumberID"></param>
        /// <returns></returns>
        public int GetOtherCaseProductTotal(string prodID, string caseNumberID)
        {
            int otherTotalNum = 0;
            try
            {
                string sql = @"select ifnull(SUM(Num),0) from CaseNumberDetail where ProdID=@ProdID and CaseNumberID<>@CaseNumberID";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@ProdID", string.IsNullOrEmpty(prodID) ? this.prodID : prodID),
                    new SQLiteParameter("@CaseNumberID", string.IsNullOrEmpty(caseNumberID) ? this.caseNumberID : caseNumberID)
                };
                DataTable dt = db.ExecuteDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    otherTotalNum = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
            }
            return otherTotalNum;
        }

        /// <summary>
        /// 获取所有箱号里面，配件ID是prodID的配件数量
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="caseNumberID"></param>
        /// <returns></returns>
        public int GetAllCaseProductTotal(string prodID)
        {
            int allTotalNum = 0;
            try
            {
                string sql = @"select ifnull(SUM(Num),0) from CaseNumberDetail where ProdID=@ProdID";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@ProdID", string.IsNullOrEmpty(prodID) ? this.prodID : prodID),
                };
                DataTable dt = db.ExecuteDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    allTotalNum = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
            }
            return allTotalNum;
        }
        #endregion

    }
}