using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HomotorDepotMgr.Utility
{
    public class MutexHelper
    {
        [DllImport("coredll.dll", EntryPoint = "CreateMutex", SetLastError = true)]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool InitialOwner, string MutexName);

        [DllImport("coredll.dll", EntryPoint = "ReleaseMutex", SetLastError = true)]
        public static extern bool ReleaseMutex(IntPtr hMutex);

        private const int ERROR_ALREADY_EXISTS = 0183;

        /// <summary>
        /// 判断程序是否已经运行
        /// </summary>
        /// <returns>
        /// true: 程序已运行
        /// false: 程序未运行
        /// </returns>
        public static bool IsApplicationOnRun()
        {
            string strAppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            IntPtr hMutex = CreateMutex(IntPtr.Zero, true, strAppName);
            if (hMutex == IntPtr.Zero)
            {
                throw new ApplicationException("Failure creating mutex: " + Marshal.GetLastWin32Error().ToString("X"));
            }
            if (Marshal.GetLastWin32Error() == ERROR_ALREADY_EXISTS)
            {
                ReleaseMutex(hMutex);
                return true;
            }
            return false;
        }

    }
}
