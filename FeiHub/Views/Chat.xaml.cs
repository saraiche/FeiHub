using FeiHub.Models;
using FeiHub.UserControls;
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
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Page
    {
        public Chat()
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            AddFollowing();
        }

        public Chat(User user)
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            AddFollowing();
            ShowChatWithUser(user);
        }

        public void AddFollowing()
        {
            
        }

        private void Border_Seguidor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Se debe mantener la parte del código, solo cambiar la manera de presentar los mensajes
            string username = (((sender as Border).Parent as UserControl) as PreviewUser).Username;
            Label_NoChatSelected.Visibility = Visibility.Collapsed;
            Label_Username.Content = username;
            Label_Username.Visibility = Visibility.Visible;
            ScrollViewer_ListMessages.Visibility = Visibility.Visible;
            ListView_Chat.Items.Clear();
            StackPanel_MessageToSend.Visibility = Visibility.Visible;

            //Esta es la línea que se debe modificar 
            ListView_Chat.Items.Add(username + " : Hola");
            ListView_Chat.Items.Add(username + " : Hola");
            ListView_Chat.Items.Add(username + " : Desde border");

        }

        public void ShowChatWithUser(User user)
        {
            Label_NoChatSelected.Visibility = Visibility.Collapsed;
            Label_Username.Content = user.username;
            Label_Username.Visibility = Visibility.Visible;
            ScrollViewer_ListMessages.Visibility = Visibility.Visible;
            ListView_Chat.Items.Clear();
            StackPanel_MessageToSend.Visibility = Visibility.Visible;

            //Esta es la línea que se debe modificar 
            ListView_Chat.Items.Add(user.username + " : Desde botón");
            ListView_Chat.Items.Add(user.username + " : Hola");
            ListView_Chat.Items.Add(user.username + " : Hola");
        }

        private void GoToChat(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var user = button.Tag as User;
                if (user != null)
                {
                    this.NavigationService.Navigate(new Chat(user));
                }
            }
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
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
        private void FindUser(object sender, RoutedEventArgs e)
        {
            string username = "";
            username = MainBar.TextBox_Search.Text;
            if (username != "")
            {
                this.NavigationService.Navigate(new SearchResults(username));
            }
        }
    }
}
