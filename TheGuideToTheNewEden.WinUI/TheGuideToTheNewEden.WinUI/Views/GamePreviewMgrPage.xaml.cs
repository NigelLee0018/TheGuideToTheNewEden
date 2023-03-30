// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TheGuideToTheNewEden.WinUI.Helpers;
using TheGuideToTheNewEden.WinUI.ViewModels;
using TheGuideToTheNewEden.WinUI.Wins;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace TheGuideToTheNewEden.WinUI.Views
{
    public sealed partial class GamePreviewMgrPage1 : Page
    {
        private BaseWindow Window;
        private Microsoft.UI.Windowing.AppWindow AppWindow;
        public GamePreviewMgrPage1()
        {
            this.InitializeComponent();
            Loaded += GamePreviewMgrPage_Loaded;
        }
        private IntPtr windowHandle = IntPtr.Zero;
        private void GamePreviewMgrPage_Loaded(object sender, RoutedEventArgs e)
        {
            Window = Helpers.WindowHelper.GetWindowForElement(this) as BaseWindow;
            (DataContext as GamePreviewMgrViewModel).Window = Window;
            ProcessList.SelectionChanged += ProcessList_SelectionChanged;
            windowHandle = Helpers.WindowHelper.GetWindowHandle(Window);
            AppWindow = Helpers.WindowHelper.GetAppWindow(Window);
            PreviewGrid.SizeChanged += PreviewGrid_SizeChanged;
        }

        private void PreviewGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateThumbDestination();
        }

        private IntPtr lastThumb = IntPtr.Zero;
        private void ProcessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                var process = e.AddedItems.FirstOrDefault() as Core.Models.GamePreviews.ProcessInfo;
                if(process != null)
                {
                    if(lastThumb != IntPtr.Zero)
                    {
                        WindowCaptureHelper.HideThumb(lastThumb);
                    }
                    lastThumb = WindowCaptureHelper.Show(windowHandle, process.MainWindowHandle);
                    UpdateThumbDestination();
                }
            }
        }
        private void UpdateThumbDestination()
        {
            var thumbSize = WindowCaptureHelper.GetThumbSourceSize(lastThumb);
            var showWPresent = PreviewGrid.ActualWidth / ContentGrid.ActualWidth;
            var showW = showWPresent * (AppWindow.ClientSize.Width - (ContentGrid.Margin.Left + ContentGrid.Margin.Right));
            var showH = showW / thumbSize.x * thumbSize.y;
            int left = (int)(AppWindow.ClientSize.Width * (1- showWPresent));
            int top = (int)(AppWindow.ClientSize.Height / 2 - showH / 2);
            int right = (int)(left + showW);
            int bottom = (int)(top + showH);
            WindowCaptureHelper.UpdateThumbDestination(lastThumb, new WindowCaptureHelper.Rect(left, top, right, bottom));
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                VM.RefreshProcessListCommand.Execute(null);
            }
        }
    }
}
