using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Keyboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStart(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            VMclass mclass = new VMclass();
            window.DataContext = mclass;
            window.Show();
        }
    }
}
