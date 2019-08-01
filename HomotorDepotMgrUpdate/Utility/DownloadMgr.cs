﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace HomotorDepotMgrUpdate.Utility
{
    public class TrustCertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest req, int problem)
        {
            return true;
        }
    }

    public class DownloadMgr
    {
        public static List<string> GetDownloadFileList(string folderName)
        {
            List<string> fileNameList = new List<string>();
            try
            {
                System.Net.ServicePointManager.CertificatePolicy = new TrustCertificatePolicy();
                string getString = string.Empty;
                string url = "https://" + GlobalShare.downloadUrl + "/KCHmt/HmtInvoiceApi/DownloadApp?name=" + folderName;
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Accept = "*/*";
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                httpWebRequest.Method = "GET";
                WebResponse webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream getStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(getStream, Encoding.UTF8);
                getString = streamReader.ReadToEnd();
                fileNameList = getString.Split(new char[] { ',' }).ToList();
                streamReader.Close();
                getStream.Close();
            }
            catch (Exception ex)
            {
            }
            return fileNameList;
        }

        public static void DownloadFile(string fileName, string saveName)
        {
            try
            {
                System.Net.ServicePointManager.CertificatePolicy = new TrustCertificatePolicy();
                string url = "https://" + GlobalShare.downloadUrl + "/PDAApp" + fileName;
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                hwr.AddRange(0, 10000000);
                using (Stream stream = hwr.GetResponse().GetResponseStream())
                {
                    using (FileStream fs = File.Create(saveName))
                    {
                        byte[] bytes = new byte[102400];
                        int n = 1;
                        while (n > 0)
                        {
                            n = stream.Read(bytes, 0, 10240);
                            fs.Write(bytes, 0, n);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
