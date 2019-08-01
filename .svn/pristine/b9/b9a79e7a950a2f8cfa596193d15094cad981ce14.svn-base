using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HomotorDepotMgr.Utility
{
    public class Hook
    {
        public delegate int KeyHandler(int vkCode, string clsName);
        public event KeyHandler KeyHandlerDelegate;

        int hHookKey;
        public delegate int HookKeyProc(int code, IntPtr wParam, IntPtr lParam);

        private HookKeyProc hookKeyDeleg;

        private string clsName;

        public Hook(string clsName)
        {
            this.clsName = clsName; 
        }

        #region public methods

        //安装钩子
        public void Start()
        {
            if (hHookKey != 0)
            {
                //Unhook the previouse one
                this.Stop();
            }
            hookKeyDeleg = new HookKeyProc(HookKeyProcedure);
            hHookKey = SetWindowsHookEx(WH_KEYBOARD_LL, hookKeyDeleg, GetModuleHandle(null), 0);
            if (hHookKey == 0)
            {
                throw new SystemException("Failed acquiring of the hook.");
            }
        }

        //拆除钩子
        public void Stop()
        {
            UnhookWindowsHookEx(hHookKey);
        }

        #endregion

        #region protected and private methods

        private int HookKeyProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            KBDLLHOOKSTRUCT hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            if (code < 0)
                return CallNextHookEx(hookKeyDeleg, code, wParam, lParam);
            if (wParam.ToInt32() == WM_KEYDOWN)  //只处理按键按下的事件
            {
                int result = KeyHandlerDelegate(hookStruct.vkCode, this.clsName);
                if (result == -1)
                {
                    return -1; //返回-1表示已经处理了，不再往下传递
                }
            }
            
            //if (hookStruct.vkCode == 0x5B)
            //{
            //    //如果捕捉到VK_LWIN按键 
            //    //......处理......             

            //    return -1; //返回-1表示已经处理了，不再往下传递
            //}

            // 没处理的键的消息往下传递
            return CallNextHookEx(hookKeyDeleg, code, wParam, lParam);
        }

        #endregion

        #region P/Invoke declarations

        [DllImport("coredll.dll")]

        private static extern int SetWindowsHookEx(int type, HookKeyProc HookKeyProc, IntPtr hInstance, int m);
        //private static extern int SetWindowsHookEx(int type, HookMouseProc HookMouseProc, IntPtr hInstance, int m);


        [DllImport("coredll.dll")]

        private static extern IntPtr GetModuleHandle(string mod);

        [DllImport("coredll.dll")]

        private static extern int CallNextHookEx(

                HookKeyProc hhk,

                int nCode,

                IntPtr wParam,

                IntPtr lParam

                );

        [DllImport("coredll.dll")]

        private static extern int GetCurrentThreadId();

        [DllImport("coredll.dll", SetLastError = true)]

        private static extern int UnhookWindowsHookEx(int idHook);

        private struct KBDLLHOOKSTRUCT
        {

            public int vkCode;

            public int scanCode;

            public int flags;

            public int time;

            public IntPtr dwExtraInfo;

        }

        const int WH_KEYBOARD_LL = 20;

        const int WM_KEYDOWN = 0x100;

        const int WM_KEYUP = 0x101;

        const int WM_SYSKEYDOWN = 0x104;

        const int WM_SYSKEYUP = 0x105;

        public class KeyBoardInfo
        {

            public int vkCode;

            public int scanCode;

            public int flags;

            public int time;

        }

        #endregion

    }

} 
