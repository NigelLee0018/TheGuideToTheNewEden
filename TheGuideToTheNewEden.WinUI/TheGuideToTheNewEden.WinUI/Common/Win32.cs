﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TheGuideToTheNewEden.WinUI.Common

{
    internal static class Win32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(IntPtr moduleName);

        public const int WM_ACTIVATE = 0x0006;
        public const int WA_ACTIVE = 0x01;
        //static int WA_CLICKACTIVE = 0x02;
        public const int WA_INACTIVE = 0x00;

        public const int WM_SETICON = 0x0080;
        public const int ICON_SMALL = 0;
        public const int ICON_BIG = 1;

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 显示窗口操作
        /// https://www.cnblogs.com/PLM-Teamcenter/p/15726204.html
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow">
        /// 0 关闭窗口
        /// 1 正常大小显示窗口
        /// 2 最小化窗口
        /// 3 最大化窗口</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
