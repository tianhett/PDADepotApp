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
using System.Collections;
using System.Reflection;

namespace HomotorDepotMgr
{
    public partial class CaseDetailDiffer : Form
    {
        Hook hkCaseDetailDiffer = new Hook("hkCaseDetailDiffer");
        SQLiteDBHelper db = new SQLiteDBHelper();

        public CaseDetailDiffer()
        {
            InitializeComponent();
            hkCaseDetailDiffer.KeyHandlerDelegate += new Hook.KeyHandler(hkCaseDetailDiffer_KeyHandlerDelegate);
            hkCaseDetailDiffer.Start();
            ReloadDifferData();
        }

        private void CaseDetailDiffer_GotFocus(object sender, EventArgs e)
        {
            //hkCaseDetailDiffer.Start();
        }

        int hkCaseDetailDiffer_KeyHandlerDelegate(int vkCode, string clsName)
        {
            int result = 0;
            if (clsName.Equals("hkCaseDetailDiffer"))
            {
                switch (vkCode)
                {
                    case VirtualKey.VK_ESCAPE:
                        hkCaseDetailDiffer.Stop();
                        //返回去箱号管理界面，刷新箱号管理的列表
                        Cls_Message.SendMessage("箱号管理", "CaseDetailDiffer");
                        this.Close();
                        result = -1;
                        break;
                }
            }
            return result;
        }

        public void ReloadDifferData()
        {
            string sql = @"select v.ProdName as 名称,v.Model as 型号,v.yNum as 应发,v.sNum as 数量,v.AreaPlaceCode as 仓位 from (
            select a.ProdName,a.Model,a.Num as yNum,ifnull(b.Num,0) as sNum,a.Num-ifnull(b.Num,0) as balance,ifnull(a.AreaPlaceCode,'') as AreaPlaceCode from FromERPDetail a left outer join 
            (select ProdID,SUM(Num) as Num from CaseNumberDetail group by ProdID) b on a.ProdID=b.ProdID
            ) v where v.balance<>0 order by v.ProdName";
            try
            {
                DataTable dt = db.ExecuteDataTable(sql, null);
                DataGridTableStyle ts = new DataGridTableStyle();
                ts.MappingName = dt.TableName; 
                DataGridColumnStyle MCColStyle = new DataGridTextBoxColumn();
                MCColStyle.MappingName = "名称";
                MCColStyle.HeaderText = "名称";
                MCColStyle.Width = 60;
                ts.GridColumnStyles.Add(MCColStyle);
                DataGridColumnStyle XHColStyle = new DataGridTextBoxColumn();
                XHColStyle.MappingName = "型号";
                XHColStyle.HeaderText = "型号";
                XHColStyle.Width = 60;
                ts.GridColumnStyles.Add(XHColStyle);
                DataGridColumnStyle YFColStyle = new DataGridTextBoxColumn();
                YFColStyle.MappingName = "应发";
                YFColStyle.HeaderText = "应发";
                YFColStyle.Width = 30;
                ts.GridColumnStyles.Add(YFColStyle);
                DataGridColumnStyle SLColStyle = new DataGridTextBoxColumn();
                SLColStyle.MappingName = "数量";
                SLColStyle.HeaderText = "数量";
                SLColStyle.Width = 30;
                ts.GridColumnStyles.Add(SLColStyle);
                DataGridColumnStyle CWColStyle = new DataGridTextBoxColumn();
                CWColStyle.MappingName = "仓位";
                CWColStyle.HeaderText = "仓位";
                CWColStyle.Width = 60;
                ts.GridColumnStyles.Add(CWColStyle);
                CreateNewCaseDetailGrid();
                dgDifferList.TableStyles.Add(ts);
                dgDifferList.DataSource = null;
                dgDifferList.DataSource = dt;
                dgDifferList.RowHeadersVisible = false;
                AutoSizeTable(dgDifferList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dgDifferList.Select(0);
                }
                dgDifferList.Invalidate();
            }
            catch (Exception ex)
            {
            }
        }

        public void CreateNewCaseDetailGrid()
        {
            this.Controls.Remove(this.dgDifferList);
            this.dgDifferList = new System.Windows.Forms.DataGrid();
            this.dgDifferList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgDifferList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDifferList.Location = new System.Drawing.Point(0, 0);
            this.dgDifferList.Name = "dgDifferList";
            this.dgDifferList.Size = new System.Drawing.Size(238, 270);
            this.dgDifferList.TabIndex = 0;
            this.dgDifferList.CurrentCellChanged += new System.EventHandler(this.dgDifferList_CurrentCellChanged);
            this.dgDifferList.GotFocus += new System.EventHandler(this.dgDifferList_GotFocus);
            this.Controls.Add(this.dgDifferList);
            //激活窗口，并把焦点返回窗口
            this.Activate();
            this.Focus();
        }

        #region 设置高度
        public void SetGridRowHeight(DataGrid dg, int nRow, int cy)
        {
            ArrayList arrRows = (ArrayList)dg.GetType().GetField("m_rlrow",
                            BindingFlags.NonPublic |
                            BindingFlags.Static |
                            BindingFlags.Instance).GetValue(dg);
            object row = arrRows[nRow];
            row.GetType().GetField("m_cy",
                             BindingFlags.NonPublic |
                             BindingFlags.Static |
                             BindingFlags.Instance).SetValue(row, cy);
        }

        public void AutoSizeTable(DataGrid dgData)
        {
            int numCols = dgData.TableStyles[0].GridColumnStyles.Count;
            for (int i = 0; i < numCols; i++)
            {
                if (dgData.TableStyles[0].GridColumnStyles[i].MappingName.Equals("名称"))
                {
                    AutoSizeCol(dgData, i);
                }
            }
        }

        private void AutoSizeCol(DataGrid dgData, int colIndex)
        {
            int rowNums = ((DataTable)dgData.DataSource).Rows.Count;
            Byte[] myByte = System.Text.Encoding.Default.GetBytes(dgData.TableStyles[0].GridColumnStyles[colIndex].HeaderText);
            int textCount = myByte.Length;
            int tempCount = 0;
            for (int i = 0; i < rowNums; i++)
            {
                if (dgData[i, colIndex] != null)
                {
                    myByte = System.Text.Encoding.Default.GetBytes(dgData[i, colIndex].ToString().Trim());
                    tempCount = myByte.Length;
                    if (tempCount > textCount)
                    {
                        int factor = tempCount / textCount;
                        if (tempCount % textCount > 0)
                        {
                            factor++;
                        }
                        SetGridRowHeight(dgData, i, factor * 7);
                    }
                }
            }
        }
        #endregion

        #region 整行选中模式
        private void dgDifferList_GotFocus(object sender, EventArgs e)
        {
            if (dgDifferList != null)
            {
                DataTable dt = (DataTable)dgDifferList.DataSource;
                if (dt != null && dt.Rows.Count > 0)
                {
                    int index = ((DataGrid)sender).CurrentCell.RowNumber;
                    ((DataGrid)sender).Select(index);
                }
            }
        }

        private void dgDifferList_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgDifferList != null)
            {
                DataTable dt = (DataTable)dgDifferList.DataSource;
                if (dt != null && dt.Rows.Count > 0)
                {
                    int index = ((DataGrid)sender).CurrentCell.RowNumber;
                    ((DataGrid)sender).Select(index);
                }
            }
        }
        #endregion

    }
}