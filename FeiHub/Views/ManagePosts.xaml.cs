using FeiHub.Models;
using FeiHub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
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
    /// Lógica de interacción para ManagePosts.xaml
    /// </summary>
    public partial class ManagePosts : Page
    {
        List<Posts> posts = new List<Posts>();
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        public ManagePosts()
        {
            InitializeComponent();
            AddPosts();
        }

        public async void AddPosts()
        {
            DataGrid_Posts.Items.Clear();
            posts = await postsAPIServices.GetReporteredPosts();
            DataGrid_Posts.ItemsSource = posts;
            
            
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

        private void GoToPost(object sender, RoutedEventArgs e)
        {
            Posts selectedPost = DataGrid_Posts.SelectedItem as Posts;
            this.NavigationService.Navigate(new CompletePost(selectedPost));
        }

        private async void DeletePost(object sender, RoutedEventArgs e)
        {
            Posts selectedPost = DataGrid_Posts.SelectedItem as Posts;
            MessageBoxResult result = MessageBox.Show("¿Estás seguro que deseas eliminar esta publicación?",
               "Eliminar publicación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                HttpResponseMessage response = await postsAPIServices.DeletePost(selectedPost);
                if (response.IsSuccessStatusCode)
                {
                    MessageBoxResult resultDelete = MessageBox.Show("Publicación eliminada", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (resultDelete == MessageBoxResult.OK)
                    {
                        this.NavigationService.Navigate(new ManagePosts());
                    }
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("No se pudo eliminar tu publicación inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }


    internal class Post
    {
        public string Title { get; set; }
        public string Number { get; set; }
    }
}
