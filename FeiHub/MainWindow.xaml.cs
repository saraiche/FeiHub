using FeiHub.Models;
using FeiHub.Views;
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

namespace FeiHub
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Frame_PagesNavigation.Navigate(new LogIn());
        }

        private void DestroySingleton(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SingletonUser.Instance.BorrarSinglenton();
        }
    }
}
