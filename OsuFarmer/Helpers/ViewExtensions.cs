using Avalonia;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Helpers
{
    public static class ViewExtensions
    {
        /// <summary>
        /// Gets the page to which an element belongs
        /// </summary>
        /// <returns>The page.</returns>
        /// <param name="element">Element.</param>
        //public static Page GetParentPage(this VisualElement element)
        //{
        //    if (element != null)
        //    {
        //        var parent = element.Parent;
        //        while (parent != null)
        //        {
        //            if (parent is Page)
        //            {
        //                return parent as Page;
        //            }
        //            parent = parent.Parent;
        //        }
        //    }
        //    return null;
        //}
    }

    public class ProgressBarWorkaround
    {
        public static AvaloniaProperty<double> ValueProperty =
            AvaloniaProperty.RegisterAttached<ProgressBarWorkaround, ProgressBar, double>("Value");

        public static void SetValue(ProgressBar pb, double value) =>
            pb.SetValue(ValueProperty, value);

        static ProgressBarWorkaround()
        {
            ValueProperty.Changed.Subscribe(ev =>
            {
                ((ProgressBar)ev.Sender).Value = ev.NewValue.Value;
            });
        }
    }
}
