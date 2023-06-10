using FeiHub.Models;
using FeiHub.Services;
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

        UsersAPIServices usersAPIServices = new UsersAPIServices(); 
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        List<User> followingUsers = new List<User>();
        public MainPage()
        {
            InitializeComponent();
            AddFollowing();
            AddPost();
        }

        public async void AddFollowing()
        {
            followingUsers = await usersAPIServices.GetListUsersFollowing(SingletonUser.Instance.Username);
            if (followingUsers.Count > 0)
            {
                if (followingUsers[0].StatusCode == System.Net.HttpStatusCode.OK)
                {

                    foreach (User user in followingUsers)
                    {
                        UserControls.PreviewUser following = new UserControls.PreviewUser();
                        following.previewUser.Username = user.username;
                        if(user.profilePhoto == null)
                        {
                            ImageSourceConverter converter = new ImageSourceConverter();
                            following.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        following.previewUser.TextBlock_Username.MouseDown += GoToProfile;
                        following.previewUser.Button_SendMessage.Click += SendMessage;
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
        public async void AddPost()
        {
            List<Posts> postsObtained = await postsAPIServices.GetPostsWithoutFollowings(SingletonUser.Instance.Rol);
            if(postsObtained.Count > 0  )
            {
                if(postsObtained[0].StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach(Posts post in postsObtained)
                    {
                        UserControls.PostPreview posts  = new UserControls.PostPreview();
                        posts.postPreview.Username = post.author;
                        User userData = await usersAPIServices.GetUser(post.author);
                        if(userData.profilePhoto == null)
                        {
                            ImageSourceConverter converter = new ImageSourceConverter();
                            posts.postPreview.ProfilePhoto= (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        posts.postPreview.PostDate = post.dateOfPublish.Date;
                        posts.postPreview.Title = post.title;
                        posts.postPreview.Body = post.body;
                        posts.postPreview.Likes = post.likes;
                        if(post.photos != null)
                        {
                            posts.Label_Photos.Visibility = Visibility.Visible;
                        }
                        posts.postPreview.Likes = post.likes;
                        posts.postPreview.Dislikes = post.dislikes;
                        posts.postPreview.Target = post.target;
                        StackPanel_Posts.Children.Add(posts);
                    }
                }
                if (postsObtained[0].StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (postsObtained[0].StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("Tuvimos un error al obtener las publicaciones, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Label labelWithoutFollowings = new Label();
                labelWithoutFollowings.Content = "No existen publicaciones recientes";
                StackPanel_Posts.Children.Add(labelWithoutFollowings);
            }
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
