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
    public partial class CaseDetailMgrWholeCaseAdd : Form
    {
        public delegate void GetCaseDetailMgrWholeCaseAdd(int selection);
        public event GetCaseDetailMgrWholeCaseAdd GetCaseDetailMgrWholeCaseAddDelegate;

        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msg = new MsgDialog();
        Hook hkCaseDetailMgrWholeCaseAdd = new Hook("CaseDetailMgrWholeCaseAdd");

        private string barcode = string.Empty;
        private string prodBarcode = string.Empty;
        private string prodID = string.Empty;

        public CaseDetailMgrWholeCaseAdd(string barcode)
        {
            InitializeComponent();
            this.barcode = barcode;
            SetProdInfo(barcode, 0);
            this.txtNum.SelectAll();
            hkCaseDetailMgrWholeCaseAdd.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailMgrWholeCaseAdd_KeyHandlerDelegate);
            hkCaseDetailMgrWholeCaseAdd.Start();
        }

        private void CaseDetailMgrWholeCaseAdd_GotFocus(object sender, EventArgs e)
        {
            //hkCaseDetailMgrWholeCaseAdd.Start();
        }

        public void SetProdInfo(string barcode, int type)
        {
            try
            {
                string caseNumberID = GetCaseNumber();
                if (type == 1)
                {
                    if (Convert.ToInt32(lblTotalNum.Text) > Convert.ToInt32(lblsNum.Text))
                    {
                        msg.ShowMessage("实发数量不能超过应发数量", 1);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtNum.Text) && Convert.ToInt32(txtNum.Text) > 0)
                        {
                            SaveDetail(Convert.ToInt32(lblTotalNum.Text), caseNumberID);
                        }
                    }
                    this.barcode = barcode;
                    txtCaseNum.Text = "";
                    ShowProductInfo(caseNumberID);
                }
                else
                {
                    ShowProductInfo(caseNumberID);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowProductInfo(string caseNumberID)
        {
            int curTotalNum = GetCurrentCaseProductTotal(barcode, caseNumberID);
            int otherTotalNum = GetOtherCaseProductTotal(barcode, caseNumberID);
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
        }

        public void SaveDetail(int curTotalNum, string caseNumberID)
        {
            try
            {
                string sql = string.Empty;
                List<SQLiteParameter> parameterList = new List<SQLiteParameter>();
                if (string.IsNullOrEmpty(caseNumberID) && string.IsNullOrEmpty(txtCaseNum.Text))
                {
                    msg.ShowMessage("请输入箱号", 1);
                }
                else
                {
                    if (string.IsNullOrEmpty(caseNumberID))
                    {
                        caseNumberID = Guid.NewGuid().ToString();
                        sql = @"insert into CaseNumber(ID,CaseNumber,FromID,LoginID) values(@ID,@CaseNumber,@FromID,@LoginID)";
                        parameterList = new List<SQLiteParameter>(){
                            new SQLiteParameter("@ID", caseNumberID),
                            new SQLiteParameter("@CaseNumber", txtCaseNum.Text),
                            new SQLiteParameter("@FromID", TerminalInfo.GetDeviceID()),
                            new SQLiteParameter("@LoginID", GlobalShare.LoginID)
                        };
                        db.ExecuteNonQuery(sql, parameterList.ToArray());
                    }
                    int otherTotalNum = GetOtherCaseProductTotal(this.prodBarcode, caseNumberID);
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
                            sql = @"select * from CaseNumberDetail where Barcode=@Barcode and CaseNumberID=@CaseNumberID";
                            parameterList.Clear();
                            parameterList = new List<SQLiteParameter>{
                                new SQLiteParameter("@Barcode", this.prodBarcode),
                                new SQLiteParameter("@CaseNumberID", caseNumberID)
                            };
                            DataTable partDt = db.ExecuteDataTable(sql, parameterList.ToArray());
                            if (partDt != null && partDt.Rows.Count > 0)
                            {
                                sql = @"update CaseNumberDetail set Num=@Num where CaseNumberID=@CaseNumberID and Barcode=@Barcode";
                                parameterList = new List<SQLiteParameter>   {
                                    new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                    new SQLiteParameter("@CaseNumberID", caseNumberID),
                                    new SQLiteParameter("@Barcode", this.prodBarcode)
                                };
                                db.ExecuteNonQuery(sql, parameterList.ToArray());
                            }
                            else
                            {
                                if (caseDetailNum - otherTotalNum > 0)
                                {
                                    sql = @"insert into CaseNumberDetail(ID,ProdID,Num,CaseNumberID,Barcode,CreateTime,LoginID)  
                                    values (@ID,@ProdID,@Num,@CaseNumberID,@Barcode,@CreateTime,@LoginID)";
                                    parameterList = new List<SQLiteParameter>{
                                        new SQLiteParameter("@ID", Guid.NewGuid().ToString()),
                                        new SQLiteParameter("@ProdID", this.prodID),
                                        new SQLiteParameter("@Num", (caseDetailNum - otherTotalNum)),
                                        new SQLiteParameter("@CaseNumberID", caseNumberID),
                                        new SQLiteParameter("@Barcode", this.prodBarcode),
                                        new SQLiteParameter("@CreateTime", DateTime.Now.ToString()),
                                        new SQLiteParameter("@LoginID", GlobalShare.LoginID)
                                    };
                                    db.ExecuteNonQuery(sql, parameterList.ToArray());
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

        int hkCaseDetailMgrWholeCaseAdd_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("CaseDetailMgrWholeCaseAdd"))
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
            }
            return 0;
        }

        private void btnAddCancel_Click(object sender, EventArgs e)
        {
            CaseDetailAddCancelHandler();
        }

        private void btnAddOk_Click(object sender, EventArgs e)
        {
            CaseDetailAddOKHandler();
        }

        public void CaseDetailAddOKHandler()
        {
            hkCaseDetailMgrWholeCaseAdd.Stop();
            if (!string.IsNullOrEmpty(txtNum.Text) && Convert.ToInt32(txtNum.Text) > 0)
            {
                string caseNumberID = GetCaseNumber();
                SaveDetail(Convert.ToInt32(lblTotalNum.Text), caseNumberID);
            }
            GetCaseDetailMgrWholeCaseAddDelegate(1);
            this.Close();
        }

        public void CaseDetailAddCancelHandler()
        {
            hkCaseDetailMgrWholeCaseAdd.Stop();
            GetCaseDetailMgrWholeCaseAddDelegate(0);
            this.Close();
        }

        private void txtNum_TextChanged(object sender, EventArgs e)
        {
            string caseNumberID = GetCaseNumber();
            int otherTotalNum = GetOtherCaseProductTotal(barcode, caseNumberID);
            if (!string.IsNullOrEmpty(txtNum.Text))
            {
                lblTotalNum.Text = (Convert.ToInt32(txtNum.Text) + otherTotalNum).ToString();
                if (Convert.ToInt32(lblTotalNum.Text) > Convert.ToInt32(lblsNum.Text))
                {
                    msg.ShowMessage("实发数量不能超过应发数量", 1);
                }
            }
            else
            {
                lblTotalNum.Text = otherTotalNum.ToString();
            }
        }

        private void CaseDetailMgrWholeCaseAdd_Activated(object sender, EventArgs e)
        {
            Scanner.Instance().OnScanedEvent += new Action<Scanner.CodeInfo>(scanner_OnScanedEvent);
            Scanner.Enable();//启用扫描
        }

        private void CaseDetailMgrWholeCaseAdd_Deactivate(object sender, EventArgs e)
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
                    SetProdInfo(obj.barcode, 1);
                }
            }
        }

        private void btnAlphabet_Click(object sender, EventArgs e)
        {
            hkCaseDetailMgrWholeCaseAdd.Stop();
            Alphabet letterFrm = new Alphabet();
            letterFrm.GetConfirmLetterDelegate += new Alphabet.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                txtCaseNum.Text += letter;
                txtCaseNum.Focus();
                txtCaseNum.Select(txtCaseNum.Text.Length, 0);
            }
            hkCaseDetailMgrWholeCaseAdd.Start();
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
                    new SQLiteParameter("@CaseNumberID", caseNumberID),
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
                    new SQLiteParameter("@CaseNumberID", caseNumberID),
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

        #region 获取箱号
        private string GetCaseNumber()
        {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(txtCaseNum.Text))
            {
                try
                {
                    string sql = @"select * from CaseNumber where CaseNumber=@CaseNumber";
                    SQLiteParameter[] parameters = new SQLiteParameter[]{
                        new SQLiteParameter("@CaseNumber", txtCaseNum.Text)
                    };
                    DataTable dt = db.ExecuteDataTable(sql, parameters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        id = dt.Rows[0]["ID"].ToString();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return id;
        }
        #endregion
        


    }
}