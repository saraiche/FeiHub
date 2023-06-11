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
                        following.previewUser.TextBlock_Username.Tag = user; 
                        following.previewUser.TextBlock_Username.MouseDown += GoToProfile;
                        following.previewUser.Button_SendMessage.Tag = user;
                        following.previewUser.Button_SendMessage.Click += GoToChat;
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
                ComboBox_Target.Visibility = Visibility.Visible;
                ComboBox_Target.Items.Add("Todos");
                if (SingletonUser.Instance.Rol == "STUDENT")
                {
                    ComboBox_Target.Items.Add("Estudiantes");
                }
                else
                {
                    ComboBox_Target.Items.Add("Académicos");
                }
                ComboBox_Target.SelectedIndex = 0;
            }
            else
            {
                Label labelWithoutFollowings = new Label();
                labelWithoutFollowings.Content = "Sigue a tus amigos para verlos aquí";
                StackPanel_Following.Children.Add(labelWithoutFollowings);
                AddPostWithoutFollowings();
            }
            

        }
        public async void AddPostWithoutFollowings()
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
                        posts.postPreview.Id = post.id;
                        User userData = await usersAPIServices.GetUser(post.author);
                        if (userData.profilePhoto == null)
                        {
                            
                            ImageSourceConverter converter = new ImageSourceConverter();
                            posts.postPreview.ProfilePhoto= (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        else
                        {
                            posts.postPreview.ProfilePhoto = new BitmapImage(new Uri(userData.profilePhoto));
                        }
                        posts.postPreview.PostDate = post.dateOfPublish.Date;
                        posts.postPreview.Title = post.title;
                        posts.postPreview.Body = post.body;
                        if (post.photos.Count() > 0)
                        {
                            posts.Label_Photos.Visibility = Visibility.Visible;
                        }
                        posts.postPreview.Likes = post.likes;
                        posts.postPreview.Dislikes = post.dislikes;
                        if(post.target == "EVERYBODY")
                        {
                            posts.postPreview.Target = "Todos";
                        }
                        if (post.target == "ACADEMIC")
                        {
                            posts.postPreview.Target = "Académicos";
                        }
                        if (post.target == "STUDENT")
                        {
                            posts.postPreview.Target = "Estudiantes";
                        }
                        posts.postPreview.Border_Post.Tag = post;
                        posts.postPreview.Border_Post.MouseDown += GoToCompletePost;
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



        private void GoToProfile(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                var user = textBlock.Tag as User;
                if (user != null)
                {
                    this.NavigationService.Navigate(new Profile(user));
                }
            }
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
        public async void AddPostWithTarget()
        {
            List<Posts> postsObtained = await postsAPIServices.GetPostsByTarget(followingUsers,SingletonUser.Instance.Rol);
            if (postsObtained.Count > 0)
            {
                if (postsObtained[0].StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (Posts post in postsObtained)
                    {
                        UserControls.PostPreview posts = new UserControls.PostPreview();
                        posts.postPreview.Username = post.author;
                        posts.postPreview.Id = post.id;
                        User userData = await usersAPIServices.GetUser(post.author);
                        if (userData.profilePhoto == null)
                        {

                            ImageSourceConverter converter = new ImageSourceConverter();
                            posts.postPreview.ProfilePhoto = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        else
                        {
                            posts.postPreview.ProfilePhoto = new BitmapImage(new Uri(userData.profilePhoto));
                        }
                        posts.postPreview.PostDate = post.dateOfPublish.Date;
                        posts.postPreview.Title = post.title;
                        posts.postPreview.Body = post.body;
                        if (post.photos.Count() > 0)
                        {
                            posts.Label_Photos.Visibility = Visibility.Visible;
                        }
                        posts.postPreview.Likes = post.likes;
                        posts.postPreview.Dislikes = post.dislikes;
                        if (post.target == "EVERYBODY")
                        {
                            posts.postPreview.Target = "Todos";
                        }
                        if (post.target == "ACADEMIC")
                        {
                            posts.postPreview.Target = "Académicos";
                        }
                        if (post.target == "STUDENT")
                        {
                            posts.postPreview.Target = "Estudiantes";
                        }
                        posts.postPreview.Border_Post.Tag = post;
                        posts.postPreview.Border_Post.MouseDown += GoToCompletePost;
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
        public async void AddPrincipalPosts()
        {
            List<Posts> postsObtained = await postsAPIServices.GetPrincipalPosts(followingUsers, SingletonUser.Instance.Rol);
            if (postsObtained.Count > 0)
            {
                if (postsObtained[0].StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (Posts post in postsObtained)
                    {
                        UserControls.PostPreview posts = new UserControls.PostPreview();
                        posts.postPreview.Username = post.author;
                        posts.postPreview.Id = post.id;
                        User userData = await usersAPIServices.GetUser(post.author);
                        if (userData.profilePhoto == null)
                        {

                            ImageSourceConverter converter = new ImageSourceConverter();
                            posts.postPreview.ProfilePhoto = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        else
                        {
                            posts.postPreview.ProfilePhoto = new BitmapImage(new Uri(userData.profilePhoto));
                        }
                        posts.postPreview.PostDate = post.dateOfPublish.Date;
                        posts.postPreview.Title = post.title;
                        posts.postPreview.Body = post.body;
                        if (post.photos.Count() > 0)
                        {
                            posts.Label_Photos.Visibility = Visibility.Visible;
                        }
                        posts.postPreview.Likes = post.likes;
                        posts.postPreview.Dislikes = post.dislikes;
                        if (post.target == "EVERYBODY")
                        {
                            posts.postPreview.Target = "Todos";
                        }
                        if (post.target == "ACADEMIC")
                        {
                            posts.postPreview.Target = "Académicos";
                        }
                        if (post.target == "STUDENT")
                        {
                            posts.postPreview.Target = "Estudiantes";
                        }
                        posts.postPreview.Border_Post.Tag = post;
                        posts.postPreview.Border_Post.MouseDown += GoToCompletePost;
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
        private void FilterPost(object sender, SelectionChangedEventArgs e)
        {
            string targetSelected = ComboBox_Target.SelectedItem.ToString();
            if (targetSelected == "Todos")
            {
                StackPanel_Posts.Children.Clear();
                AddPrincipalPosts();
            }
            else
            {
                StackPanel_Posts.Children.Clear();
                AddPostWithTarget();
            }
        }

        private void FindUser(object sender, RoutedEventArgs e)
        {
            string username = "";
            username = TextBox_Finder.Text;
            if (username != "")
            {
                this.NavigationService.Navigate(new SearchResults(username));
            }
        }
        
        private void GoToCompletePost(object sender, RoutedEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                var post = border.Tag as Posts;
                if (post != null)
                {
                    this.NavigationService.Navigate(new CompletePost(post));
                }
            }
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

        private void GoToMyProfile(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Profile(SingletonUser.Instance.Username));
        }
    }
}
