using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace HomotorDepotMgr.Utility
{
    public class TimeService
    {
        public static DateTime GetStandardTime()
        {
            //<?xml version="1.0" encoding="GB2312" ?> 
            //- <ntsc>
            //- <time>
            //  <year>2011</year> 
            //  <month>7</month> 
            //  <day>10</day> 
            //  <Weekday /> 
            //  <hour>19</hour> 
            //  <minite>45</minite> 
            //  <second>37</second> 
            //  <Millisecond /> 
            //  </time>
            //  </ntsc>
            DateTime dt;
            WebRequest wrt = null;
            WebResponse wrp = null;
            try
            {
                wrt = WebRequest.Create("http://www.time.ac.cn/timeflash.asp?user=flash");     //国家授时中心
                wrt.Credentials = CredentialCache.DefaultCredentials;

                wrp = wrt.GetResponse();
                StreamReader sr = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8);
                string html = sr.ReadToEnd();

                sr.Close();
                wrp.Close();

                int yearIndex = html.IndexOf("<year>") + 6;
                int monthIndex = html.IndexOf("<month>") + 7;
                int dayIndex = html.IndexOf("<day>") + 5;
                int hourIndex = html.IndexOf("<hour>") + 6;
                int miniteIndex = html.IndexOf("<minite>") + 8;
                int secondIndex = html.IndexOf("<second>") + 8;

                string year = html.Substring(yearIndex, html.IndexOf("</year>") - yearIndex);
                string month = html.Substring(monthIndex, html.IndexOf("</month>") - monthIndex); ;
                string day = html.Substring(dayIndex, html.IndexOf("</day>") - dayIndex);
                string hour = html.Substring(hourIndex, html.IndexOf("</hour>") - hourIndex);
                string minite = html.Substring(miniteIndex, html.IndexOf("</minite>") - miniteIndex);
                string second = html.Substring(secondIndex, html.IndexOf("</second>") - secondIndex);
                dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            }
            catch (Exception)
            {
                string result = GetBaiduNetDateTime();    //百度时间
                return DateTime.Parse(result);
            }
            finally
            {
                if (wrp != null)
                    wrp.Close();
                if (wrt != null)
                    wrt.Abort();
            }
            return dt;
        }

        public static string GetBaiduNetDateTime()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("http://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { 
                    if (h == "Date")
                    { 
                        datetime = headerCollection[h]; 
                    }
                }
                return datetime;
            }
            catch (Exception) 
            { 
                return datetime; 
            }
            finally
            {
                if (request != null)
                { 
                    request.Abort(); 
                }
                if (response != null)
                { 
                    response.Close(); 
                }
                if (headerCollection != null)
                { 
                    headerCollection.Clear(); 
                }
            }
        }


        [DllImport("coredll.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME lpSystemTime);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        public static void SetSystemLocalTime()
        {
            try
            {
                DateTime dt = GetStandardTime();
                SYSTEMTIME systNew = new SYSTEMTIME();
                systNew.wDay = Convert.ToUInt16(dt.Day);
                systNew.wMonth = Convert.ToUInt16(dt.Month);
                systNew.wYear = Convert.ToUInt16(dt.Year);
                systNew.wHour = Convert.ToUInt16(dt.Hour);
                systNew.wMinute = Convert.ToUInt16(dt.Minute);
                systNew.wSecond = Convert.ToUInt16(dt.Second);
                SetLocalTime(ref systNew);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
