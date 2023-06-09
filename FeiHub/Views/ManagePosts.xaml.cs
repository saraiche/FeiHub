using System;
using System.Collections.Generic;
using System.Linq;
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
        public ManagePosts()
        {
            InitializeComponent();
            AddPosts();
        }

        public void AddPosts()
        {
            DataGrid_Posts.Items.Clear();
            DataGrid_Posts.Items.Add(new Post(){
                Number = "3", Title = "Hola" }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
            DataGrid_Posts.Items.Add(new Post()
            {
                Number = "3",
                Title = "Hola"
            }
            );
        }
    }

    internal class Post
    {
        public string Title { get; set; }
        public string Number { get; set; }
    }
}
