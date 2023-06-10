﻿using FeiHub.Models;
using FeiHub.Services;
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
        public NewPost()
        {
            InitializeComponent();
        }

        public NewPost(string idPublicacion)
        {
            InitializeComponent();
            Label_PageTitle.Content = "Editar Publicación";
            TextBox_Title.Text = "Aquí va el titulo recuperado";
            TextBox_Body.Text = "Aquí va toda la descrípción";
            Button_Post.Visibility = Visibility.Collapsed;
            Button_SaveChanges.Visibility = Visibility.Visible;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
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

        
    }
}
