﻿using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TheGuideToTheNewEden.Core.Models.EVELogs;
using TheGuideToTheNewEden.WinUI.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using TheGuideToTheNewEden.Core.Extensions;
using Microsoft.UI;

namespace TheGuideToTheNewEden.WinUI.Views
{
    public sealed partial class EarlyWarningItemPage : Page
    {
        private TabViewItem TabViewItem;
        public EarlyWarningItemPage(TabViewItem tabViewItem)
        {
            TabViewItem = tabViewItem;
            this.InitializeComponent();
            Loaded += EarlyWarningItemPage_Loaded;
        }

        private void EarlyWarningItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as EarlyWarningItemViewModel).OnSelectedCharacterChanged += EarlyWarningItemPage_OnSelectedCharacterChanged;
            ChatContents.Blocks.Add(new Paragraph());
            (this.DataContext as EarlyWarningItemViewModel).ChatContents.CollectionChanged += ChatContents_CollectionChanged;
            ChatContentsScroll.LayoutUpdated += ChatContentsScroll_LayoutUpdated;
        }
        private void EarlyWarningItemPage_OnSelectedCharacterChanged(string selectedCharacter)
        {
            TabViewItem.Header = selectedCharacter;
        }

        private void ChatContentsScroll_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChatContentsScroll.ScrollToVerticalOffset(ChatContentsScroll.ScrollableHeight);
        }
        private bool isAdded = false;
        private void ChatContents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var chatContent = item as ChatContent;
                    Paragraph paragraph = new Paragraph()
                    {
                        Margin = new Thickness(0, 8, 0, 8),
                    };
                    Run timeRun = new Run()
                    {
                        FontWeight = FontWeights.Light,
                        Text = $"[ {chatContent.EVETime} ]"
                    };
                    Run nameRun = new Run()
                    {
                        FontWeight = FontWeights.Medium,
                        Text = $" {chatContent.SpeakerName} > "
                    };
                    Run contentRun = new Run()
                    {
                        FontWeight = FontWeights.Normal,
                        Text = chatContent.Content
                    };
                    if (chatContent.Important)
                    {
                        contentRun.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    }
                    paragraph.Inlines.Add(timeRun);
                    paragraph.Inlines.Add(nameRun);
                    paragraph.Inlines.Add(contentRun);
                    if (ChatContentsScroll.VerticalOffset == ChatContentsScroll.ScrollableHeight)
                    {
                        isAdded = true;
                    }
                    ChatContents.Blocks.Add(paragraph);
                }
            }
            else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                ChatContents.Blocks.Clear();
            }
        }

        private void ChatContentsScroll_LayoutUpdated(object sender, object e)
        {
            if(isAdded)
            {
                isAdded = false;
                ChatContentsScroll.ScrollToVerticalOffset(ChatContentsScroll.ScrollableHeight);
            }
        }

        public void Stop()
        {
            (DataContext as ViewModels.EarlyWarningItemViewModel).StopCommand.Execute(null);
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EarlyWarningItemViewModel).SelectedNameDbs = (sender as ListView).SelectedItems?.ToList<string>();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedItems.Add((sender as ListView).Items.FirstOrDefault());
        }
    }
}
