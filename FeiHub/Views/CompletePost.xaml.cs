using FeiHub.Models;
using FeiHub.Services;
using FeiHub.UserControls;
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
            AddComments();
        }

        public CompletePost(Posts post)
        {
            InitializeComponent();
            postConsulted = post;
            PostPreview = new PostPreview();
            PostPreview.Title = post.title;
            PostPreview.Body = post.body;
            PostPreview.Username = post.author;
            PostPreview.PostDate = post.dateOfPublish.Date;
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
    }
}
