﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuideToTheNewEden.Core.Models.WindowPreviews
{
    public class PreviewItem : ObservableObject
    {

        private string title;
        /// <summary>
        /// 窗口标题
        /// 默认为游戏角色名
        /// </summary>
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        /// <summary>
        /// 窗口透明度
        /// 0-100
        /// </summary>
        private int overlapOpacity = 80;
        public int OverlapOpacity
        {
            get => overlapOpacity;
            set => SetProperty(ref overlapOpacity, value);
        }

        private int winX = -1;
        public int WinX
        {
            get => winX;
            set => SetProperty(ref winX, value);
        }

        private int winY = -1;
        public int WinY
        {
            get => winY;
            set => SetProperty(ref winY, value);
        }

        private int winW = 500;
        public int WinW
        {
            get => winW;
            set => SetProperty(ref winW, value);
        }

        private int winH = 500;
        public int WinH
        {
            get => winH;
            set => SetProperty(ref winH, value);
        }

        private bool showTitleBar = true;
        public bool ShowTitleBar
        {
            get => showTitleBar;
            set => SetProperty(ref showTitleBar, value);
        }

        private string hotKey;
        /// <summary>
        /// 快捷键
        /// 以,分隔
        /// </summary>
        public string HotKey
        {
            get => hotKey;
            set => SetProperty(ref hotKey, value);
        }
    }
}
