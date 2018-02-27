using MathSpace.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MathSpace
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        
        private void App_OnStartUp(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MathSpace.MainWindow();
            Login login = new Login();
            if (login.ShowDialog()==true)
            {
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}
