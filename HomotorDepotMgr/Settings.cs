using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HomotorDepotMgr.Model;
using HomotorDepotMgr.Utility;
using System.Data.SQLite;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using D300.System;
using Newtonsoft.Json;
using System.Data.Common;

namespace HomotorDepotMgr
{
    public partial class Settings : Form
    {
        public delegate void GetCaseSettings(int selection, SettingModel setting);
        public event GetCaseSettings GetCaseSettingsDelegate;

        Hook hkSettings = new Hook("Settings");
        SQLiteDBHelper db = new SQLiteDBHelper();
        MsgDialog msgDialog = new MsgDialog();
        string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        private int inputFocusIndex = 0;

        public Settings()
        {
            InitializeComponent();
            ReloadSettings();
            hkSettings.KeyHandlerDelegate += new Hook.KeyHandler(hkSettings_KeyHandlerDelegate);
            hkSettings.Start();
        }

        private void Settings_GotFocus(object sender, EventArgs e)
        {
            //hkSettings.Start();
        }

        int hkSettings_KeyHandlerDelegate(int vkCode, string clsName)
        {
            if (clsName.Equals("Settings"))
            {
                if (vkCode == VirtualKey.VK_ENTER)
                {
                    SettingsOKHandler();
                    return -1;
                }
                if (vkCode == VirtualKey.VK_ESCAPE)
                {
                    SettingsCancelHandler();
                    return -1;
                }
            }
            return 0;
        }

        public void SettingsOKHandler()
        {
            hkSettings.Stop();
            GetCaseSettingsDelegate(1, new SettingModel() { IP = txtIPAddress.Text, Port = txtPort.Text });
            this.Close();
        }

        public void SettingsCancelHandler()
        {
            hkSettings.Stop();
            GetCaseSettingsDelegate(0, new SettingModel());
            this.Close();
        }

        private void btnSettingOK_Click(object sender, EventArgs e)
        {
            SettingsOKHandler();
        }

        private void btnSettingCancel_Click(object sender, EventArgs e)
        {
            SettingsCancelHandler();
        }

        public void ReloadSettings()
        {
            SQLiteDBHelper db = new SQLiteDBHelper();
            string sql = @"select * from AppConfig where Key=@Key";
            SQLiteParameter[] parameters = new SQLiteParameter[]{
                new SQLiteParameter("@Key", "IP")
             };
            DataTable dt = db.ExecuteDataTable(sql, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                txtIPAddress.Text = dt.Rows[0].ItemArray[2].ToString();
            }
            parameters = new SQLiteParameter[]{
                new SQLiteParameter("@Key", "Port")
             };
            dt = db.ExecuteDataTable(sql, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                txtPort.Text = dt.Rows[0].ItemArray[2].ToString();
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            hkSettings.Stop();
            MsgBox prompt = new MsgBox("是否确定要清除登录历史？", "警告", 3);
            prompt.ConfirmSelectionDelegate += delegate(int selection)
            {
                if (selection == 1)
                {
                    try
                    {
                        string sql = @"delete from LoginInfo";
                        db.ExecuteNonQuery(sql, null);
                        msgDialog.ShowMessage("清除成功", 1);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                hkSettings.Start();
            };
            prompt.Show();
        }

        private void btnUpdateApp_Click(object sender, EventArgs e)
        {
            hkSettings.Stop();
            MsgBox prompt = new MsgBox("更新会清除数据，是否确定要更新？", "警告", 3);
            prompt.ConfirmSelectionDelegate += delegate(int selection)
            {
                if (selection == 1)
                {
                    UpdateAppHandler();
                }
                hkSettings.Start();
            };
            prompt.Show();
        }

        public void UpdateAppHandler()
        {
            string startProcess = string.Empty;
            string folder = "HomotorDepotMgrUpdate";
            string updatePath = Path.GetDirectoryName(path) + "\\" + folder;
            if (Directory.Exists(updatePath))
            {
                //找到更新程序的exe，并进行更新
                DirectoryInfo root = new DirectoryInfo(updatePath);
                if (root.GetFiles().Length > 0)
                {
                    foreach (var fn in root.GetFiles())
                    {
                        if (Path.GetExtension(fn.FullName).Equals(".exe"))
                        {
                            startProcess = fn.FullName;
                            break;
                        }
                    }
                    File.Copy(path + "\\appDB.db", updatePath + "\\appDB.db", true);
                    Process.Start(startProcess, "");
                    //关闭当前程序
                    GlobalShare.LoginID = string.Empty;
                    this.Close();
                    this.Dispose();
                    Application.Exit();
                }
            }
            else
            {
                try
                {
                    if (D300SysUI.CheckNetworkStatus())
                    {
                        msgDialog.ShowMessage("正在下载更新程序，请稍后···", 1);
                        List<string> fileList = DataUpDownload.GetDownloadFileList(folder);
                        if (fileList.Count > 0)
                        {
                            float factor = 0.25f;
                            if (!Directory.Exists(updatePath))
                            {
                                Directory.CreateDirectory(updatePath);
                            }
                            //下载更新程序
                            for (int i = 0; i < fileList.Count; i++)
                            {
                                string curFullName = string.Empty;
                                FileInfo newInfo = new FileInfo(fileList[i]);
                                if (!newInfo.DirectoryName.Equals("\\"))
                                {
                                    if (!Directory.Exists(updatePath + newInfo.DirectoryName))
                                    {
                                        Directory.CreateDirectory(updatePath + newInfo.DirectoryName);
                                    }
                                }
                                curFullName = updatePath + newInfo.FullName;
                                if (Path.GetExtension(curFullName).Equals(".exe"))
                                {
                                    startProcess = curFullName;
                                }
                                DataUpDownload.DownloadFile("/" + folder + newInfo.FullName.Replace("\\", "/"), curFullName);
                                int percent = (int)(fileList.Count * factor);
                                if ((i + 1) == percent)
                                {
                                    if ((i + 1) == fileList.Count)
                                    {
                                        msgDialog.ShowMessage("更新程序下载完成，立即启动更新", 1);
                                    }
                                    else
                                    {
                                        msgDialog.ShowMessage("更新程序已下载" + (factor * 100) + "%", 1);
                                        factor = factor + 0.25f;
                                    }
                                }
                            }
                            //找到更新程序的exe，并进行更新
                            File.Copy(path + "\\appDB.db", updatePath + "\\appDB.db", true);
                            Process.Start(startProcess, "");
                            //关闭当前程序
                            GlobalShare.LoginID = string.Empty;
                            this.Close();
                            this.Dispose();
                            Application.Exit();
                        }
                        else
                        {
                            msgDialog.ShowMessage("更新失败，没有找到更新程序", 1);
                        }
                    }
                    else
                    {
                        msgDialog.ShowMessage("网络没有连接", 1);
                    }
                }
                catch (Exception ex)
                {
                    msgDialog.ShowMessage(ex.Message, 1);
                }
            }
        }

        private void btnUpProdData_Click(object sender, EventArgs e)
        {
            SQLiteDBHelper helper = new SQLiteDBHelper("prodDB");
            string res = DataUpDownload.GetMaxProductSeedID();
            if (!string.IsNullOrEmpty(res) && !res.Equals("\"\"") && !res.Equals("Failed"))
            {
                bool configFlag = false;
                int localSeedID = 0;
                int remoteSeedID = Convert.ToInt32(res);
                string tsql = @"select * from ProdConfig where Key=@Key";
                SQLiteParameter[] tparameters = new SQLiteParameter[] {
                    new SQLiteParameter("@Key", "SeedID")
                };
                DataTable partsDt = helper.ExecuteDataTable(tsql, tparameters);
                if (partsDt != null && partsDt.Rows.Count > 0)
                {
                    localSeedID = Convert.ToInt32(partsDt.Rows[0]["Value"]);
                    configFlag = true;
                }
                if (localSeedID != remoteSeedID)
                {
                    string result = DataUpDownload.GetProductTotal();
                    if (!string.IsNullOrEmpty(result) && !result.Equals("\"\"") && !result.Equals("Failed"))
                    {
                        int total = Convert.ToInt32(result);
                        if (total > 0)
                        {
                            AlertBoxHelper alertHelper = new AlertBoxHelper();
                            alertHelper.Show("产品更新中，请稍后···", "产品更新中，请稍后···", 1, -1, "ProductDataUpdating");
                            //清空表
                            string sql = @"delete from APISysProduct";
                            helper.ExecuteNonQuery(sql, null);
                            //插入记录
                            int size = 1000;
                            int count = (int)(total / size);
                            int modCount = total % size;
                            if (modCount > 0)
                            {
                                count = count + 1;
                            }
                            for (int i = 0; i < count; i++)
                            {
                                int start = i * size;
                                string json = DataUpDownload.GetProductDataByPartition(start, size);
                                if (!string.IsNullOrEmpty(json) && !json.Equals("\"\"") && !json.Equals("Failed"))
                                {
                                    List<vAPISysProduct> dataList = JsonConvert.DeserializeObject<List<vAPISysProduct>>(json);
                                    if (dataList.Count > 0)
                                    {
                                        using (SQLiteConnection connection = new SQLiteConnection(helper.ConnectionString))
                                        {
                                            connection.Open();
                                            using (DbTransaction transaction = connection.BeginTransaction())
                                            {
                                                try
                                                {
                                                    using (SQLiteCommand command = new SQLiteCommand(connection))
                                                    {
                                                        foreach (var each in dataList)
                                                        {
                                                            sql = @"insert into APISysProduct(ID,Model,ProdName,ClassName,ProdCode,Brands,OrigCode,FMSICode,Norm,Material,Unit,PlaceofOrigin,GW,Size,Parameters,SuitVehicles,NormNum,Barcode,BoxBarcode,bJianMaInBox,SeedID)  
                                                                                                                        values(@ID,@Model,@ProdName,@ClassName,@ProdCode,@Brands,@OrigCode,@FMSICode,@Norm,@Material,@Unit,@PlaceofOrigin,@GW,@Size,@Parameters,@SuitVehicles,@NormNum,@Barcode,@BoxBarcode,@bJianMaInBox,@SeedID)";
                                                            command.CommandText = sql;
                                                            command.Parameters.Add(new SQLiteParameter("@ID", each.ID));
                                                            command.Parameters.Add(new SQLiteParameter("@Model", each.Model));
                                                            command.Parameters.Add(new SQLiteParameter("@ProdName", each.ProdName));
                                                            command.Parameters.Add(new SQLiteParameter("@ClassName", each.ClassName));
                                                            command.Parameters.Add(new SQLiteParameter("@ProdCode", each.ProdCode));
                                                            command.Parameters.Add(new SQLiteParameter("@Brands", each.Brands));
                                                            command.Parameters.Add(new SQLiteParameter("@OrigCode", each.OrigCode));
                                                            command.Parameters.Add(new SQLiteParameter("@FMSICode", each.FMSICode));
                                                            command.Parameters.Add(new SQLiteParameter("@Norm", each.Norm));
                                                            command.Parameters.Add(new SQLiteParameter("@Material", each.Material));
                                                            command.Parameters.Add(new SQLiteParameter("@Unit", each.Unit));
                                                            command.Parameters.Add(new SQLiteParameter("@PlaceofOrigin", each.PlaceofOrigin));
                                                            command.Parameters.Add(new SQLiteParameter("@GW", each.GW));
                                                            command.Parameters.Add(new SQLiteParameter("@Size", each.Size));
                                                            command.Parameters.Add(new SQLiteParameter("@Parameters", each.Parameters));
                                                            command.Parameters.Add(new SQLiteParameter("@SuitVehicles", each.SuitVehicles));
                                                            command.Parameters.Add(new SQLiteParameter("@NormNum", each.NormNum));
                                                            command.Parameters.Add(new SQLiteParameter("@Barcode", each.Barcode));
                                                            command.Parameters.Add(new SQLiteParameter("@BoxBarcode", each.BoxBarcode));
                                                            command.Parameters.Add(new SQLiteParameter("@bJianMaInBox", each.bJianMaInBox));
                                                            command.Parameters.Add(new SQLiteParameter("@SeedID", each.SeedID));
                                                            command.ExecuteNonQuery();
                                                        }
                                                    }
                                                    transaction.Commit();
                                                }
                                                catch (Exception ex)
                                                {
                                                    transaction.Rollback();
                                                    throw ex;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            string configSql = string.Empty;
                            if (configFlag)
                            {
                                configSql = @"update ProdConfig set Value=(select MAX(SeedID) from APISysProduct) where Key='SeedID'";
                            }
                            else
                            {
                                configSql = @"insert into ProdConfig(Key,Value) select 'SeedID',(select MAX(SeedID) from APISysProduct)";
                            }
                            helper.ExecuteNonQuery(configSql, null);

                            alertHelper.Hide("ProductDataUpdating");
                            msgDialog.ShowMessage("产品数据更新成功", 1);
                        }
                        else
                        {
                            msgDialog.ShowMessage("产品数据更新失败", 1);
                        }
                    }
                    else
                    {
                        msgDialog.ShowMessage("产品数据更新失败", 1);
                    }
                }
                else
                {
                    msgDialog.ShowMessage("产品数据无变动", 1);
                }
            }
            else
            {
                msgDialog.ShowMessage("产品数据更新失败", 1);
            }
        }

        private void btnAlphabet_Click(object sender, EventArgs e)
        {
            hkSettings.Stop();
            AlphabetComplex letterFrm = new AlphabetComplex();
            letterFrm.GetConfirmLetterDelegate += new AlphabetComplex.GetConfirmLetter(letterFrm_GetConfirmLetterDelegate);
            letterFrm.Show();
        }

        void letterFrm_GetConfirmLetterDelegate(int selection, string letter)
        {
            if (selection == 1)
            {
                if (inputFocusIndex == 0)
                {
                    txtIPAddress.Text = letter;
                    txtIPAddress.Focus();
                    txtIPAddress.SelectionStart = txtIPAddress.Text.Length;
                }
                else if (inputFocusIndex == 1)
                {
                    txtPort.Text = letter;
                    txtPort.Focus();
                    txtPort.SelectionStart = txtPort.Text.Length;
                }
            }
            hkSettings.Start();
        }

        private void txtIPAddress_GotFocus(object sender, EventArgs e)
        {
            inputFocusIndex = 0;
        }

        private void txtPort_GotFocus(object sender, EventArgs e)
        {
            inputFocusIndex = 1;
        }

        //#region 检测最新版本
        //public bool isNewVersion()
        //{
        //    bool flag = false;
        //    try
        //    {
        //        if (D300SysUI.CheckNetworkStatus())
        //        {
        //            string exeFile = string.Empty;
        //            DateTime curVersion = DateTime.Now;
        //            TimeService.SetSystemLocalTime();
        //            DirectoryInfo root = new DirectoryInfo(path);
        //            if (root.GetFiles().Length > 0)
        //            {
        //                foreach (var fn in root.GetFiles())
        //                {
        //                    if (Path.GetExtension(fn.FullName).Equals(".exe"))
        //                    {
        //                        exeFile = fn.FullName;
        //                        curVersion = fn.LastWriteTime;
        //                        break;
        //                    }
        //                }
        //            }
        //            string newVersion = DataUpDownload.GetLatestVersionStr();
        //            if (!string.IsNullOrEmpty(newVersion))
        //            {
        //                if (DateTime.Parse(newVersion) > curVersion)
        //                {
        //                    msgDialog.ShowMessage("有最新版本，请到【设置】里更新", 1);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            msgDialog.ShowMessage("网络没有连接，请检查网络", 1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return flag;
        //}
        //#endregion

    }
}