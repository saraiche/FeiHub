using FeiHub.Models;
using FeiHub.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
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
    /// Lógica de interacción para EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Page
    {
        string name = "";
        string paternalSurname = "";
        string maternalSurname = "";
        string photoUrl = "";
        string pathProfilePhoto = "";
        User userToEdit = new User();
        S3Service s3Service = new S3Service();
        UsersAPIServices usersAPIServices = new UsersAPIServices();
        public EditProfile(User user)
        {
            InitializeComponent();
            userToEdit = user;
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += Search;
            this.MainBar.Button_Profile.Click += GoToProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            TextBox_Name.Text = userToEdit.name;
            TextBox_PaternalSurname.Text = userToEdit.paternalSurname;
            TextBox_MaternalSurname.Text = userToEdit.maternalSurname;
            photoUrl = userToEdit.profilePhoto;
            if (photoUrl == null )
            {
                ImageSourceConverter converter = new ImageSourceConverter();
                ImageBrush_ProfilePhoto.ImageSource = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            }
            else
            {
                ImageBrush_ProfilePhoto.ImageSource = new BitmapImage(new Uri(photoUrl));
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres cancelar la edición de tu perfil?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                SingletonUser.Instance.BorrarSinglenton();
                this.NavigationService.Navigate(new LogIn());
            }
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres cancelar la edición de tu perfil?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new Profile(SingletonUser.Instance.Username));
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres cancelar la edición de tu perfil?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string stringToSearch = this.MainBar.TextBox_Search.Text;
                if (stringToSearch != "")
                {
                    this.NavigationService.Navigate(new SearchResults(stringToSearch));
                }
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres cancelar la edición de tu perfil?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.NavigationService.GoBack();
            }
        }

        private async void SaveChanges(object sender, RoutedEventArgs e)
        {
            name = TextBox_Name.Text;
            paternalSurname = TextBox_PaternalSurname.Text;
            maternalSurname = TextBox_MaternalSurname.Text;
            bool withoutNullFields = ValidateNullFields();
            if (withoutNullFields)
            {
                if (!String.IsNullOrEmpty(pathProfilePhoto))
                {
                    var loadWindow = CreateWindow();
                    loadWindow.Show();
                    bool uploadSuccess = await s3Service.UploadImage(pathProfilePhoto, SingletonUser.Instance.Username);
                    if (uploadSuccess)
                    {
                        string imageUrl = s3Service.GetImageURL(SingletonUser.Instance.Username);
                        userToEdit.profilePhoto = imageUrl;
                        userToEdit.name = name;
                        userToEdit.paternalSurname = paternalSurname;
                        userToEdit.maternalSurname = maternalSurname;
                        HttpResponseMessage response = await usersAPIServices.EditUser(userToEdit);
                        if (response.IsSuccessStatusCode)
                        {
                            loadWindow.Close();
                            MessageBoxResult result = MessageBox.Show("Perfil editado correctamente, se te regresará a tu perfil", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (result == MessageBoxResult.OK)
                            {
                                this.NavigationService.Navigate(new Profile(SingletonUser.Instance.Username));
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
                            MessageBox.Show("Tuvimos un error al editar tu perfil, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    userToEdit.name = name;
                    userToEdit.paternalSurname = paternalSurname;
                    userToEdit.maternalSurname = maternalSurname;
                    var loadWindow = CreateWindow();
                    loadWindow.Show();
                    HttpResponseMessage response = await usersAPIServices.EditUser(userToEdit);
                    if (response.IsSuccessStatusCode)
                    {
                        loadWindow.Close();
                        MessageBoxResult result = MessageBox.Show("Perfil editado correctamente, se te regresará a tu perfil", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (result == MessageBoxResult.OK)
                        {
                            this.NavigationService.Navigate(new Profile(SingletonUser.Instance.Username));
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
                        MessageBox.Show("Tuvimos un error al editar tu perfil, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ChangeProfilePhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                pathProfilePhoto = openFileDialog.FileName;
                ImageSourceConverter converter = new ImageSourceConverter();
                ImageBrush_ProfilePhoto.ImageSource = (ImageSource)converter.ConvertFromString(pathProfilePhoto);
            }

        }
        public bool ValidateNullFields()
        {
            bool fullFields = false;
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(paternalSurname) && !String.IsNullOrWhiteSpace(maternalSurname))
            {
                fullFields = true;
            }
            return fullFields;
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
                Text = "Modificando tu perfil, porfavor espere",
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
