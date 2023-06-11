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
            AddUserDataToPost();
            AddComments();
            AddImages();
        }
        public void AddImages()
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;

            /* 
             * ESTO SE SUSTITUYE CON LO DE LAS IMÁGENES QUE TENGAS :)
            ImageSourceConverter converter = new ImageSourceConverter();
            Image image = new Image();
            image.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            image.Margin = new Thickness(0, 10, 0, 10);
            wrapPanel.Children.Add(image);
            Image image2 = new Image();
            image2.Source = (ImageSource)converter.ConvertFromString("../../Resources/uv.png");
            image2.Margin = new Thickness(0, 10, 0, 10);
            wrapPanel.Children.Add(image2);
            Image image3 = new Image();
            image3.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            image3.Margin = new Thickness(0, 10, 0, 10);
            wrapPanel.Children.Add(image3);
            */

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
                        this.NavigationService.Navigate(new MainPage());
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
                        // ADD PHOTO IN AWS
                    }
                    commentUserControl.comment.Body = commentObtained.body;
                    commentUserControl.comment.DateOfComment = commentObtained.dateOfComment.Date;
                    if(commentObtained.author == SingletonUser.Instance.Username)
                    {
                        commentUserControl.comment.MenuComment.Visibility = Visibility.Visible;
                        commentUserControl.comment.MenuComment.Tag = commentObtained;
                        commentUserControl.comment.Button_SaveChages.Tag = commentObtained;
                        commentUserControl.comment.MenuItem_EditComment.Click += EditComment;
                        commentUserControl.comment.MenuItem_DeleteComment.Click += DeleteComment;
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

        private void DeleteComment(object sender, RoutedEventArgs e)
        {
            var menu = (sender as MenuItem).Parent as MenuItem;
            if (menu != null)
            {
                var comment = menu.Tag as Models.Comment;
                if (comment != null)
                {
                    MessageBox.Show("Eliminar");
                }
            }
        }

        private void EditComment(object sender, RoutedEventArgs e)
        {
            var menu = (sender as MenuItem).Parent as MenuItem;
            if (menu != null)
            {
                var comment = menu.Tag as Models.Comment;
                if (comment != null)
                {
                    MessageBox.Show("Editar");
                    if ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment != null)
                        ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment).ThisVisibility = Visibility.Visible;
                    ((((menu.Parent as Menu).Parent as Grid).Parent as Border).Parent as UserControls.Comment).TextBox_Comment.IsEnabled = true;
                }
            }
        }

        private void CommentPost(object sender, RoutedEventArgs args)
        {
            var idPost = postConsulted.id;
            var body = TextBlox_Comment.Text;
            var date = DateTime.Now;
            var author = SingletonUser.Instance.Username;
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
