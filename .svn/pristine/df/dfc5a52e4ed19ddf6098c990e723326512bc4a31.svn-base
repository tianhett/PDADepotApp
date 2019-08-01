using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using D300.System;
using HomotorDepotMgrUpdate.Utility;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace HomotorDepotMgrUpdate
{
    public partial class Form1 : Form
    {
        SQLiteDBHelper db = new SQLiteDBHelper();
        //MsgDialog msgDialog = new MsgDialog();

        public Form1()
        {
            //InitializeComponent();
            SetGlobalVar();
        }

        public void UpdateHandler()
        {
            string startProcess = string.Empty;
            string folder = "HomotorDepotMgr";
            if (D300SysUI.CheckNetworkStatus())
            {
                List<string> fileList = DownloadMgr.GetDownloadFileList(folder);
                if (fileList.Count > 0)
                {
                    //下载更新程序
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        string curFullName = string.Empty;
                        FileInfo newInfo = new FileInfo(fileList[i]);
                        if (!newInfo.DirectoryName.Equals("\\"))
                        {
                            if (!Directory.Exists(GlobalShare.stratPath + newInfo.DirectoryName))
                            {
                                Directory.CreateDirectory(GlobalShare.stratPath + newInfo.DirectoryName);
                            }
                        }
                        curFullName = GlobalShare.stratPath + newInfo.FullName;
                        if (Path.GetExtension(curFullName).Equals(".exe"))
                        {
                            startProcess = curFullName;
                        }
                        DownloadMgr.DownloadFile("/" + folder + newInfo.FullName.Replace("\\", "/"), curFullName);
                        int percent = (int)(((i + 1) * 100) / fileList.Count);
                        Console.WriteLine("应用程序已下载{0}%", percent);
                        //this.Invoke(new EventHandler(delegate
                        //{
                        //    progressBar1.Value = percent;
                        //}));
                    }
                    //找到更新程序的exe，并进行更新
                    SetAppProgramConfig();
                    Process.Start(startProcess, "");
                    //关闭当前程序
                    this.Close();
                    this.Dispose();
                    Application.Exit();
                }
                else
                {
                    //msgDialog.ShowMessage("更新失败，没有找到可更新的程序", 1);
                    Console.WriteLine("更新失败，没有找到可更新的程序");
                }
            }
            else
            {
                //msgDialog.ShowMessage("网络没有连接", 1);
                Console.WriteLine("网络没有连接");
            }
        }

        public void SetGlobalVar()
        {
            try
            {
                //获取应用程序的目录
                string sql = @"select * from AppConfig where Key=@Key";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Key", "AppLocation")
                 };
                DataTable dt = db.ExecuteDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GlobalShare.stratPath = dt.Rows[0].ItemArray[2].ToString();
                }
                //获取端口号
                parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Key", "Port")
                 };
                dt = db.ExecuteDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    GlobalShare.port = dt.Rows[0].ItemArray[2].ToString();
                }
                //获取IP地址
                parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Key", "IP")
                 };
                dt = db.ExecuteDataTable(sql, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string rs = dt.Rows[0].ItemArray[2].ToString();
                    GlobalShare.ipAddress = rs;
                    if (Regex.IsMatch(rs, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$"))
                    {
                        //IP地址检测
                        GlobalShare.downloadUrl = rs + ":" + GlobalShare.port;
                    }
                    else if (Regex.IsMatch(rs, @"^([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$"))
                    {
                        //URL检测
                        GlobalShare.downloadUrl = rs;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void SetAppProgramConfig()
        {
            try
            {
                SQLiteDBHelper dbApp = new SQLiteDBHelper(GlobalShare.stratPath);
                string sql = @"update AppConfig set Value=@Value where Key=@Key";
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Key", "IP"),
                    new SQLiteParameter("@Value", GlobalShare.ipAddress)
                 };
                dbApp.ExecuteNonQuery(sql, parameters);
                parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Key", "Port"),
                    new SQLiteParameter("@Value", GlobalShare.port)
                 };
                dbApp.ExecuteNonQuery(sql, parameters);
                sql = @"select * from LoginInfo";
                DataTable dt = db.ExecuteDataTable(sql, null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sql = @"insert into LoginInfo(LoginID,CreateTime) values(@LoginID,@CreateTime)";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        parameters = new SQLiteParameter[]{
                            new SQLiteParameter("@LoginID", dt.Rows[i].ItemArray[1].ToString()),
                            new SQLiteParameter("@CreateTime", dt.Rows[i].ItemArray[2].ToString())
                         };
                        dbApp.ExecuteNonQuery(sql, parameters);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}