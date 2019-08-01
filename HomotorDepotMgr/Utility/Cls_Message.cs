using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HomotorDepotMgr.Utility
{
    class Cls_Message
    {
        private struct COPYDATASTRUCT
        {
            public int dwData;
            public int cbData;
            public IntPtr lpData;
        }
        //-------------------------------------------------------------------------------  
        private const int WM_COPYDATA = 0x004A;
        private const int GWL_WNDPROC = -4;
        private const int LMEM_FIXED = 0x0000;
        private const int LMEM_ZEROINIT = 0x0040;
        private const int LPTR = (LMEM_FIXED | LMEM_ZEROINIT);
        private IntPtr oldWndProc = IntPtr.Zero;
        private WndProcDelegate newWndProc;
        private IntPtr formHandle;
        //-------------------------------------------------------------------------------  
        delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("coredll.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("coredll.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("coredll.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newWndProc);
        [DllImport("coredll.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("coredll.dll")]
        private static extern int SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("coredll.dll")]
        private static extern IntPtr LocalAlloc(int flag, int size);
        [DllImport("coredll.dll")]
        private static extern IntPtr LocalFree(IntPtr p);

        public delegate void MessageHandler(IntPtr hWnd, string message);
        public event MessageHandler MessageHandlerDelegate;

        /// <summary>  
        /// 初始化消息类  
        /// </summary>  
        /// <param name="handle">接受消息的窗体的句柄</param>  
        public Cls_Message(IntPtr formHandle)
        {
            this.formHandle = formHandle;
            newWndProc = new WndProcDelegate(WndProc);
            oldWndProc = GetWindowLong(formHandle, GWL_WNDPROC);
            int success = SetWindowLong(formHandle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc));
        }
        /// <summary>  
        /// 消息处理  
        /// </summary>  
        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_COPYDATA)
            {
                COPYDATASTRUCT st = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                string str = Marshal.PtrToStringUni(st.lpData);
                MessageHandlerDelegate(hWnd, str);
                //MsgDialog dialog = new MsgDialog();
                //dialog.ShowMessage(str, 2);
            }
            return CallWindowProc(oldWndProc, this.formHandle, msg, wParam, lParam);
        }

        static private IntPtr AllocHGlobal(int cb)
        {
            IntPtr hMemory = new IntPtr();
            hMemory = LocalAlloc(LPTR, cb);
            return hMemory;
        }
        static private void FreeHGlobal(IntPtr hMemory)
        {
            if (hMemory != IntPtr.Zero)
                LocalFree(hMemory);
        }
        /// <summary>  
        /// 发送消息  
        /// </summary>  
        /// <param name="formTitle">目标窗体的名称</param>  
        /// <param name="message">消息内容</param>  
        static public void SendMessage(String formTitle, String message)
        {
            IntPtr hWndDest = FindWindow(null, formTitle);
            COPYDATASTRUCT oCDS = new COPYDATASTRUCT();
            oCDS.cbData = (message.Length + 1) * 2;
            oCDS.lpData = LocalAlloc(LPTR, oCDS.cbData);
            Marshal.Copy(message.ToCharArray(), 0, oCDS.lpData, message.Length);
            oCDS.dwData = 1;
            IntPtr lParam = AllocHGlobal(oCDS.cbData);
            Marshal.StructureToPtr(oCDS, lParam, false);
            SendMessageW(hWndDest, WM_COPYDATA, IntPtr.Zero, lParam);
            LocalFree(oCDS.lpData);
            FreeHGlobal(lParam);
        }  
    }
}
