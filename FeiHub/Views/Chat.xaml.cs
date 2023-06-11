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
using System.Windows.Interop;
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
        private PostsAPIServices postsAPIServices = new PostsAPIServices();
        private Chats UserChat = new Chats();
        private User User = new User();
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
            User = user;
            AddFollowing();
            AddMessages(user.username);
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
                        else
                        {
                            following.previewUser.Source = new BitmapImage(new Uri(user.profilePhoto));
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
            User.username = (((sender as Border).Parent as UserControl) as PreviewUser).Username;
            AddMessages(User.username);
        }

        private async void AddMessages(string username)
        {
            Label_NoChatSelected.Visibility = Visibility.Collapsed;
            Label_Username.Content = username;
            Label_Username.Visibility = Visibility.Visible;
            ScrollViewer_ListMessages.Visibility = Visibility.Visible;
            ListView_Chat.Items.Clear();
            StackPanel_MessageToSend.Visibility = Visibility.Visible;
            UserChat = await postsAPIServices.GetChatByUsername(username);
            if (UserChat.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var messages = SortMessages(UserChat.chats);
                foreach(Chats.Chat msg in messages)
                {
                    ListView_Chat.Items.Add(msg.ToString());
                }
                Label_NoMessages.Visibility = Visibility.Collapsed;
            }
            if(UserChat.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Label_NoMessages.Visibility = Visibility.Visible;
            }
            if(UserChat.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                SingletonUser.Instance.BorrarSinglenton();
                this.NavigationService.Navigate(new LogIn());
            }
            if(UserChat.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                MessageBox.Show("Tuvimos un error al obtener los mensajes, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private Chats.Chat[] SortMessages(Chats.Chat[] messages)
        {
            return messages.OrderBy(message => message.DateOfMessageString).ToArray();
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

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            Chats.Chat newMessage = new Chats.Chat();
            newMessage.Message = TextBox_Message.Text;
            newMessage.DateOfMessageString = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            newMessage.DateAPI = DateTime.Now.AddHours(6).ToString("MM/dd/yyyy hh:mm:ss tt");
            if (UserChat.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Label_NoMessages.Visibility = Visibility.Collapsed;
                UserChat = await postsAPIServices.CreateChat(newMessage, User.username);
            }
            else
            {
                UserChat = await postsAPIServices.SendMessage(newMessage, User.username);
            }
            if (UserChat.StatusCode == System.Net.HttpStatusCode.OK || UserChat.StatusCode == System.Net.HttpStatusCode.Created)
            {
                if (UserChat.messages.Length > 0)
                {
                    var msg = SearchMessage(newMessage, UserChat.messages);
                    if (msg.username != null)
                    {
                        ListView_Chat.Items.Add(msg.ToString());
                        TextBox_Message.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Tuvimos un error al enviar el mensajes, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            if (UserChat.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                SingletonUser.Instance.BorrarSinglenton();
                this.NavigationService.Navigate(new LogIn());
            }
            if (UserChat.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                MessageBox.Show("Tuvimos un error al enviar el mensajes, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private Chats.Message SearchMessage(Chats.Chat chat, Chats.Message[] messages)
        {
            Chats.Message message = new Chats.Message();
            foreach(Chats.Message msg in messages)
            {
                if (msg.message == chat.Message &&
                        msg.dateOfMessage.ToString().Contains(chat.DateAPI)   &&
                        msg.username == SingletonUser.Instance.Username)
                {
                    message.dateOfMessage = msg.dateOfMessage;
                    message.message = msg.message;
                    message.username = msg.username;
                }
            }
            return message;
        }
    }
}
