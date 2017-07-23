
using MathSpace.ViewModel;
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
using System.Xml;

namespace MathSpace
{
    /// <summary>
    /// MathKeyBoard.xaml 的交互逻辑
    /// </summary>
    public partial class MathKeyBoard : UserControl
    {
        public delegate void  InputTypeChangedEventHandler(InputTypes inputType);
        public event InputTypeChangedEventHandler InputTypeChangedEvent;

        public MathKeyBoard()
        {
            InitializeComponent();
        }

        private void InputCharButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void InputCommandButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void MathKeyBoard_Loaded(object sender, RoutedEventArgs e)
        {
            var vm=this.DataContext as MathKeyBoardViewModel;
            vm.LoadKey();
        }

        private void InputType_Changed(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Tag.ToString() == "Fraction")
            {
                if (InputTypeChangedEvent != null)
                {
                    InputTypeChangedEvent(InputTypes.Fraction);
                }
            }
            else if (btn.Tag.ToString()=="Radical") 
            {
                if (InputTypeChangedEvent != null)
                {
                    InputTypeChangedEvent(InputTypes.Radical);
                }
            }
            else if (btn.Tag.ToString()== "Exponenal")
            {
                if (InputTypeChangedEvent!=null)
                {
                    InputTypeChangedEvent(InputTypes.Exponential);                
                }
            }
        }
    }
}
