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
    /// Lógica de interacción para Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public User User { get; set; }
        UsersAPIServices usersAPIServices = new UsersAPIServices();
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        public Profile()
        {
            InitializeComponent();
            AddImagesToPost();
        }
        public Profile(User user)
        {
            InitializeComponent();
            this.User = user;
            if (String.IsNullOrEmpty(user.educationalProgram))
            {
                Label_EducationalProgram.Visibility = Visibility.Collapsed;
                Label_Mail.Visibility = Visibility.Collapsed;
                Label_UserType.Content = "Académico";
            }
            else
            {
                Label_EducationalProgram.Visibility = Visibility.Visible;
                Label_EducationalProgram.Content = User.educationalProgram;
                Label_Mail.Content = User.schoolId;
                Label_UserType.Content = "Estudiante";
            }
            if (user.profilePhoto == null)
            {
                ImageSourceConverter converter = new ImageSourceConverter();
                ProfilePhoto.ImageSource = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            }
            FillHeader();
            AddPosts();
        }

        /// <summary>
        /// Write the data from the user in the header
        /// </summary>
        public void FillHeader()
        {
            Label_Name.Content = User.name + " " + User.paternalSurname + " " + User.maternalSurname;
            Label_Username.Content = User.username;
        }
        public void AddImagesToPost()
        {
            UserControls.PostPreview postPreview = new UserControls.PostPreview();
            ImageSourceConverter converter = new ImageSourceConverter();
            Image image = new Image();
            image.Source = (ImageSource)converter.ConvertFromString("../../Resources/uv.png");
            postPreview.WrapPanel_Images.Children.Add(image);
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
            List<User> followingUsers = await usersAPIServices.GetListUsersFollowing(User.username);
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
                        following.Margin = new Thickness(10);
                        following.previewUser.TextBlock_Username.Tag = user;
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
            List<User> followersUsers = await usersAPIServices.GetListUsersFollowers(User.username);
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
                        follower.Margin = new Thickness(10);
                        follower.previewUser.TextBlock_Username.Tag = user;
                        follower.previewUser.TextBlock_Username.MouseDown += GoToProfile;
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
            List<Posts> postsObtained = await postsAPIServices.GetPostsByUsername(User.username);
            if (postsObtained.Count > 0)
            {
                if (postsObtained[0].StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (Posts post in postsObtained)
                    {
                        UserControls.PostPreview posts = new UserControls.PostPreview();
                        posts.postPreview.Username = post.author;
                        User userData = await usersAPIServices.GetUser(post.author);
                        if (userData.profilePhoto == null)
                        {

                            ImageSourceConverter converter = new ImageSourceConverter();
                            posts.postPreview.ProfilePhoto = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
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
            username = TextBox_Finder.Text;
            if (username != "")
            {
                this.NavigationService.Navigate(new SearchResults(username));
            }
        }
    }
}
