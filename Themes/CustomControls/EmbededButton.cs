using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Themes.CustomControls
{
    public class EmbededButton : Button
    {
        static EmbededButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmbededButton), new FrameworkPropertyMetadata(typeof(EmbededButton)));
        }
    }


    public class EmbededToggleButton : ToggleButton
    {
        static EmbededToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmbededToggleButton), new FrameworkPropertyMetadata(typeof(EmbededToggleButton)));
        }
    }
}

