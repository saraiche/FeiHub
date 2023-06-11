using FeiHub.Models;
using FeiHub.Services;
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
        private UsersAPIServices usersAPIServices = new UsersAPIServices();
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
            ShowChatWithUser(user.username);
        }

        public async void AddFollowing()
        {
            List<User> followingUsers = new List<User>();
            followingUsers = await usersAPIServices.GetListUsersFollowing(SingletonUser.Instance.Username);
            if (followingUsers.Count > 0)
            {
                if (followingUsers[0].StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (User user in followingUsers)
                    {
                        PreviewUser following = new PreviewUser();
                        following.previewUser.Username = user.username;
                        if (user.profilePhoto == null)
                        {
                            ImageSourceConverter converter = new ImageSourceConverter();
                            following.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        following.previewUser.TextBlock_Username.Tag = user;
                        following.previewUser.Button_SendMessage.Tag = user;
                        following.previewUser.Button_SendMessage.Click += GoToChat;
                        following.previewUser.Border_Seguidor.MouseDown += Border_Seguidor_MouseDown;
                        following.previewUser.ThisVisibility = Visibility.Collapsed;
                        StackPanel_Following.Children.Add(following);
                    }
                }
                if (followingUsers[0].StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (followingUsers[0].StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("Tuvimos un error al obtener a quiénes sigues, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Label labelWithoutFollowings = new Label();
                labelWithoutFollowings.Content = "Sigue a tus amigos para verlos aquí";
                StackPanel_Following.Children.Add(labelWithoutFollowings);
            }
        }

        private void Border_Seguidor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string username = (((sender as Border).Parent as UserControl) as PreviewUser).Username;
            ShowChatWithUser(username);
        }

        public void ShowChatWithUser(string username)
        {
            Label_NoChatSelected.Visibility = Visibility.Collapsed;
            Label_Username.Content = username;
            Label_Username.Visibility = Visibility.Visible;
            ScrollViewer_ListMessages.Visibility = Visibility.Visible;
            ListView_Chat.Items.Clear();
            StackPanel_MessageToSend.Visibility = Visibility.Visible;

            //Esta es la línea que se debe modificar 
            ListView_Chat.Items.Add(username + " : Hola");
            ListView_Chat.Items.Add(username + " : Hola");
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
