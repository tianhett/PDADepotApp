using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SCAN.Scanner2D;
using HomotorDepotMgr.Utility;
using System.Data.SQLite;

namespace HomotorDepotMgr
{
    public partial class CaseDetailMgrAdd : Form
    {
        private string caseNumberID = string.Empty;
        private string barcode = string.Empty;
        private string caseNo = string.Empty;
        private string prodID = string.Empty;
        private string prodBarcode = string.Empty;
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msg = new MsgDialog();
        Hook hkCaseDetailMgrAdd = new Hook("CaseDetailMgrAdd");

        public CaseDetailMgrAdd(string caseNumberID, string barcode, string caseNo)
        {
            InitializeComponent();
            this.caseNumberID = caseNumberID;
            this.barcode = barcode;
            this.caseNo = caseNo;
            SetProdInfo(barcode);
            this.txtNum.SelectAll();
            hkCaseDetailMgrAdd.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrAdd_KeyHandlerDelegate);
            hkCaseDetailMgrAdd.Start();
        }

        private void CaseDetailMgrAdd_GotFocus(object sender, EventArgs e)
        {
            //hkCaseDetailMgrAdd.Start();
        }

        public void SetProdInfo(string barcode)
        {
            try
            {
                int curTotalNum = GetCurrentCaseProductTotal(barcode, this.caseNumberID);
                int otherTotalNum = GetOtherCaseProductTotal(barcode, this.caseNumberID);
                string sql = string.Empty;
                List<SQLiteParameter> parameters = new List<SQLiteParameter>();
                //国际商品码
                sql = @"select * from FromERPDetail where BoxBarcode=@BoxBarcode";
                parameters = new List<SQLiteParameter>() {
                    new SQLiteParameter("@BoxBarcode", barcode)
                };
                //处理Code128C码奇数位前面补零的问题
                sql += "  or BoxBarcode=@FormatBoxBarcode ";
                parameters.Add(new SQLiteParameter("@FormatBoxBarcode", barcode.Substring(1)));

                DataTable partsDt = db.ExecuteDataTable(sql, parameters.ToArray());
                if (partsDt != null && partsDt.Rows.Count > 0)
                {
                    prodID = partsDt.Rows[0]["ProdID"].ToString();
                    prodBarcode = partsDt.Rows[0]["Barcode"].ToString();
                    lblProdName.Text = partsDt.Rows[0]["ProdName"].ToString();
                    lblModel.Text = partsDt.Rows[0]["Model"].ToString();
                    lblsNum.Text = partsDt.Rows[0]["Num"].ToString();
                    txtNum.Text = (Convert.ToInt32(partsDt.Rows[0]["NormNum"]) + curTotalNum).ToString();
                    lblTotalNum.Text = (Convert.ToInt32(partsDt.Rows[0]["NormNum"]) + curTotalNum + otherTotalNum).ToString();
                }
                else
                {
                    if (barcode.Contains("-"))
                    {
                        //件码-数量
                        string[] infoArray = barcode.Split(new char[] { '-' });
                        sql = @"select * from FromERPDetail where Barcode=@Barcode";
                        parameters = new List<SQLiteParameter>() {
                            new SQLiteParameter("@Barcode", infoArray[0])
                        };
                        //处理Code128C码奇数位前面补零的问题
                        sql += "  or Barcode=@FormatBarcode ";
                        parameters.Add(new SQLiteParameter("@FormatBarcode", infoArray[0].Substring(1)));

                        DataTable dtFirst = db.ExecuteDataTable(sql, parameters.ToArray());
                        if (dtFirst != null && dtFirst.Rows.Count > 0)
                        {
                            prodID = dtFirst.Rows[0]["ProdID"].ToString();
                            prodBarcode = dtFirst.Rows[0]["Barcode"].ToString();
                            lblProdName.Text = dtFirst.Rows[0]["ProdName"].ToString();
                            lblModel.Text = dtFirst.Rows[0]["Model"].ToString();
                            lblsNum.Text = dtFirst.Rows[0]["Num"].ToString();
                            txtNum.Text = (Convert.ToInt32(infoArray[1]) + curTotalNum).ToString();
                            lblTotalNum.Text = (Convert.ToInt32(infoArray[1]) + curTotalNum + otherTotalNum).ToString();
                        }
                        else
                        {
                            msg.ShowMessage("该配件不在发货单中", 1);
                            lblProdName.Text = "该配件不在发货单中";
                        }
                    }
                    else
                    {
                        //自编条码
                        sql = @"select * from FromERPDetail where Barcode=@Barcode";
                        parameters = new List<SQLiteParameter>() {
                            new SQLiteParameter("@Barcode", barcode)
                        };
                        //处理Code128C码奇数位前面补零的问题
                        sql += "  or Barcode=@FormatBarcode ";
                        parameters.Add(new SQLiteParameter("@FormatBarcode", barcode.Substring(1)));

                        DataTable dtSecond = db.ExecuteDataTable(sql, parameters.ToArray());
                        if (dtSecond != null && dtSecond.Rows.Count > 0)
                        {
                            prodID = dtSecond.Rows[0]["ProdID"].ToString();
                            prodBarcode = dtSecond.Rows[0]["Barcode"].ToString();
                            lblProdName.Text = dtSecond.Rows[0]["ProdName"].ToString();
                            lblModel.Text = dtSecond.Rows[0]["Model"].ToString();
                            lblsNum.Text = dtSecond.Rows[0]["Num"].ToString();
                            if (dtSecond.Rows[0]["bJianMaInBox"].ToString().Equals("True"))
                            {
                                txtNum.Text = (Convert.ToInt32(dtSecond.Rows[0]["NormNum"]) + curTotalNum).ToString();
                                lblTotalNum.Text = (Convert.ToInt32(dtSecond.Rows[0]["NormNum"]) + curTotalNum + otherTotalNum).ToString();
                            }
                            else
                            {
                                txtNum.Text = (curTotalNum + 1).ToString();
                                lblTotalNum.Text = (curTotalNum + 1 + otherTotalNum).ToString();
                            }
                        }
                        else
                        {
                            msg.ShowMessage("该配件不在发货单中", 1);
                            lblProdName.Text = "该配件不在发货单中";
                        }
                    }
                }
                if (Convert.ToInt32(lblTotalNum.Text) > Convert.ToInt32(lblsNum.Text))
                {
                    msg.ShowMessage("实发数量不能超过应发数量", 1);
                    txtNum.Text = curTotalNum.ToString();
                    lblTotalNum.Text = lblsNum.Text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtNum.Text) && Convert.ToInt32(txtNum.Text) > 0)
                    {
                        SaveDetail(Convert.ToInt32(lblTotalNum.Text));
                    }
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
                int otherTotalNum = GetOtherCaseProductTotal(this.prodBarcode, this.caseNumberID);
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
                        string sql = @"select * from CaseNumberDetail where Barcode=@Barcode and CaseNumberID=@CaseNumberID";
                        SQLiteParameter[] parameters = new SQLiteParameter[]{
                            new SQLiteParameter("@Barcode", this.prodBarcode),
                            new SQLiteParameter("@CaseNumberID", this.caseNumberID)
                        };
                        DataTable partDt = db.ExecuteDataTable(sql, parameters);
                        if (partDt != null && partDt.Rows.Count > 0)
                        {
                            sql = @"update CaseNumberDetail set Num=@Num where CaseNumberID=@CaseNumberID and Barcode=@Barcode";
                            parameters = new SQLiteParameter[]{
                                new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                new SQLiteParameter("@CaseNumberID", this.caseNumberID),
                                new SQLiteParameter("@Barcode", this.prodBarcode)
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
                                    new SQLiteParameter("@Barcode", this.prodBarcode),
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

        int hkCaseDetailMgrAdd_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrAdd"))
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

        private void txtNum_TextChanged(object sender, EventArgs e)
        {
            int otherTotalNum = GetOtherCaseProductTotal(barcode, this.caseNumberID);
            if (!string.IsNullOrEmpty(txtNum.Text))
            {
                lblTotalNum.Text = (Convert.ToInt32(txtNum.Text) + otherTotalNum).ToString();
                if (Convert.ToInt32(lblTotalNum.Text) > Convert.ToInt32(lblsNum.Text))
                {
                    msg.ShowMessage("实发数量不能超过应发数量", 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtNum.Text) && Convert.ToInt32(txtNum.Text) > 0)
                    {
                        SaveDetail(Convert.ToInt32(lblTotalNum.Text));
                    }
                }
            }
            else
            {
                lblTotalNum.Text = otherTotalNum.ToString();
            }
        }

        public void CaseDetailAddOKHandler()
        {
            hkCaseDetailMgrAdd.Stop();
            MsgDialog.WindowClose(this.caseNo + "号配件明细");
            CaseDetailMgr detailFrm = new CaseDetailMgr(this.caseNumberID, "", this.caseNo);
            this.Close();
            detailFrm.Show();
        }

        public void CaseDetailAddCancelHandler()
        {
            hkCaseDetailMgrAdd.Stop();
            MsgDialog.WindowClose(this.caseNo + "号配件明细");
            CaseDetailMgr detailFrm = new CaseDetailMgr(this.caseNumberID, "", this.caseNo);
            this.Close();
            detailFrm.Show();
        }

        public void CaseDetailAddDeleteHandler()
        {
            hkCaseDetailMgrAdd.Stop();
            MsgBox delMsgBox = new MsgBox("是否确认要删除该明细？", "警告", 3);
            delMsgBox.ConfirmSelectionDelegate += new MsgBox.ConfirmSelection(delMsgBox_ConfirmSelectionDelegate);
            delMsgBox.Show();
        }

        void delMsgBox_ConfirmSelectionDelegate(int selection)
        {
            if (selection == 1)
            {
                DeleteDetail();
                MsgDialog.WindowClose(this.caseNo + "号配件明细");
                CaseDetailMgr detailFrm = new CaseDetailMgr(this.caseNumberID, "", this.caseNo);
                this.Close();
                detailFrm.Show();
            }
            else
            {
                hkCaseDetailMgrAdd.Start();
            }
        }

        public void DeleteDetail()
        {
            try
            {
                string sql = @"delete from CaseNumberDetail where CaseNumberID=@CaseNumberID and Barcode=@Barcode";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Barcode", this.prodBarcode),
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

        private void CaseDetailMgrAdd_Activated(object sender, EventArgs e)
        {
            Scanner.Instance().OnScanedEvent += new Action<Scanner.CodeInfo>(scanner_OnScanedEvent);
            Scanner.Enable();//启用扫描
        }

        private void CaseDetailMgrAdd_Deactivate(object sender, EventArgs e)
        {
            Scanner.Instance().OnScanedEvent -= new Action<Scanner.CodeInfo>(scanner_OnScanedEvent);
            Scanner.Disable();//禁用扫描
        }

        void scanner_OnScanedEvent(Scanner.CodeInfo obj)
        {
            if (this.InvokeRequired)
            {
                Action<Scanner.CodeInfo> delegateFun = new Action<Scanner.CodeInfo>(scanner_OnScanedEvent);
                this.Invoke(delegateFun, obj);
            }
            else
            {
                if (!string.IsNullOrEmpty(obj.barcode))
                {
                    SetProdInfo(obj.barcode);
                }
            }
        }

        #region 获取配件数量
        /// <summary>
        /// 获取当前箱号里面，配件条码是barcode的配件数量
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="caseNumberID"></param>
        /// <returns></returns>
        public int GetCurrentCaseProductTotal(string barcode, string caseNumberID)
        {
            int currentTotalNum = 0;
            try
            {
                string sql = @"select ifnull(SUM(Num),0) from CaseNumberDetail where (Barcode=@Barcode or Barcode=@FormatBarcode) and CaseNumberID=@CaseNumberID";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Barcode", string.IsNullOrEmpty(barcode) ? this.prodBarcode : barcode.Contains("-") ? barcode.Split(new char[] { '-' })[0] : barcode),
                    new SQLiteParameter("@CaseNumberID", string.IsNullOrEmpty(caseNumberID) ? this.caseNumberID : caseNumberID),
                    new SQLiteParameter("@FormatBarcode", string.IsNullOrEmpty(barcode) ? this.prodBarcode : barcode.Contains("-") ? barcode.Substring(1).Split(new char[] { '-' })[0] : barcode.Substring(1))
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
        /// 获取除当前箱号外的其他箱号里面，配件条码是barcode的配件数量
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="caseNumberID"></param>
        /// <returns></returns>
        public int GetOtherCaseProductTotal(string barcode, string caseNumberID)
        {
            int otherTotalNum = 0;
            try
            {
                string sql = @"select ifnull(SUM(Num),0) from CaseNumberDetail where (Barcode=@Barcode or Barcode=@FormatBarcode) and CaseNumberID<>@CaseNumberID";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Barcode", string.IsNullOrEmpty(barcode) ? this.prodBarcode : barcode.Contains("-") ? barcode.Split(new char[] { '-' })[0] : barcode),
                    new SQLiteParameter("@CaseNumberID", string.IsNullOrEmpty(caseNumberID) ? this.caseNumberID : caseNumberID),
                    new SQLiteParameter("@FormatBarcode", string.IsNullOrEmpty(barcode) ? this.prodBarcode : barcode.Contains("-") ? barcode.Substring(1).Split(new char[] { '-' })[0] : barcode.Substring(1))
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
        /// 获取所有箱号里面，配件条码是barcode的配件数量
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="caseNumberID"></param>
        /// <returns></returns>
        public int GetAllCaseProductTotal(string barcode)
        {
            int allTotalNum = 0;
            try
            {
                string sql = @"select ifnull(SUM(Num),0) from CaseNumberDetail where Barcode=@Barcode";
                //处理Code128C码奇数位前面补零的问题
                sql += "  or Barcode=@FormatBarcode ";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Barcode", string.IsNullOrEmpty(barcode) ? this.prodBarcode : barcode.Contains("-") ? barcode.Split(new char[] { '-' })[0] : barcode),
                    new SQLiteParameter("@FormatBarcode", string.IsNullOrEmpty(barcode) ? this.prodBarcode : barcode.Contains("-") ? barcode.Substring(1).Split(new char[] { '-' })[0] : barcode.Substring(1))
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