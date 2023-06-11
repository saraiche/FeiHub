using FeiHub.Models;
using FeiHub.Services;
using FeiHub.UserControls;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Lógica de interacción para Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        User userConsulted = new User();
        UsersAPIServices usersAPIServices = new UsersAPIServices();
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        public Profile()
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            AddImagesToPost();
        }

        public Profile(string username)
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            AddDataUserLogged();
            
        }
        public Profile(User user)
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            this.userConsulted = user;
            if (String.IsNullOrEmpty(user.educationalProgram))
            {
                Label_EducationalProgram.Visibility = Visibility.Collapsed;
                Label_Mail.Visibility = Visibility.Collapsed;
                Label_UserType.Content = "Académico";
            }
            else
            {
                Label_EducationalProgram.Visibility = Visibility.Visible;
                Label_EducationalProgram.Content = userConsulted.educationalProgram;
                Label_Mail.Content = userConsulted.schoolId;
                Label_UserType.Content = "Estudiante";
            }
            if (user.profilePhoto == null)
            {
                ImageSourceConverter converter = new ImageSourceConverter();
                ProfilePhoto.ImageSource = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            }
            else
            {
                ProfilePhoto.ImageSource = new BitmapImage(new Uri(user.profilePhoto));
            }
            if (user.username == SingletonUser.Instance.Username)
            {
                IsOwner();
            }
            else
            {
                Button_EditProfile.IsEnabled = false;
            }
            FillHeader();
            AddPosts();
            IsFollowingUser();
        }

        public async void AddDataUserLogged()
        {
            userConsulted = await usersAPIServices.GetUser(SingletonUser.Instance.Username);
            Label_EducationalProgram.Visibility = Visibility.Visible;
            Label_EducationalProgram.Content = userConsulted.educationalProgram;
            Label_Mail.Content = userConsulted.schoolId;
            Label_UserType.Content = "Estudiante";
            if (userConsulted.profilePhoto == null)
            {
                ImageSourceConverter converter = new ImageSourceConverter();
                ProfilePhoto.ImageSource = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            }
            else
            {   
                ProfilePhoto.ImageSource = new BitmapImage(new Uri(userConsulted.profilePhoto));
            }
            FillHeader();
            AddPosts();
            IsOwner();
        }
        public void FillHeader()
        {
            Label_Name.Content = userConsulted.name + " " + userConsulted.paternalSurname + " " + userConsulted.maternalSurname;
            Label_Username.Content = userConsulted.username;
        }
        public void AddImagesToPost()
        {
            UserControls.PostPreview postPreview = new UserControls.PostPreview();
            ImageSourceConverter converter = new ImageSourceConverter();
            Image image = new Image();
            image.Source = (ImageSource)converter.ConvertFromString("../../Resources/uv.png");
            postPreview.WrapPanel_Images.Children.Add(image);
            //  NO TIENE OBJETO POST
            //postPreview.postPreview.Tag = post.id;
            //posts.postPreview.Id = post.id;
            postPreview.postPreview.Border_Post.MouseDown += GoToCompletePost;
            StackPanel_Posts.Children.Add(postPreview);

        }

        private void ShowFollowing(object sender, RoutedEventArgs e)
        {
            WrapPanel_Following.Children.Clear();
            Label_Content.Content = "Siguiendo";
            WrapPanel_Following.Visibility = Visibility.Visible;
            WrapPanel_Followers.Visibility = Visibility.Collapsed;
            StackPanel_Posts.Visibility = Visibility.Collapsed;
            AddFollowing();
        }

        private void ShowFollowers(object sender, RoutedEventArgs e)
        {
            WrapPanel_Followers.Children.Clear();
            Label_Content.Content = "Seguidores";
            WrapPanel_Followers.Visibility = Visibility.Visible;
            WrapPanel_Following.Visibility = Visibility.Collapsed;
            StackPanel_Posts.Visibility = Visibility.Collapsed;
            AddFollowers();
        }

        private void ShowPosts(object sender, RoutedEventArgs e)
        {
            StackPanel_Posts.Children.Clear();
            Label_Content.Content = "Publicaciones";
            StackPanel_Posts.Visibility = Visibility.Visible;
            WrapPanel_Followers.Visibility = Visibility.Collapsed;
            WrapPanel_Following.Visibility = Visibility.Collapsed;
            AddPosts();
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
        public async void AddFollowing()
        {
            List<User> followingUsers = await usersAPIServices.GetListUsersFollowing(userConsulted.username);
            if (followingUsers.Count > 0)
            {
                if (followingUsers[0].StatusCode == System.Net.HttpStatusCode.OK)
                {

                    foreach (User user in followingUsers)
                    {
                        UserControls.PreviewUser following = new UserControls.PreviewUser();
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
                        following.Margin = new Thickness(10);
                        following.previewUser.TextBlock_Username.Tag = user;
                        following.previewUser.Button_SendMessage.Tag = user;
                        following.previewUser.Button_SendMessage.Click += GoToChat;
                        following.previewUser.TextBlock_Username.MouseDown += GoToProfile;
                        WrapPanel_Following.Children.Add(following);
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

                    MessageBox.Show("Tuvimos un error al obtener a quiénes sigue esta persona, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Label labelWithoutFollowings = new Label();
                labelWithoutFollowings.Content = "Esta persona no sigue a nadie";
                WrapPanel_Following.Children.Add(labelWithoutFollowings);
            }

        }
        public async void AddFollowers()
        {
            List<User> followersUsers = await usersAPIServices.GetListUsersFollowers(userConsulted.username);
            if (followersUsers.Count > 0)
            {
                if (followersUsers[0].StatusCode == System.Net.HttpStatusCode.OK)
                {

                    foreach (User user in followersUsers)
                    {
                        UserControls.PreviewUser follower = new UserControls.PreviewUser();
                        follower.previewUser.Username = user.username;
                        if (user.profilePhoto == null)
                        {
                            ImageSourceConverter converter = new ImageSourceConverter();
                            follower.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        else
                        {
                            follower.previewUser.Source = new BitmapImage(new Uri(user.profilePhoto));
                        }
                        follower.Margin = new Thickness(10);
                        follower.previewUser.TextBlock_Username.Tag = user;
                        follower.previewUser.TextBlock_Username.MouseDown += GoToProfile;
                        follower.previewUser.Button_SendMessage.Tag = user;
                        follower.previewUser.Button_SendMessage.Click += GoToChat;
                        WrapPanel_Followers.Children.Add(follower);
                    }
                }
                if (followersUsers[0].StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {

                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (followersUsers[0].StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {

                    MessageBox.Show("Tuvimos un error al obtener los seguidores de esta persona, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Label labelWithoutFollowings = new Label();
                labelWithoutFollowings.Content = "Esta persona no tiene seguidores";
                WrapPanel_Followers.Children.Add(labelWithoutFollowings);
            }
        }
        public async void AddPosts()
        {
            List<Posts> postsObtained = await postsAPIServices.GetPostsByUsername(userConsulted.username);
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
                        posts.postPreview.ThisVisibility = Visibility.Hidden;
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
                        posts.postPreview.Button_Comment.Tag = post;
                        posts.postPreview.Button_Comment.Click += CommentPost;
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
        private void FindUser(object sender, RoutedEventArgs e)
        {
            string username = "";
            username = MainBar.TextBox_Search.Text;
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
        private void CommentPost(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var post = button.Tag as Posts;
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
        private void EditProfile(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditProfile(userConsulted));
        }
        public void IsOwner()
        {
            Button_Follow.IsEnabled = false;
            Button_UnFollow.IsEnabled = false;
            Button_SendMessage.IsEnabled = false;
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

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }
        private void GoToThisUserChat(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Chat(userConsulted));
        }
        private async void Follow(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = await usersAPIServices.Follow(userConsulted.username, SingletonUser.Instance.Username);
            if (response.IsSuccessStatusCode)
            {
                MessageBoxResult result = MessageBox.Show($"Haz empezado a seguir a {userConsulted.username}", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                Button_UnFollow.IsEnabled = true;
                Button_Follow.IsEnabled = false;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                SingletonUser.Instance.BorrarSinglenton();
                this.NavigationService.Navigate(new LogIn());
            }
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                MessageBox.Show($"Tuvimos un error al seguir a {userConsulted.username}, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UnFollow(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = await usersAPIServices.Unfollow(userConsulted.username, SingletonUser.Instance.Username);
            if (response.IsSuccessStatusCode)
            {
                MessageBoxResult result = MessageBox.Show($"Haz dejado de a seguir a {userConsulted.username}", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                Button_UnFollow.IsEnabled = false;
                Button_Follow.IsEnabled = true;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                SingletonUser.Instance.BorrarSinglenton();
                this.NavigationService.Navigate(new LogIn());
            }
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                MessageBox.Show($"Tuvimos un error al dejar de seguir a {userConsulted.username}, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void IsFollowingUser()
        {
            if(userConsulted.username != SingletonUser.Instance.Username)
            {
                List<string> followings = await usersAPIServices.GetListFollowing(SingletonUser.Instance.Username);
                if (followings.Contains(userConsulted.username))
                {
                    Button_UnFollow.IsEnabled = true;
                    Button_Follow.IsEnabled = false;
                }
                else
                {
                    Button_UnFollow.IsEnabled=false; 
                    Button_Follow.IsEnabled = true;
                }
            }
        }
    }
}
