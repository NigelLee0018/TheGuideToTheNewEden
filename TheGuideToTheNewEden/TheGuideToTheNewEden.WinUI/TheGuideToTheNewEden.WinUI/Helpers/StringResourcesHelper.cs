﻿using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGuideToTheNewEden.WinUI.Helpers
{
    internal static class StringResourcesHelper
    {
        public static string GetString(string key,bool returnKey = true)
        {
            if (Application.Current.Resources.TryGetValue(key, out var str))
            {
                return str.ToString();
            }
            else
            {
                if(returnKey)
                {
                    return key;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
    }
}
