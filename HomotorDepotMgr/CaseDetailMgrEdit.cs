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
using SCAN.Scanner2D;

namespace HomotorDepotMgr
{
    public partial class CaseDetailMgrEdit : Form
    {
        public delegate void GetCaseDetailEdit(int selection, int Num, string Barcode, string caseNumberID);
        public event GetCaseDetailEdit GetCaseDetailEditDelegate;

        private string Barcode;
        private string caseNumberID;
        private int otherTotalNum;
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msg = new MsgDialog();
        Hook hkCaseDetailMgrEdit = new Hook("CaseDetailMgrEdit");

        public CaseDetailMgrEdit(string ProdName, string Model, string sNum, string Num, string Barcode, string caseNumberID, bool isEdit)
        {
            InitializeComponent();
            this.lblProdName.Text = ProdName;
            this.lblModel.Text = Model;
            this.lblsNum.Text = sNum;
            this.Barcode = Barcode;
            this.caseNumberID = caseNumberID;
            int otherTotalNum = GetOtherCaseProductTotal(Barcode, caseNumberID);
            this.otherTotalNum = otherTotalNum;
            int curNum = GetCurrentCaseProductTotal(Barcode, caseNumberID);
            if (curNum > 0)
            {
                if (isEdit)
                {
                    this.txtNum.Text = curNum.ToString();
                    this.lblTotalNum.Text = (curNum + otherTotalNum).ToString();
                }
                else
                {
                    this.txtNum.Text = (curNum + Convert.ToInt32(Num)).ToString();
                    this.lblTotalNum.Text = (curNum + Convert.ToInt32(Num) + otherTotalNum).ToString();
                }
            }
            else
            {
                if (isEdit)
                {
                    this.txtNum.Text = "0";
                    this.lblTotalNum.Text = otherTotalNum.ToString();
                }
                else
                {
                    this.txtNum.Text = Num;
                    this.lblTotalNum.Text = (Convert.ToInt32(Num) + otherTotalNum).ToString();
                }
            }
            if (Convert.ToInt32(this.lblTotalNum.Text) > Convert.ToInt32(sNum))
            {
                msg.ShowMessage("实发数量不能超过应发数量", 1);
                this.txtNum.Text = curNum.ToString();
                this.lblTotalNum.Text = sNum;
            }
            this.txtNum.SelectAll();
            hkCaseDetailMgrEdit.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrEdit_KeyHandlerDelegate);
            hkCaseDetailMgrEdit.Start();
        }

        private void CaseDetailMgrEdit_GotFocus(object sender, EventArgs e)
        {
            //hkCaseDetailMgrEdit.Start();
        }

        int hkCaseDetailMgrEdit_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrEdit"))
            {
                if (vkCode == VirtualKey.VK_ENTER)
                {
                    CaseDetailEditOKHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    CaseDetailEditCancelHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_F1)
                {
                    CaseDetailEditDeleteHandler();
                    return -1;
                }
            }
            return 0;
        }

        private void txtNum_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNum.Text))
            {
                lblTotalNum.Text = (Convert.ToInt32(txtNum.Text) + this.otherTotalNum).ToString();
            }
            else
            {
                lblTotalNum.Text = this.otherTotalNum.ToString();
            }
        }

        public void CaseDetailEditOKHandler()
        {
            try
            {
                int caseDetailNum = Convert.ToInt32(lblTotalNum.Text);
                int caseSNum = Convert.ToInt32(lblsNum.Text);
                if (caseDetailNum > caseSNum)
                {
                    msg.ShowMessage("实发数量不能超过应发数量", 1);
                }
                else
                {
                    hkCaseDetailMgrEdit.Stop();
                    GetCaseDetailEditDelegate(1, caseDetailNum - this.otherTotalNum, this.Barcode, this.caseNumberID);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                msg.ShowMessage("请输入数字", 1);
            }
        }

        public void CaseDetailEditCancelHandler()
        {
            hkCaseDetailMgrEdit.Stop();
            GetCaseDetailEditDelegate(0, 0, this.Barcode, this.caseNumberID);
            this.Close();
        }

        public void CaseDetailEditDeleteHandler()
        {
            hkCaseDetailMgrEdit.Stop();
            MsgBox delMsgBox = new MsgBox("是否确认要删除该明细？", "警告", 3);
            delMsgBox.ConfirmSelectionDelegate += new MsgBox.ConfirmSelection(delMsgBox_ConfirmSelectionDelegate);
            delMsgBox.Show();
        }

        void delMsgBox_ConfirmSelectionDelegate(int selection)
        {
            if (selection == 1)
            {
                GetCaseDetailEditDelegate(2, 0, this.Barcode, this.caseNumberID);
                this.Close();
            }
            else
            {
                hkCaseDetailMgrEdit.Start();
            }
        }

        private void btnEditOk_Click(object sender, EventArgs e)
        {
            CaseDetailEditOKHandler();
        }

        private void btnEditCancel_Click(object sender, EventArgs e)
        {
            CaseDetailEditCancelHandler();
        }

        private void btnEditDel_Click(object sender, EventArgs e)
        {
            CaseDetailEditDeleteHandler();
        }

        private void CaseDetailMgrEdit_Activated(object sender, EventArgs e)
        {
            Scanner.Instance().OnScanedEvent += new Action<Scanner.CodeInfo>(scanner_OnScanedEvent);
            Scanner.Enable();//启用扫描
        }

        private void CaseDetailMgrEdit_Deactivate(object sender, EventArgs e)
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
                    //保存当前的
                    SaveCurrentProdDetail();
                    //加载扫条码的
                    LoadProdDetailByBarcode(obj.barcode);
                }
            }
        }

        public void SaveCurrentProdDetail()
        {
            try
            {
                int otherTotalNum = GetOtherCaseProductTotal(this.Barcode, this.caseNumberID);
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
                        string sql = @"select * from CaseNumberDetail where (Barcode=@Barcode or Barcode=@FormatBarcode) and CaseNumberID=@CaseNumberID";
                        SQLiteParameter[] parameters = new SQLiteParameter[]{
                            new SQLiteParameter("@Barcode", this.Barcode),
                            new SQLiteParameter("@CaseNumberID", this.caseNumberID),
                            new SQLiteParameter("@FormatBarcode", this.Barcode.Substring(1))
                        };
                        DataTable partDt = db.ExecuteDataTable(sql, parameters);
                        if (partDt != null && partDt.Rows.Count > 0)
                        {
                            sql = @"update CaseNumberDetail set Num=@Num where CaseNumberID=@CaseNumberID and Barcode=@Barcode";
                            parameters = new SQLiteParameter[]{
                                new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                new SQLiteParameter("@CaseNumberID", this.caseNumberID),
                                new SQLiteParameter("@Barcode", partDt.Rows[0]["Barcode"].ToString())
                            };
                            db.ExecuteNonQuery(sql, parameters);
                        }
                        else
                        {
                            if (caseDetailNum - otherTotalNum > 0)
                            {
                                sql = @"select * from FromERPDetail where Barcode=@Barcode or Barcode=@FormatBarcode";
                                DataTable prodDt = db.ExecuteDataTable(sql, parameters);
                                if (prodDt != null && prodDt.Rows.Count > 0)
                                {
                                    sql = @"insert into CaseNumberDetail(ID,ProdID,Num,CaseNumberID,Barcode,CreateTime,LoginID)  
                                    values (@ID,@ProdID,@Num,@CaseNumberID,@Barcode,@CreateTime,@LoginID)";
                                    parameters = new SQLiteParameter[]{
                                        new SQLiteParameter("@ID", Guid.NewGuid().ToString()),
                                        new SQLiteParameter("@ProdID", prodDt.Rows[0]["ProdID"].ToString()),
                                        new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                        new SQLiteParameter("@CaseNumberID", this.caseNumberID),
                                        new SQLiteParameter("@Barcode", prodDt.Rows[0]["Barcode"].ToString()),
                                        new SQLiteParameter("@CreateTime", DateTime.Now.ToString()),
                                        new SQLiteParameter("@LoginID", GlobalShare.LoginID)
                                    };
                                    db.ExecuteNonQuery(sql, parameters);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadProdDetailByBarcode(string barcode)
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
                    this.lblProdName.Text = partsDt.Rows[0]["ProdName"].ToString();
                    this.lblModel.Text = partsDt.Rows[0]["Model"].ToString();
                    this.lblsNum.Text = partsDt.Rows[0]["Num"].ToString();
                    this.txtNum.Text = (Convert.ToInt32(partsDt.Rows[0]["NormNum"]) + curTotalNum).ToString();
                    this.lblTotalNum.Text = (Convert.ToInt32(partsDt.Rows[0]["NormNum"]) + curTotalNum + otherTotalNum).ToString();
                    this.Barcode = partsDt.Rows[0]["Barcode"].ToString();
                    this.txtNum.SelectAll();
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
                            this.lblProdName.Text = dtFirst.Rows[0]["ProdName"].ToString();
                            this.lblModel.Text = dtFirst.Rows[0]["Model"].ToString();
                            this.lblsNum.Text = dtFirst.Rows[0]["Num"].ToString();
                            this.txtNum.Text = (Convert.ToInt32(infoArray[1]) + curTotalNum).ToString();
                            this.lblTotalNum.Text = (Convert.ToInt32(infoArray[1]) + curTotalNum + otherTotalNum).ToString();
                            this.Barcode = dtFirst.Rows[0]["Barcode"].ToString();
                            this.txtNum.SelectAll();
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
                            this.Barcode = dtSecond.Rows[0]["Barcode"].ToString();
                            this.txtNum.SelectAll();
                        }
                        else
                        {
                            msg.ShowMessage("该配件不在发货单中", 1);
                            lblProdName.Text = "该配件不在发货单中";
                        }
                    }
                }
                if (Convert.ToInt32(txtNum.Text) > Convert.ToInt32(lblsNum.Text))
                {
                    msg.ShowMessage("实发数量不能超过应发数量", 1);
                    txtNum.Text = curTotalNum.ToString();
                    lblTotalNum.Text = lblsNum.Text;
                }
            }
            catch (Exception ex)
            {
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
                    new SQLiteParameter("@Barcode", string.IsNullOrEmpty(barcode) ? this.Barcode : barcode.Contains("-") ? barcode.Split(new char[] { '-' })[0] : barcode),
                    new SQLiteParameter("@CaseNumberID", string.IsNullOrEmpty(caseNumberID) ? this.caseNumberID : caseNumberID),
                    new SQLiteParameter("@FormatBarcode", string.IsNullOrEmpty(barcode) ? this.Barcode.Substring(1) : barcode.Contains("-") ? barcode.Substring(1).Split(new char[] { '-' })[0] : barcode.Substring(1))
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
                    new SQLiteParameter("@Barcode", string.IsNullOrEmpty(barcode) ? this.Barcode : barcode.Contains("-") ? barcode.Split(new char[] { '-' })[0] : barcode),
                    new SQLiteParameter("@CaseNumberID", string.IsNullOrEmpty(caseNumberID) ? this.caseNumberID : caseNumberID),
                    new SQLiteParameter("@FormatBarcode", string.IsNullOrEmpty(barcode) ? this.Barcode.Substring(1) : barcode.Contains("-") ? barcode.Substring(1).Split(new char[] { '-' })[0] : barcode.Substring(1))
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
                    new SQLiteParameter("@Barcode", string.IsNullOrEmpty(barcode) ? this.Barcode : barcode.Contains("-") ? barcode.Split(new char[] { '-' })[0] : barcode),
                    new SQLiteParameter("@FormatBarcode", string.IsNullOrEmpty(barcode) ? this.Barcode.Substring(1) : barcode.Contains("-") ? barcode.Substring(1).Split(new char[] { '-' })[0] : barcode.Substring(1))
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