using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathSpace.View
{
    /// <summary>
    /// Questions.xaml 的交互逻辑
    /// </summary>
    public partial class Questions : UserControl
    {
        public Questions()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var win = Application.Current.MainWindow as MainWindow;
            if (null != win)
            {
                win.OnPageSwitchEvent(Model.ContentPages.Chapters);
            }
        }

        private void Question_Click(object sender, RoutedEventArgs e)
        {
            var win = Application.Current.MainWindow as MainWindow;
            if (null != win)
            {
                win.OnPageSwitchEvent(Model.ContentPages.AnswerQuestion);
            }
        }
    }
}
