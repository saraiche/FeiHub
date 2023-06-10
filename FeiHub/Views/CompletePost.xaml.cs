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
        public CompletePost()
        {
            InitializeComponent();
            IsOwner();
        }
        private void EditPost(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new NewPost(1));
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
    }
}
