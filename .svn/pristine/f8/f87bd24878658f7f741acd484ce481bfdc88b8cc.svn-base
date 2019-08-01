using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace HomotorDepotMgr.Utility
{
    /// <summary>
    /// WINCE获取设备ID
    /// </summary>
    public class TerminalInfo
    {
        private const int ERROR_NOT_SUPPORTED = 50;

        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        private static int METHOD_BUFFERED;

        private static int FILE_ANY_ACCESS;

        private static int FILE_DEVICE_HAL;

        private static int IOCTL_HAL_GET_DEVICEID;

        private static int IOCTL_HAL_GET_UUID;

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool KernelIoControl(int dwIoControlCode, IntPtr lpInBuf, int nInBufSize, byte[] lpOutBuf, int nOutBufSize, ref int lpBytesReturned);

        public byte[] UniqueUnitID
        {
            get
            {
                byte[] numArray = new byte[21];
                string deviceID = TerminalInfo.GetDeviceID();
                return Encoding.Default.GetBytes(deviceID);
            }
        }

        static TerminalInfo()
        {
            TerminalInfo.METHOD_BUFFERED = 0;
            TerminalInfo.FILE_ANY_ACCESS = 0;
            TerminalInfo.FILE_DEVICE_HAL = 257;
            TerminalInfo.IOCTL_HAL_GET_DEVICEID = TerminalInfo.FILE_DEVICE_HAL << 16 | TerminalInfo.FILE_ANY_ACCESS << 14 | 84 | TerminalInfo.METHOD_BUFFERED;
            TerminalInfo.IOCTL_HAL_GET_UUID = TerminalInfo.FILE_DEVICE_HAL << 16 | TerminalInfo.FILE_ANY_ACCESS << 14 | 52 | TerminalInfo.METHOD_BUFFERED;
        }

        public TerminalInfo()
        {
        }

        public static string GetDeviceID()
        {
            byte[] numArray = new byte[20];
            bool flag = false;
            int length = (int)numArray.Length;
            BitConverter.GetBytes(length).CopyTo(numArray, 0);
            int num = 0;
            while (!flag)
            {
                if (!TerminalInfo.KernelIoControl(TerminalInfo.IOCTL_HAL_GET_UUID, IntPtr.Zero, 0, numArray, length, ref num))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    int num1 = lastWin32Error;
                    if (num1 == 50)
                    {
                        throw new NotSupportedException("IOCTL_HAL_GET_DEVICEID is not supported on this device", new Win32Exception(lastWin32Error));
                    }
                    if (num1 != 122)
                    {
                        throw new Win32Exception(lastWin32Error, "Unexpected error");
                    }
                    length = BitConverter.ToInt32(numArray, 0);
                    numArray = new byte[length];
                    BitConverter.GetBytes(length).CopyTo(numArray, 0);
                }
                else
                {
                    flag = true;
                }
            }
            BitConverter.ToInt32(numArray, 4);
            BitConverter.ToInt32(numArray, 8);
            int num2 = BitConverter.ToInt32(numArray, 12);
            int num3 = BitConverter.ToInt32(numArray, 16);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 8; i < 16; i++)
            {
                stringBuilder.Append(string.Format("{0:X2}", numArray[i]));
            }
            stringBuilder.Append("-");
            for (int j = num2; j < num2 + num3; j++)
            {
                stringBuilder.Append(string.Format("{0:X2}", numArray[j]));
            }
            return stringBuilder.ToString();
        }
    }  
}
