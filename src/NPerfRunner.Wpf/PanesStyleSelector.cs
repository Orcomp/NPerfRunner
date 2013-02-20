using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace NPerfRunner.Wpf
{
    using System.Windows;

    using NPerfRunner.Wpf.ViewModels;

    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle
        {
            get;
            set;
        }

        public Style ChartStyle
        {
            get;
            set;
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ToolViewModel)
                return ToolStyle;

            if (item is ChartViewModel)
                return ChartStyle;

            return base.SelectStyle(item, container);
        }
    }
}
