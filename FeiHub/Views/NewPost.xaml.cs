using FeiHub.Models;
using FeiHub.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

        List<string> selectedFilePaths = new List<string>();
        string title = "";
        string body = "";
        string target = "";
        string author = SingletonUser.Instance.Username;
        DateTime dateOfPublish = DateTime.Now.Date;
        Photo[] photos = null;
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        S3Service s3Service = new S3Service();
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
            MessageBox.Show("No podemos actualizar las fotos de tu publicación, pero pronto lo agregaremos", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUser;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            postToEdit = post;
            Label_PageTitle.Content = "Editar Publicación";
            TextBox_Title.Text = postToEdit.title;
            TextBox_Body.Text = postToEdit.body;
            TextBlock_PhotosName.Visibility = Visibility.Collapsed;
            TextBlock_TitlePhotos.Visibility = Visibility.Collapsed;
            Button_Photos.Visibility = Visibility.Collapsed;
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
                postCreated.photos = new Photo[0];
                postCreated.dateOfPublish = dateOfPublish;
                var loadWindow = CreateWindow();
                loadWindow.Show();
                Posts newPost = await postsAPIServices.CreatePost(postCreated);
                if (newPost.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    if (selectedFilePaths.Count > 0)
                    {

                        int counter = 1;
                        List<Photo> tempPhotos = new List<Photo>(newPost.photos);
                        foreach (string imagePath in selectedFilePaths)
                        {
                            string customName = $"{newPost.id}{counter++}";
                            bool uploadSuccess = await s3Service.UploadImage(imagePath, customName);

                            if (uploadSuccess)
                            {
                                string imageUrl = s3Service.GetImageURL(customName);
                                tempPhotos.Add(new Photo { url = imageUrl });
                            }
                        }
                        newPost.photos = tempPhotos.ToArray();
                        HttpResponseMessage response = await postsAPIServices.EditPost(newPost);
                        if (response.IsSuccessStatusCode)
                        {
                            loadWindow.Close();
                            MessageBoxResult result = MessageBox.Show("Publicación creada exitosamente, se te redirigirá a la página principal", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (result == MessageBoxResult.OK)
                            {
                                this.NavigationService.Navigate(new MainPage());
                            }
                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            loadWindow.Close();
                            MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                            SingletonUser.Instance.BorrarSinglenton();
                            this.NavigationService.Navigate(new LogIn());
                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            loadWindow.Close();
                            MessageBox.Show("Tuvimos un error al crear tu publicación, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        postCreated.photos = new Photo[0];
                    }
                }
                if (newPost.StatusCode == System.Net.HttpStatusCode.Unauthorized) 
                {
                    loadWindow.Close();
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (newPost.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    loadWindow.Close();
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
            selectedFilePaths.Clear();
            TextBlock_PhotosName.Text = "";
            TextBlock_PhotosName.Visibility = Visibility.Visible;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePaths.AddRange(openFileDialog.FileNames);
                List<string> namesImages = new List<string>();

                foreach (string pathImage in selectedFilePaths)
                {
                    string nameImage = System.IO.Path.GetFileName(pathImage);
                    namesImages.Add(nameImage);
                }

                string nombresSeparados = string.Join(", ", namesImages);

                TextBlock_PhotosName.Text = nombresSeparados;
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
                var loadWindow = CreateWindow();
                loadWindow.Show();
                HttpResponseMessage response = await postsAPIServices.EditPost(postToEdit);
                loadWindow.Close();
                if (response.IsSuccessStatusCode)
                {
                    loadWindow.Close();
                    MessageBoxResult result = MessageBox.Show("Publicación editada exitosamente, se te redirigirá a la página principal", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (result == MessageBoxResult.OK)
                    {
                        this.NavigationService.Navigate(new MainPage());
                    }
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    loadWindow.Close();
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    loadWindow.Close();
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
        private Window CreateWindow()
        {
            var emergentWindow = new Window
            {
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = Brushes.Transparent,
                Width = 300,
                Height = 150,
                Topmost = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new Grid
                {
                    Background = Brushes.White,
                    Margin = new Thickness(10),
                }
            };

            var grid = emergentWindow.Content as Grid;

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            var textBlock = new TextBlock
            {
                Text = "Cargando publicación, porfavor espere",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var progressBar = new ProgressBar
            {
                IsIndeterminate = true,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 20,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Grid.SetRow(textBlock, 0);
            Grid.SetRow(progressBar, 1);

            grid.Children.Add(textBlock);
            grid.Children.Add(progressBar);

            return emergentWindow;
        }
    }
}
