using FeiHub.Models;
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
    /// Lógica de interacción para EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Page
    {
        public EditProfile()
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += Search;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            SingletonUser.Instance.BorrarSinglenton();
            this.NavigationService.Navigate(new LogIn());
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Profile(SingletonUser.Instance.Username));
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string stringToSearch = this.MainBar.TextBox_Search.Text;
            MessageBox.Show(stringToSearch);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
