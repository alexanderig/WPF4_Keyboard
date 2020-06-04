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
using System.Windows.Automation.Peers;
using System.Reflection;
//using System.Windows.Automation.Provider;

namespace Keyboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VMclass mclass;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetKeyDown(object sender, KeyEventArgs e)
        {
            if (mclass.isEnabledAllKeys)
            {
                if((e.Key >= Key.D0 && e.Key <= Key.Z) || (e.Key >= Key.Oem1 && e.Key <= Key.OemBackslash) || e.Key == Key.Space)
                     mclass.CurrentKeyArg = e;
                var mybutton = (Button)this.FindName(e.Key.ToString());
                if (mybutton != null && e.Key != Key.Capital)
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(mybutton, new object[] { true });
                else if (e.Key == Key.Capital)
                    Capital.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));//CapsLock Push Pull event
                else if (e.Key == Key.System)//this is for Alt buttons because they are same code
                {
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(SystemL, new object[] { true });
                    typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(SystemR, new object[] { true });
                }
            }
            e.Handled = true; 
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            var mybutton = (Button)this.FindName(e.Key.ToString());
            if (mybutton != null)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(mybutton, new object[] { false });
            else if (e.Key == Key.System)
            {
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(SystemL, new object[] { false });
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(SystemR, new object[] { false });
            }
        }

        private void CapsL_Click(object sender, RoutedEventArgs e)
        {
            if (mclass.CapsLocked)
            {
                Capital.Background = Brushes.LightGray;
                mclass.CapsLocked = false;
            }
            else
            {
                Capital.Background = Brushes.DarkGray;
                mclass.CapsLocked = true;
            }
        }

       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mclass = this.DataContext as VMclass;
        }

      

    }
   
}