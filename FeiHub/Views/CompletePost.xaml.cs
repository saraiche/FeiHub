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
    /// Lógica de interacción para CompletePost.xaml
    /// </summary>
    public partial class CompletePost : Page
    {
        private PostPreview PostPreview;
        private Posts postConsulted = new Posts();
        UsersAPIServices usersAPIServices = new UsersAPIServices();
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        public CompletePost()
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += Search;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            AddComments();
        }

        public CompletePost(Posts post)
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += Search;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            postConsulted = post;
            PostPreview = new PostPreview();
            PostPreview.Id = post.id;
            PostPreview.Title = post.title;
            PostPreview.Body = post.body;
            PostPreview.Username = post.author;
            PostPreview.PostDate = post.dateOfPublish.Date;
            PostPreview.ThisVisibility = Visibility.Hidden;
            if (post.target == "EVERYBODY")
            {
                PostPreview.Target = "Todos";
            }
            if (post.target == "ACADEMIC")
            {
                PostPreview.Target = "Académicos";
            }
            if (post.target == "STUDENT")
            {
                PostPreview.Target = "Estudiantes";
            }
            PostPreview.Likes = post.likes;
            PostPreview.Dislikes = post.dislikes;
            StackPanel_Post.Children.Add(PostPreview);
            if (post.author == SingletonUser.Instance.Username)
            {
                IsOwner();
            }
            if(SingletonUser.Instance.Rol == "ADMIN")
            {
                IsAdmin();
            }
            AddUserDataToPost();
            AddComments();
            AddImages();
        }
        public void AddImages()
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
            foreach(Photo photo in postConsulted.photos)
            {
                Image image = new Image();
                image.Source =  new BitmapImage(new Uri(photo.url));
                image.Margin = new Thickness(0, 10, 0, 10);
                wrapPanel.Children.Add(image);
            }

            StackPanel_Post.Children.Add(wrapPanel);
        }
        public async void AddUserDataToPost()
        {
            User userPost = await usersAPIServices.GetUser(postConsulted.author);
            if (userPost.profilePhoto == null)
            {
                ImageSourceConverter converter = new ImageSourceConverter();
                PostPreview.ProfilePhoto = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            }
            else
            {
                PostPreview.ProfilePhoto = new BitmapImage(new Uri(userPost.profilePhoto));
            }
        }


        private void EditPost(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NewPost(postConsulted));
        }

        private async void DeletePost(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro que deseas eliminar esta publicación?", 
                "Eliminar publicación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                HttpResponseMessage response = await postsAPIServices.DeletePost(postConsulted);
                if (response.IsSuccessStatusCode) 
                {
                    MessageBoxResult resultDelete = MessageBox.Show("Publicación eliminada, serás redirigido a la página principal", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    if(resultDelete == MessageBoxResult.OK) 
                    {
                        if(SingletonUser.Instance.Rol == "ADMIN")
                        {
                            this.NavigationService.Navigate(new ManagePosts());
                        }
                        else
                        {
                            this.NavigationService.Navigate(new MainPage());
                        }
                    }
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("No se pudo eliminar tu publicación inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void IsOwner()
        {
            PostPreview.MenuPost.Visibility = Visibility.Visible;
            PostPreview.MenuItem_EditPost.Click += EditPost;
            PostPreview.MenuItem_DeletePost.Click += DeletePost;
        }
        public void IsAdmin()
        {
            PostPreview.MenuPost.Visibility = Visibility.Visible;
            PostPreview.MenuItem_EditPost.IsEnabled = false;
            PostPreview.MenuItem_DeletePost.Click += DeletePost;
            Button_Comment.IsEnabled = false;
            PostPreview.Button_Comment.IsEnabled = false;
            PostPreview.Button_Dislike.IsEnabled = false;
            PostPreview.Button_Like.IsEnabled = false;
            PostPreview.Button_Report.IsEnabled = false;
        }

        public async void AddComments()
        {
            if (postConsulted.comments.Count() > 0)
            {
                foreach (Models.Comment commentObtained in postConsulted.comments)
                {
                    UserControls.Comment commentUserControl = new UserControls.Comment();
                    commentUserControl.comment.Username = commentObtained.author;
                    User userComment = await usersAPIServices.GetUser(commentObtained.author);
                    if (userComment.profilePhoto == null)
                    {
                        ImageSourceConverter converter = new ImageSourceConverter();
                        commentUserControl.comment.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                    }
                    else
                    {
                         commentUserControl.comment.Source = new BitmapImage(new Uri(userComment.profilePhoto));
                    }
                    commentUserControl.comment.Body = commentObtained.body;
                    if(commentObtained.author == SingletonUser.Instance.Username)
                    {
                        commentUserControl.comment.MenuComment.Visibility = Visibility.Visible;
                        commentUserControl.comment.MenuComment.Tag = commentObtained;
                        commentUserControl.comment.Button_SaveChages.Tag = commentObtained;
                        commentUserControl.comment.MenuItem_EditComment.Click += EditComment;
                        commentUserControl.comment.MenuItem_DeleteComment.Click += DeleteComment;
                    }
                    if (SingletonUser.Instance.Rol == "ADMIN")
                    {
                        commentUserControl.comment.MenuComment.Visibility = Visibility.Collapsed;
                    }
                    StackPanel_Comments.Children.Add(commentUserControl);
                }
            }
            else
            {
                Label labelWithoutComments = new Label();
                labelWithoutComments.Content = "Esta publicación aún no tiene comentarios, ¡sé el primero en comentarla!";
                StackPanel_Comments.Children.Add(labelWithoutComments);
            }
        }

        private async void DeleteComment(object sender, RoutedEventArgs e)
        {
            var menu = (sender as MenuItem).Parent as MenuItem;
            if (menu != null)
            {
                var comment = menu.Tag as Models.Comment;
                if (comment != null)
                {
                    var idPost = postConsulted.id;
                    var idComment = comment.commentId;
                    Posts postCommented = await postsAPIServices.DeleteComment(idComment, idPost);
                    if (postCommented.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        this.NavigationService.Navigate(new CompletePost(postCommented));
                    }
                    if (postCommented.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                        SingletonUser.Instance.BorrarSinglenton();
                        this.NavigationService.Navigate(new LogIn());
                    }
                    if (postCommented.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        MessageBox.Show("Tuvimos un error al crear el comentario, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private async void EditComment(object sender, RoutedEventArgs e)
        {
            var menu = (sender as MenuItem).Parent as MenuItem;
            if (menu != null)
            {
                var comment = menu.Tag as Models.Comment;
                if (comment != null)
                {
                    if ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment != null)
                        ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment).ThisVisibility = Visibility.Visible;
                    ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment).TextBox_Comment.IsEnabled = true;
                    ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment).IdPost = postConsulted.id;
                }
            }

        }

        private async void CommentPost(object sender, RoutedEventArgs args)
        {
            var idPost = postConsulted.id;
            var body = TextBlox_Comment.Text;
            var date = DateTime.Now.Date;
            var author = SingletonUser.Instance.Username;
            if (!String.IsNullOrEmpty(body))
            {
                Models.Comment comment = new Models.Comment();
                comment.author = author;
                comment.dateOfComment = date;
                comment.body = body;
                Posts postCommented = await postsAPIServices.AddComment(comment, idPost);
                if (postCommented.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    this.NavigationService.Navigate(new CompletePost(postCommented));
                }
                if (postCommented.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (postCommented.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("Tuvimos un error al crear el comentario, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No puedes dejar el comentario vacío", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            if (stringToSearch != "")
            {
                this.NavigationService.Navigate(new SearchResults(stringToSearch));
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if(SingletonUser.Instance.Rol == "ADMIN")
            {
                this.NavigationService.Navigate(new ManagePosts());
            }
            else
            {
                this.NavigationService.Navigate(new MainPage());
            }
        }
    }
}
