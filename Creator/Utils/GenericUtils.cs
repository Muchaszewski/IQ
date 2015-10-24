﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Creator.Utils
{
    public static class GenericUtils
    {
        public static void CenterWindow(Window parent, Window target)
        {
            target.Owner = parent;
            target.ShowDialog();
        }

        /// <summary>
        /// Finds the visual parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender">The sender.</param>
        /// <returns></returns>
        public static T FindVisualParent<T>(DependencyObject sender) where T : DependencyObject
        {
            if (sender == null)
            {
                return (null);
            }
            else if (VisualTreeHelper.GetParent(sender) is T)
            {
                return (VisualTreeHelper.GetParent(sender) as T);
            }
            else
            {
                DependencyObject parent = VisualTreeHelper.GetParent(sender);
                return (FindVisualParent<T>(parent));
            }
        }
    }
}
