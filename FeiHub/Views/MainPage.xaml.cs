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
    /// Lógica de interacción para MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            AddFollowing();
        }

        public void AddFollowing()
        {
            UserControls.PreviewUser following = new UserControls.PreviewUser();
            following.previewUser.Username = "Carsiano";
            ImageSourceConverter converter = new ImageSourceConverter();
            following.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/uv.png");

            following.previewUser.TextBlock_Username.MouseDown += GoToProfile;
            following.previewUser.Button_SendMessage.Click += SendMessage;

            StackPanel_Following.Children.Add(following);

            UserControls.PreviewUser anotherFollowing = new UserControls.PreviewUser();
            anotherFollowing.previewUser.Username = "Saraiche";
            anotherFollowing.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");

            anotherFollowing.previewUser.TextBlock_Username.MouseDown += GoToProfile;
            anotherFollowing.previewUser.Button_SendMessage.Click += SendMessage;
            
            StackPanel_Following.Children.Add(anotherFollowing);

        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mando mensaje a " + (((((e.Source as Button).Parent as Grid).Parent as Border).Parent as UserControl) as UserControls.PreviewUser).Username);
        }

        private void GoToProfile(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hola, me voy al perfil de " + (((((e.Source as TextBlock).Parent as Grid).Parent as Border).Parent as UserControl) as UserControls.PreviewUser).Username);
        }

        private void GoToNewPost(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NewPost());
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            SingletonUser.Instance.BorrarSinglenton();
            this.NavigationService.Navigate(new LogIn());
        }
    }
}
