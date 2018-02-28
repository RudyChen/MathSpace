using MathSpace.Model;
using MathSpace.View;
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

namespace MathSpace
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private Achievement achievement;

        private Chapters chapters;

        private Questions questions;

        private MathEidtor questionEidtor;

        /// <summary>
        /// 主页内容页切换事件
        /// </summary>
        /// <param name="switchToPage"></param>
        public delegate void PageSwitchEventHandler(ContentPages switchToPage);
        /// <summary>
        /// 
        /// </summary>
        public event PageSwitchEventHandler PageSwitchEvent;

        /// <summary>
        /// 触发页面切换事件
        /// </summary>
        public void OnPageSwitchEvent(ContentPages switchToPage)
        {
            if (null != PageSwitchEvent)
            {
                PageSwitchEvent(switchToPage);
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            PageSwitchEvent += MainWindow_PageSwitchEvent;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            achievement = new Achievement();
            mainContent.Content = achievement;
        }

        /// <summary>
        /// 窗体切换内容页
        /// </summary>
        /// <param name="switchToPage"></param>
        private void MainWindow_PageSwitchEvent(ContentPages switchToPage)
        {
            switch (switchToPage)
            {
                case ContentPages.Achievement:
                    if (null!=achievement)
                    {
                        mainContent.Content = achievement;
                    }
                    else
                    {
                        mainContent.Content = new Achievement();
                    }
                    break;
                case ContentPages.Chapters:
                    if (null != chapters)
                    {
                        mainContent.Content = chapters;
                    }
                    else
                    {
                        mainContent.Content = new Chapters();
                    }
                    break;
                case ContentPages.Questions:
                    if (null != questions)
                    {
                        mainContent.Content = questions;
                    }
                    else
                    {
                        mainContent.Content = new Questions();
                    }
                    break;
                case ContentPages.AnswerQuestion:
                    if (null!= questionEidtor)
                    {
                        mainContent.Content = questionEidtor;
                    }
                    else
                    {
                        mainContent.Content = new MathEidtor();
                    }
                    break;
                default:
                    break;
            }
        }

       
    }
}
