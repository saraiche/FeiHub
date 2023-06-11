using FeiHub.Models;
using FeiHub.Services;
using Microsoft.Win32;
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
    /// Lógica de interacción para NewPost.xaml
    /// </summary>
    public partial class NewPost : Page
    {
        string title = "";
        string body = "";
        string target = "";
        string author = SingletonUser.Instance.Username;
        DateTime dateOfPublish = DateTime.Now;
        Photo[] photos = null;
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        Posts postToEdit = new Posts();
        public NewPost()
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
        }

        public NewPost(Posts post)
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            postToEdit = post;
            Label_PageTitle.Content = "Editar Publicación";
            TextBox_Title.Text = postToEdit.title;
            TextBox_Body.Text = postToEdit.body;
            if(postToEdit.target == "STUDENT")
            {
                ComboBox_Target.SelectedIndex = 0;
            }
            if (postToEdit.target == "ACADEMIC")
            {
                ComboBox_Target.SelectedIndex = 1;
            }
            if (postToEdit.target == "EVERYBODY")
            {
                ComboBox_Target.SelectedIndex = 2;
            }
            Button_Post.Visibility = Visibility.Collapsed;
            Button_Post.Background = null;
            Button_SaveChanges.Visibility = Visibility.Visible;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private async void CreatePost(object sender, RoutedEventArgs e)
        {
            title = TextBox_Title.Text;
            body = TextBox_Body.Text;
            if (ComboBox_Target.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)ComboBox_Target.SelectedItem;
                string selectedValue = selectedItem.Content.ToString();

                if (selectedValue == "Estudiantes")
                {
                    target = "STUDENT";
                }
                else if (selectedValue == "Académicos")
                {
                    target = "ACADEMIC";
                }
                else if (selectedValue == "Todos")
                {
                    target = "EVERYBODY";
                }
            }
            else
            {
                MessageBox.Show("Selecciona una etiqueta de a quienés irá dirigida la publicación", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            bool withoutNullFields = ValidateNullFields();
            if (withoutNullFields)
            {
                Posts postCreated = new Posts();
                postCreated.title = title;
                postCreated.body = body;
                postCreated.author = author;
                postCreated.target = target;
                if(photos != null)
                {
                    postCreated.photos = photos;
                }
                else
                {
                    postCreated.photos = new Photo[0];
                }
                postCreated.dateOfPublish = dateOfPublish;
                HttpResponseMessage response = await postsAPIServices.CreatePost(postCreated);
                if (response.IsSuccessStatusCode)
                {
                    MessageBoxResult result = MessageBox.Show("Publicación creada exitosamente, se te redirigirá a la página principal", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (result == MessageBoxResult.OK)
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
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("Tuvimos un error al crear tu publicación, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No puede dejar campos vacíos", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool ValidateNullFields()
        {
            bool fullFields = false;
            if (!String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(body) && !String.IsNullOrWhiteSpace(target))
            {
                fullFields = true;
            }
            return fullFields;
        }

        private void AddPhotos(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Todos los archivos|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                // No se realiza ninguna acción adicional aquí
            }
        }

        private async void EditPost(object sender, RoutedEventArgs e)
        {
            title = TextBox_Title.Text;
            body = TextBox_Body.Text;
            if (ComboBox_Target.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)ComboBox_Target.SelectedItem;
                string selectedValue = selectedItem.Content.ToString();

                if (selectedValue == "Estudiantes")
                {
                    target = "STUDENT";
                }
                else if (selectedValue == "Académicos")
                {
                    target = "ACADEMIC";
                }
                else if (selectedValue == "Todos")
                {
                    target = "EVERYBODY";
                }
            }
            else
            {
                MessageBox.Show("Selecciona una etiqueta de a quienés irá dirigida la publicación", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            bool withoutNullFields = ValidateNullFields();
            if (withoutNullFields)
            {
                postToEdit.title = title;
                postToEdit.body = body;
                postToEdit.target = target;
                postToEdit.dateOfPublish = dateOfPublish;
                HttpResponseMessage response = await postsAPIServices.EditPost(postToEdit);
                if (response.IsSuccessStatusCode)
                {
                    MessageBoxResult result = MessageBox.Show("Publicación editada exitosamente, se te redirigirá a la página principal", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (result == MessageBoxResult.OK)
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
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("Tuvimos un error al editar tu publicación, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No puede dejar campos vacíos", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void FindUser(object sender, RoutedEventArgs e)
        {
            string username = "";
            username = MainBar.TextBox_Search.Text;
            if (username != "")
            {
                this.NavigationService.Navigate(new SearchResults(username));
            }
        }
    }
}
