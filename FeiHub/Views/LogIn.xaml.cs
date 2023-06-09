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

namespace FeiHub.Views
{
    /// <summary>
    /// Lógica de interacción para LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel_LogIn.Visibility = Visibility.Collapsed;
            StackPanel_SignIn.Visibility = Visibility.Visible;
        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonGoToLogIn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel_LogIn.Visibility = Visibility.Visible;
            StackPanel_SignIn.Visibility = Visibility.Collapsed;
        }
    }
}
