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
    /// Lógica de interacción para CompletePost.xaml
    /// </summary>
    public partial class CompletePost : Page
    {
        private PostPreview PostPreview;
        public CompletePost()
        {
            InitializeComponent();
            AddComments();
        }
        public CompletePost(PostPreview post)
        {
            InitializeComponent();
            if (post.postPreview.Username == SingletonUser.Instance.Username)
            {
                IsOwner();
            }
            AddComments();
        }
        private void EditPost(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NewPost());
        }

        private void DeletePost(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro que deseas eliminar esta publicación?", 
                "Eliminar publicación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //Request to API to Delete this post 
            }
        }
        public void IsOwner()
        {
            PostPreview_Post.MenuPost.Visibility = Visibility.Visible;
            PostPreview_Post.MenuItem_EditPost.Click += EditPost;
            PostPreview_Post.MenuItem_DeletePost.Click += DeletePost;
        }

        public void AddComments()
        {
            UserControls.Comment commentUserControl = new UserControls.Comment();
            commentUserControl.comment.Username = "Hola";
            ImageSourceConverter converter = new ImageSourceConverter();
            commentUserControl.comment.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            commentUserControl.comment.Body = "Este es el comentario";
            commentUserControl.comment.DateOfComment = DateTime.Now;
            //StackPanel_Comments.Children.Add(commentUserControl);
            StackPanel_Comments.Children.Insert(0, commentUserControl);
        }
    }
}
